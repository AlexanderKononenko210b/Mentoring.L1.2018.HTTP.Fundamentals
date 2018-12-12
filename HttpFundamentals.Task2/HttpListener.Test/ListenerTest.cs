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
        [Test]
        public void Listener_Test()
        {
            using (var context = new NorthwindContext())
            {
                var parser = new Parser();
                var orderRepository = new OrderRepository(context);
                var converter = new Converter();
                var listenerService = new ListenerService(parser, orderRepository, converter);

                listenerService.Listen();
            }
        }
    }
}
