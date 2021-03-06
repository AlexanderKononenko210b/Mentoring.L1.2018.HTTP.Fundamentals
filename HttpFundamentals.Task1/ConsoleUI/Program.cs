﻿using System;
using System.Collections.Generic;
using System.Configuration;
using ConsoleUI.Parameters;
using SiteAnalyzer.Infrastructure.Interfaces;
using SiteAnalyzer;
using SiteAnalyzer.Validators;

namespace ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parameters = new ConsoleParameters();

            if (!CommandLine.Parser.Default.ParseArguments(args, parameters))
            {
                return;
            }

            var directory = ConfigurationManager.AppSettings["output"];
            var contentValidator = new Validator(new List<IConstraintRule>
            {
                new ExtensionConstraint(parameters.ContentExtensions)
            });
            var logger = new Logger();
            var siteSaver = new SiteSaver(directory, logger);
            var siteDownloader = new SiteDownloader(logger, siteSaver, contentValidator);
            var urlValidator = new Validator(new List<IConstraintRule>
            {
                new SchemeConstraint(),
                new DomainConstraint(new Uri(parameters.BaseUrl), parameters.DomainRestriction)
            });
            var siteManager = new SiteManager(siteDownloader, urlValidator, logger);
            var listUrl = new List<Uri> { new Uri(parameters.BaseUrl) };
            var countLevel = 0;
            
            try
            {
                siteManager.Start(listUrl, parameters.MaxDeepLevel, countLevel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}