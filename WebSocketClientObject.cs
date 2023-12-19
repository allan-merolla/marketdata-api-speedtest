using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SpeedTest
{
    public class WebSockeClientObject : IDisposable
    {
        public int ReceiveBufferSize { get; set; } = 8192;
        public event callbackHandler processWebSocketMessage;
        public delegate void callbackHandler(string message);
        int reconnectAttempts = 0;

        public async Task ConnectAsync(string url, bool autoreconnect)
        {
            if (WS != null)
            {
                if (WS.State == WebSocketState.Open) return;
                else WS.Dispose();
            }
            WS = new ClientWebSocket();
            if (CTS != null) CTS.Dispose();
            CTS = new CancellationTokenSource();
            await WS.ConnectAsync(new Uri(url), CTS.Token);
            await Task.Factory.StartNew(ReceiveLoop, CTS.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            bool loop = true;
            if (autoreconnect)
            {
                while (loop)
                {
                    if (WS.State == WebSocketState.Closed || WS.State == WebSocketState.Aborted)
                    {
                        reconnectAttempts++;
                        Console.WriteLine("Failed to reconnect to Benzinga after "+reconnectAttempts.ToString()+" attempts.");
                        loop = false;
                        await Task.Delay(1000);
                        await this.ConnectAsync(url, true);
                    }
                    await Task.Delay(5000);
                }
            }
        }

        public async Task DisconnectAsync()
        {
            if (WS is null) return;
            // TODO: requests cleanup code, sub-protocol dependent.
            if (WS.State == WebSocketState.Open)
            {
                CTS.CancelAfter(TimeSpan.FromSeconds(2));
                await WS.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await WS.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            WS.Dispose();
            WS = null;
            CTS.Dispose();
            CTS = null;
        }

        private async Task ReceiveLoop()
        {
            var loopToken = CTS.Token;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[ReceiveBufferSize];
            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    outputStream = new MemoryStream(ReceiveBufferSize);
                    do
                    {
                        receiveResult = await WS.ReceiveAsync(buffer, CTS.Token);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    outputStream.Position = 0;
                    ResponseReceived(outputStream);
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                outputStream?.Dispose();
            }
        }

        private void ResponseReceived(Stream inputStream)
        {
            StreamReader rd = new StreamReader(inputStream);
            string result = rd.ReadToEnd();
            processWebSocketMessage(result);
        }

        public void Dispose() => DisconnectAsync().Wait();

        private ClientWebSocket WS;
        private CancellationTokenSource CTS;

    }
}
