using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;
using HttpListener.BusinessLayer.Infrastructure.Models;
using HttpListener.DataLayer.Infrastructure.Interfaces;
using HttpListener.DataLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer
{
    /// <summary>
    /// Represents a <see cref="ListenerService"/> class.
    /// </summary>
    public class ListenerService
    {
        private readonly IParser _parser;
        private readonly IRepository<Order> _orderRepository;
        private readonly IConverter _converter;
        private readonly IMapper<Order, OrderView> _mapper;

        public ListenerService(
            IParser parser,
            IRepository<Order> orderRepository,
            IConverter converter,
            IMapper<Order, OrderView> mapper)
        {
            _parser = parser;
            _orderRepository = orderRepository;
            _converter = converter;
            _mapper = mapper;
        }

        public void Listen()
        {
            var listener = new System.Net.HttpListener();
            listener.Prefixes.Add("http://+:81/");
            listener.Start();

            do
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                if (request.Url.PathAndQuery == "/~close")
                {
                    response.Close();
                    break;
                }

                var searchInfo = new SearchInfo();

                if (request.HttpMethod == "POST" && request.InputStream != null)
                {
                    searchInfo = _parser.ParseBody(request.InputStream);
                }
                else
                {
                    var dataFromQuery = request.Url.ParseQueryString();
                    searchInfo = _parser.ParseQuery(dataFromQuery);
                }

                var predicate = GetPredicate(searchInfo);
                var data = GetData(searchInfo, predicate);
                var accept = GetAcceptType(request.AcceptTypes);

                SendResponse(accept, data, response);

            } while (true);

            listener.Close();
        }

        /// <summary>
        /// Send response.
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="data"></param>
        /// <param name="response"></param>
        private void SendResponse(string accept, List<OrderView> data, HttpListenerResponse response)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (accept != null)
                {
                    switch (accept)
                    {
                        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                        {
                            _converter.ToExcelFormat(data, memoryStream);
                            response.ContentType = "application / vnd.ms - excel";
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xlsx");
                            break;
                        }
                        case "text/xml":
                        {
                            _converter.ToXmlFormat(data, memoryStream);
                            response.ContentType = "text/xml";
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xml");
                            break;
                        }
                        case "application/xml":
                        {
                            _converter.ToXmlFormat(data, memoryStream);
                            response.ContentType = "application/xml";
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xml");
                            break;
                        }
                        default:
                        {
                            _converter.ToExcelFormat(data, memoryStream);
                            response.ContentType = "application / vnd.ms - excel";
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xlsx");
                            break;
                        }
                    }
                }

                response.StatusCode = (int) HttpStatusCode.OK;

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.WriteTo(response.OutputStream);
                response.OutputStream.Flush();
                response.OutputStream.Close();
            }
        }

        /// <summary>
        /// Get accept type for response.
        /// </summary>
        /// <param name="acceptTypes">The accepts types.</param>
        /// <returns></returns>
        private string GetAcceptType(string[] acceptTypes)
        {
            if (acceptTypes == null || !acceptTypes.Any())
            {
                return "unknown";
            }

            return acceptTypes.First();
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="searchInfo">The search info.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        private List<OrderView> GetData(SearchInfo searchInfo, Func<Order, bool> predicate)
        {
            var data = new List<Order>();

            if (searchInfo == null)
            {
                data = _orderRepository.GetOrders(order => predicate(order)).ToList();
            }
            else
            {
                if (searchInfo.CustomerId != 0)
                {
                    data = _orderRepository.GetOrders(order => predicate(order)).ToList();
                }
                else
                {
                    if (searchInfo.Skip != 0 && searchInfo.Take != 0)
                    {
                        data = _orderRepository.GetOrders(order => predicate(order)).Skip(searchInfo.Skip).Take(searchInfo.Take)
                            .ToList();
                    }
                    else
                    {
                        if (searchInfo.Skip == 0 && searchInfo.Take != 0)
                        {
                            data = _orderRepository.GetOrders(order => predicate(order)).Take(searchInfo.Take).ToList();
                        }
                        else
                        {
                            data = _orderRepository.GetOrders(order => predicate(order)).Skip(searchInfo.Skip).ToList();
                        }
                    }
                }
            }
            return data.Select(order => _mapper.Map(order)).ToList();
        }

        /// <summary>
        /// Get predicate.
        /// </summary>
        /// <param name="searchInfo">The search info.</param>
        /// <returns>The <see cref="Func{Order, bool}"/></returns>
        private Func<Order, bool> GetPredicate(SearchInfo searchInfo)
        {
            if (searchInfo == null)
            {
                return order => order.OrderID == order.OrderID;
            }

            if (searchInfo.CustomerId != 0)
            {
                return order => order.OrderID == searchInfo.CustomerId;
            }

            if (searchInfo.From != default(DateTime) && searchInfo.To != default(DateTime))
            {
                return order => order.OrderDate >= searchInfo.From && order.OrderDate <= searchInfo.To;
            }

            if (searchInfo.From == default(DateTime) && searchInfo.To != default(DateTime))
            {
                return order => order.OrderDate == searchInfo.To;
            }

            if (searchInfo.From != default(DateTime) && searchInfo.To == default(DateTime))
            {
                return order => order.OrderDate == searchInfo.From;
            }

            return order => order.OrderID == order.OrderID;
        }
    }
}
