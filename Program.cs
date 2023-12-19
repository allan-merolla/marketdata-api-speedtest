using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using System.Text.Json;
using YahooFinance.NET;


namespace SpeedTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string url = string.Empty;
            HttpWebRequest httpRequest;
            HttpWebResponse httpResponse;

            string newsResult = string.Empty;
            Console.WriteLine("NewsFilterIO");
            var stpnews = new System.Diagnostics.Stopwatch();
            stpnews.Start();
            url = "http://stream.newsfilter.io?apiKey=xx";
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Proxy = null;
            httpRequest.Accept = "application/json";
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                newsResult = streamReader.ReadToEnd();
                Console.WriteLine(newsResult);
            }
            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(newsResult);
            stpnews.Stop();
            var ts2 = stpnews.Elapsed;
            var elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts2.Hours, ts2.Minutes, ts2.Seconds,
                 ts2.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime2);

            string result = string.Empty;
            Console.WriteLine("Alpha");
            var stp = new System.Diagnostics.Stopwatch();
            stp.Start();
            url = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=BABA&apikey=xxx";
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Proxy = null;
            httpRequest.Accept = "application/json";
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
            Console.WriteLine(httpResponse.StatusCode);
            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new DynamicJsonConverter());
            dynamic json = JsonSerializer.Deserialize<dynamic>(result, serializerOptions);
            IDictionary<string, object> dict = json as IDictionary<string, object>;
            IDictionary<string, object> dictValues = dict["Global Quote"] as IDictionary<string,object>;
            Console.WriteLine($"Last price: {dictValues["05. price"]}");
            stp.Stop();
            TimeSpan ts = stp.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            var stp2 = new System.Diagnostics.Stopwatch();
            stp2.Start();

            Console.WriteLine("YAHOO");
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;
                wc.Headers.Add("x-api-key", "xxxx");
                result = wc.DownloadString("https://yfapi.net/v6/finance/quote?symbols=WHC.AX&region=AU");
                Console.WriteLine(result);
            }
            stp2.Stop();
            ts = stp2.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.WriteLine("YAHOO");
            var stp3 = new System.Diagnostics.Stopwatch();
            stp3.Start();

            url = "https://rest.yahoofinanceapi.com/v6/finance/quote?symbols=WHC.AX&region=AU&lang=en";
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Proxy = null;
            httpRequest.Accept = "application/json";
            httpRequest.Headers.Add("x-api-key", "XWVOLRPCs15OhAiKS7qKp33hsyIOAqmB4BfCOerc");
            result = string.Empty;
            using (StreamReader reader = new StreamReader(httpRequest.GetResponse().GetResponseStream()))
            {
                result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new DynamicJsonConverter());
            stp3.Stop();
            ts = stp3.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            var stp4 = new System.Diagnostics.Stopwatch();
            Console.WriteLine("FINNHUB");
            stp4.Start();

            url = "https://finnhub.io/api/v1/quote?symbol=BABA&token=xxxx";
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Proxy = null;
            httpRequest.Accept = "application/json";
            result = string.Empty;
            using (StreamReader reader = new StreamReader(httpRequest.GetResponse().GetResponseStream()))
            {
                result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new DynamicJsonConverter());
            json = JsonSerializer.Deserialize<dynamic>(result, serializerOptions);
            dict = json as IDictionary<string, object>;
            stp4.Stop();
            ts = stp4.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            var stp5 = new System.Diagnostics.Stopwatch();
            Console.WriteLine("IEX");
            stp5.Start();

            url = "https://cloud.iexapis.com/stable/stock/aapl/quote?token=xxx";
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Proxy = null;
            httpRequest.Accept = "application/json";
            result = string.Empty;
            using (StreamReader reader = new StreamReader(httpRequest.GetResponse().GetResponseStream()))
            {
                result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new DynamicJsonConverter());
            json = JsonSerializer.Deserialize<dynamic>(result, serializerOptions);
            dict = json as IDictionary<string, object>;
            stp5.Stop();
            ts = stp5.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
