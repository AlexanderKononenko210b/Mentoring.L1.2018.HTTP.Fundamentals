﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpListener.BusinessLayer;
using HttpListener.BusinessLayer.Converters;
using HttpListener.BusinessLayer.Infrastructure.Interfaces;
using HttpListener.BusinessLayer.Parsers;
using HttpListener.DataLayer;
using NUnit.Framework;

namespace HttpListener.Test
{
    [TestFixture]
    public class ListenerTest
    {
        private readonly string ExcelAcceptType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly string TextXmlAcceptType = "text/xml";
        private readonly string ApplicationXmlAcceptType = "application/xml";
        private readonly string DefaultAcceptType = "custom/type";

        private readonly NorthwindContext _context;
        private readonly ListenerService _service;
        private readonly HttpClient _client;
        private readonly UriBuilder _uriBuilder;
        private readonly IConverter _converter;

        public ListenerTest()
        {
            _context = new NorthwindContext();
            var parser = new Parser();
            var orderRepository = new OrderRepository(_context);
            var converter = new Converter();
            _service = new ListenerService(parser, orderRepository, converter);
            Thread listenerThread = new Thread(_service.Listen);
            listenerThread.Start();
            _client = new HttpClient();
            _uriBuilder = new UriBuilder("http://localhost:81");
            _converter = new Converter();
        }

        [Test]
        public async Task Listener_Get_Excel_Data_Accept_ExcelAcceptType()
        {
                _uriBuilder.Query = "customerId=VINET";
                _client.DefaultRequestHeaders.Add("Accept", ExcelAcceptType);
                var response = await _client.GetAsync(_uriBuilder.Uri);
                var contentType = response.Content.Headers.ContentType.MediaType;

                Assert.AreEqual(true, response.IsSuccessStatusCode);
                Assert.AreEqual(ExcelAcceptType, contentType);
        }

        [Test]
        public async Task Listener_Get_Xml_Data_Accept_TextXmlAcceptType()
        {
            _uriBuilder.Query = "customerId=VINET";
            _client.DefaultRequestHeaders.Add("Accept", TextXmlAcceptType);
            var response = await _client.GetAsync(_uriBuilder.Uri);
            var contentType = response.Content.Headers.ContentType.MediaType;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var data = _converter.FromXmlFormat(stream);
                Assert.True(data.Any());
            }

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(TextXmlAcceptType, contentType);
        }

        [Test]
        public async Task Listener_Get_Xml_Data_Accept_ApplicationXmlAcceptType()
        {
            _uriBuilder.Query = "customerId=VINET";
            _client.DefaultRequestHeaders.Add("Accept", ApplicationXmlAcceptType);
            var response = await _client.GetAsync(_uriBuilder.Uri);
            var contentType = response.Content.Headers.ContentType.MediaType;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var data = _converter.FromXmlFormat(stream);
                Assert.True(data.Any());
            }

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ApplicationXmlAcceptType, contentType);
        }

        [Test]
        public async Task Listener_Get_Xml_Data_Accept_DefaultAcceptType()
        {
            _uriBuilder.Query = "customerId=VINET";
            _client.DefaultRequestHeaders.Add("Accept", DefaultAcceptType);
            var response = await _client.GetAsync(_uriBuilder.Uri);
            var contentType = response.Content.Headers.ContentType.MediaType;

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ExcelAcceptType, contentType);
        }
    }
}
