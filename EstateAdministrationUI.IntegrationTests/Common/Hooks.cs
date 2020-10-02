﻿namespace EstateAdministrationUI.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BoDi;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Firefox;
    using TechTalk.SpecFlow;

    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer ObjectContainer;
        private IWebDriver WebDriver;

        public Hooks(IObjectContainer objectContainer)
        {
            this.ObjectContainer = objectContainer;
        }

        [BeforeScenario(Order = 0)]
        public async Task BeforeScenario()
        {
            String? browser = Environment.GetEnvironmentVariable("Browser");
            //browser = "Edge";

            if (browser == null || browser == "Chrome")
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--disable-gpu");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                var experimentalFlags = new List<String>();
                experimentalFlags.Add("same-site-by-default-cookies@2");
                experimentalFlags.Add("cookies-without-same-site-must-be-secure@2");
                options.AddLocalStatePreference("browser.enabled_labs_experiments", experimentalFlags);
                this.WebDriver = new ChromeDriver(options);
            }

            if (browser == "Firefox")
            {
                FirefoxOptions options = new FirefoxOptions();
                options.AddArguments("-headless");
                this.WebDriver = new FirefoxDriver(options);
            }

            if (browser == "Edge")
            {
                String? driverPath = Environment.GetEnvironmentVariable("DriverPath");
                String? driverExe = Environment.GetEnvironmentVariable("DriverExe");
                EdgeOptions options = new EdgeOptions();
                EdgeDriverService service = null;
                if (driverPath == null && driverExe == null)
                {
                    service = EdgeDriverService.CreateDefaultService(@"D:\Program Files (x86)\EdgeDriver\", "msedgedriver.exe");
                }
                else
                {
                    service = EdgeDriverService.CreateDefaultService(driverPath, driverExe);
                }

                this.WebDriver = new EdgeDriver(service, options);

            }

            this.ObjectContainer.RegisterInstanceAs(this.WebDriver);
        }

        [AfterScenario(Order = 0)]
        public void AfterScenario()
        {
            if (this.WebDriver != null)
            {
                this.WebDriver.Quit(); //.Dispose();
            }
        }
    }
}
