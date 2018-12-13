using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;
using HttpListener.BusinessLayer.Infrastructure.Models;
using HttpListener.BusinessLayer.MapperConfigurations;
using HttpListener.DataLayer.Infrastructure.Interfaces;
using HttpListener.DataLayer.Infrastructure.Models;
using Mapper = AutoMapper.Mapper;

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
        private readonly IFilter<Order> _filter;

        public ListenerService(
            IParser parser,
            IRepository<Order> orderRepository,
            IConverter converter,
            IFilter<Order> filter)
        {
            _parser = parser;
            _orderRepository = orderRepository;
            _converter = converter;
            _filter = filter;
            Mapper.Reset();
            Mapper.Initialize(cfg => { cfg.AddProfile(new HttpListenerMappingProfile()); });
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

                var predicate = _filter.BuildExpression(searchInfo);
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
                            response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xlsx;");
                            break;
                        }
                        case "text/xml":
                        {
                            _converter.ToXmlFormat(data, memoryStream);
                            response.AppendHeader("Content-Type", "text/xml");
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xml");
                            break;
                        }
                        case "application/xml":
                        {
                            _converter.ToXmlFormat(data, memoryStream);
                            response.AppendHeader("Content-Type", "application/xml");
                            response.AppendHeader("Content-Disposition",
                                "attachment; filename=translationText.xml");
                            break;
                        }
                        default:
                        {
                            _converter.ToExcelFormat(data, memoryStream);
                            response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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
        private List<OrderView> GetData(SearchInfo searchInfo, Expression<Func<Order, bool>> predicate)
        {
            if (searchInfo.Take != null && searchInfo.Skip != null)
            {
                return _orderRepository.GetOrders(predicate)
                    .Skip(searchInfo.Skip.Value)
                    .Take(searchInfo.Take.Value)
                    .Select(order => Mapper.Map<OrderView>(order))
                    .OrderBy(order => order.OrderID)
                    .ToList();
            }

            if (searchInfo.Take == null && searchInfo.Skip.HasValue)
            {
                return _orderRepository.GetOrders(predicate)
                    .Skip(searchInfo.Skip.Value)
                    .Select(order => Mapper.Map<OrderView>(order))
                    .OrderBy(order => order.OrderID)
                    .ToList();
            }

            if (searchInfo.Skip == null && searchInfo.Take.HasValue)
            {
                return _orderRepository.GetOrders(predicate)
                    .Take(searchInfo.Take.Value)
                    .Select(order => Mapper.Map<OrderView>(order))
                    .OrderBy(order => order.OrderID)
                    .ToList();
            }

            return _orderRepository.GetOrders(predicate)
                .Select(order => Mapper.Map<OrderView>(order))
                .ToList();
        }
    }
}
