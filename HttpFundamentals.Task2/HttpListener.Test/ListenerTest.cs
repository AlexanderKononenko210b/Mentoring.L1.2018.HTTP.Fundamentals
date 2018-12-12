using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpListener.BusinessLayer;
using HttpListener.BusinessLayer.Converters;
using HttpListener.BusinessLayer.Parsers;
using HttpListener.DataLayer;
using NUnit.Framework;

namespace HttpListener.Test
{
    [TestFixture]
    public class ListenerTest
    {
        private ListenerService _listenerService;

        [SetUp]
        public void Initialize()
        {
            using (var context = new NorthwindContext())
            {
                var parser = new Parser();
                var orderRepository = new OrderRepository(context);
                var converter = new Converter();
                _listenerService = new ListenerService(parser, orderRepository, converter);
            }
        }

        [Test]
        public void Listener_Test()
        {
            using (var context = new NorthwindContext())
            {
                var parser = new Parser();
                var orderRepository = new OrderRepository(context);
                var converter = new Converter();
                _listenerService = new ListenerService(parser, orderRepository, converter);

                _listenerService.Listen();
            }

            //Thread listenerThread = new Thread(_listenerService.Listen);
            //listenerThread.Start();

            //using (var client = new HttpClient())
            //{
            //    UriBuilder builder = new UriBuilder("http://localhost:81");
            //    builder.Query = "customerId=10248";

            //    var result = client.GetAsync(builder.Uri).Result;

            //    using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            //    {
            //        Console.WriteLine(sr.ReadToEnd());
            //    }
            //}
        }
    }
}
