﻿namespace EstateAdministrationUI.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Gherkin;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Shared.IntegrationTesting;
    using Shouldly;

    public static class Extensions
    {
        public static async Task FillIn(this IWebDriver webDriver,
                                        String elementName,
                                        String value,
                                        Boolean clearExistingText = false)
        {
            await Retry.For(async () =>
                            {
                                IWebElement webElement = webDriver.FindElement(By.Name(elementName));
                                webElement.ShouldNotBeNull();
                                webElement.Displayed.ShouldBe(true);
                                webElement.Enabled.ShouldBe(true);
                                if (clearExistingText)
                                {
                                    webElement.Clear();
                                }
                                webElement.SendKeys(value);
                            });
        }

        public static async Task<IWebElement> FindButtonById(this IWebDriver webDriver,
                                                             String buttonId)
        {
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                element = webDriver.FindElement(By.Id(buttonId));

                                element.ShouldNotBeNull();
                            });
            return element;
        }

        public static async Task<IWebElement> FindButtonByText(this IWebDriver webDriver,
                                                               String buttonText)
        {
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                ReadOnlyCollection<IWebElement> elements = webDriver.FindElements(By.TagName("button"));

                                List<IWebElement> e = elements.Where(e => e.GetAttribute("innerText") == buttonText).ToList();

                                e.ShouldHaveSingleItem();

                                element = e.Single();
                            });
            return element;
        }

        public static void ClickLink(this IWebDriver webDriver,
                                     String linkText)
        {
            IWebElement webElement = webDriver.FindElement(By.LinkText(linkText));
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static async Task ClickButtonById(this IWebDriver webDriver,
                                           String buttonId)
        {
            IWebElement webElement = await webDriver.FindButtonById(buttonId);
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static async Task ClickButtonByText(this IWebDriver webDriver,
                                             String buttonText)
        {
            IWebElement webElement = await webDriver.FindButtonByText(buttonText);
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static async Task SelectDropDownItemByText(this IWebDriver webDriver, String dropdownId, String textToSelect)
        {
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                element = webDriver.FindElement(By.Id(dropdownId));

                                element.ShouldNotBeNull();

                                SelectElement dropdown = new SelectElement(element);
                                dropdown.SelectByText(textToSelect);
                            });
        }
    }
}