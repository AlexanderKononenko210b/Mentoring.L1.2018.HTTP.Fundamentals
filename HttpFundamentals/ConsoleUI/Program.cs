using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI.ConsoleParameters;
using SiteAnalizer.Infrastructure.Interfaces;
using SiteAnalyzer;
using SiteAnalyzer.Validators;

namespace ConsoleTest
{
    public class Program
    {
        private static readonly ILogger _logger = new Logger();

        public static void Main(string[] args)
        {
            var parameters = new ConsoleParameters();

            if (!CommandLine.Parser.Default.ParseArguments(args, parameters))
            {
                return;
            }

            var siteDownloader = new SiteDownloader(_logger);
            var urlValidator = new UrlValidator(new List<IConstraintRule> { new SchemeConstraint() });
            var siteSaver = new SiteSaver();
            var siteManager = new SiteManager(siteDownloader, urlValidator, siteSaver, _logger, parameters.MaxDeepLevel);
            var uri = new Uri(parameters.BaseUrl);
            var countLevel = 0;
            
            try
            {
                Parsing(siteManager, uri, parameters.MaxDeepLevel, countLevel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Parsing using recursive call method Start.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="baseUri"></param>
        /// <param name="maxDeepLevel"></param>
        /// <param name="countLevel"></param>
        private static void Parsing(SiteManager manager, Uri baseUri, int maxDeepLevel, int countLevel)
        {
            var listUrl = new List<Uri> { baseUri };

            var links = manager.StartAnalyze(listUrl, countLevel);

            if (links == null || !links.Any()) return;

            _logger.Log(links);
            ++countLevel;

            if (countLevel > maxDeepLevel) return;

            _logger.Log($"Level {countLevel}");

            foreach (var uri in links)
            {
                Parsing(manager, uri, maxDeepLevel, countLevel);
            }
        }
    }
}