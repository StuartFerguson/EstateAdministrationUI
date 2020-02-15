﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EstateAdministrationUI.IntegrationTests.Common
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.Logger;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private readonly TestingContext TestingContext;

        private readonly IWebDriver WebDriver;

        public SharedSteps(TestingContext testingContext, IWebDriver webDriver)
        {
            this.TestingContext = testingContext;
            this.WebDriver = webDriver;
        }

        [Given(@"I create the following roles")]
        public async Task GivenICreateTheFollowingRoles(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                CreateRoleRequest createRoleRequest = new CreateRoleRequest
                                                      {
                                                          RoleName = SpecflowTableHelper.GetStringRowValue(tableRow, "Role Name").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"))
                                                      };
                CreateRoleResponse createRoleResponse = await this.CreateRole(createRoleRequest, CancellationToken.None).ConfigureAwait(false);

                createRoleResponse.ShouldNotBeNull();
                createRoleResponse.RoleId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Roles.Add(createRoleRequest.RoleName, createRoleResponse.RoleId);
            }
        }

        private async Task<CreateRoleResponse> CreateRole(CreateRoleRequest createRoleRequest,
                                                          CancellationToken cancellationToken)
        {
            CreateRoleResponse createRoleResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateRole(createRoleRequest, cancellationToken).ConfigureAwait(false);
            return createRoleResponse;
        }


        private async Task<CreateApiResourceResponse> CreateApiResource(CreateApiResourceRequest createApiResourceRequest,
                                                                        CancellationToken cancellationToken)
        {
            CreateApiResourceResponse createApiResourceResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateApiResource(createApiResourceRequest, cancellationToken).ConfigureAwait(false);
            return createApiResourceResponse;
        }

        private async Task<CreateClientResponse> CreateClient(CreateClientRequest createClientRequest,
                                                              CancellationToken cancellationToken)
        {
            CreateClientResponse createClientResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateClient(createClientRequest, cancellationToken).ConfigureAwait(false);
            return createClientResponse;
        }

        [Given(@"I create the following api resources")]
        public async Task GivenICreateTheFollowingApiResources(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // Get the scopes
                String scopes = SpecflowTableHelper.GetStringRowValue(tableRow, "Scopes");
                String userClaims = SpecflowTableHelper.GetStringRowValue(tableRow, "UserClaims");
                scopes = scopes.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));

                CreateApiResourceRequest createApiResourceRequest = new CreateApiResourceRequest
                {
                    Secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret"),
                    Name = SpecflowTableHelper.GetStringRowValue(tableRow, "Name").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N")),
                    Scopes = string.IsNullOrEmpty(scopes) ? null : scopes.Split(",").ToList(),
                    UserClaims = string.IsNullOrEmpty(userClaims) ? null : userClaims.Split(",").ToList(),
                    Description = SpecflowTableHelper.GetStringRowValue(tableRow, "Description"),
                    DisplayName = SpecflowTableHelper.GetStringRowValue(tableRow, "DisplayName")
                };
                CreateApiResourceResponse createApiResourceResponse =
                    await this.CreateApiResource(createApiResourceRequest, CancellationToken.None).ConfigureAwait(false);

                createApiResourceResponse.ShouldNotBeNull();
                createApiResourceResponse.ApiResourceName.ShouldNotBeNullOrEmpty();

                this.TestingContext.ApiResources.Add(createApiResourceResponse.ApiResourceName);
            }
        }

        [Given(@"I create the following users")]
        public async Task GivenICreateTheFollowingUsers(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // Get the claims
                Dictionary<String, String> userClaims = null;
                String claims = SpecflowTableHelper.GetStringRowValue(tableRow, "Claims");
                if (string.IsNullOrEmpty(claims) == false)
                {
                    userClaims = new Dictionary<String, String>();
                    String[] claimList = claims.Split(",");
                    foreach (String claim in claimList)
                    {
                        // Split into claim name and value
                        String[] c = claim.Split(":");
                        userClaims.Add(c[0], c[1]);
                    }
                }

                String roles = SpecflowTableHelper.GetStringRowValue(tableRow, "Roles");
                roles = roles.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));

                CreateUserRequest createUserRequest = new CreateUserRequest
                {
                    EmailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "Email Address").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N")),
                    FamilyName = SpecflowTableHelper.GetStringRowValue(tableRow, "Family Name"),
                    GivenName = SpecflowTableHelper.GetStringRowValue(tableRow, "Given Name"),
                    PhoneNumber = SpecflowTableHelper.GetStringRowValue(tableRow, "Phone Number"),
                    MiddleName = SpecflowTableHelper.GetStringRowValue(tableRow, "Middle name"),
                    Claims = userClaims,
                    Roles = string.IsNullOrEmpty(roles) ? null : roles.Split(",").ToList(),
                    Password = SpecflowTableHelper.GetStringRowValue(tableRow, "Password")
                };
                CreateUserResponse createUserResponse = await this.CreateUser(createUserRequest, CancellationToken.None).ConfigureAwait(false);

                createUserResponse.ShouldNotBeNull();
                createUserResponse.UserId.ShouldNotBe(Guid.Empty);

                this.TestingContext.Users.Add(createUserRequest.EmailAddress, createUserResponse.UserId);
            }
        }

        private async Task<CreateUserResponse> CreateUser(CreateUserRequest createUserRequest,
                                                          CancellationToken cancellationToken)
        {
            CreateUserResponse createUserResponse = await this.TestingContext.DockerHelper.SecurityServiceClient.CreateUser(createUserRequest, cancellationToken).ConfigureAwait(false);
            return createUserResponse;
        }

        [Given(@"I create the following clients")]
        public async Task GivenICreateTheFollowingClients(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                // Get the scopes
                String scopes = SpecflowTableHelper.GetStringRowValue(tableRow, "Scopes");
                // Get the grant types
                String grantTypes = SpecflowTableHelper.GetStringRowValue(tableRow, "GrantTypes");
                // Get the redirect uris
                String redirectUris = SpecflowTableHelper.GetStringRowValue(tableRow, "RedirectUris");
                // Get the post logout redirect uris
                String postLogoutRedirectUris = SpecflowTableHelper.GetStringRowValue(tableRow, "PostLogoutRedirectUris");

                scopes = scopes.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N"));
                redirectUris = redirectUris.Replace("[port]", this.TestingContext.DockerHelper.EstateManagementUIPort.ToString());
                postLogoutRedirectUris = postLogoutRedirectUris.Replace("[port]", this.TestingContext.DockerHelper.EstateManagementUIPort.ToString());

                CreateClientRequest createClientRequest = new CreateClientRequest
                {
                    ClientId = SpecflowTableHelper.GetStringRowValue(tableRow, "ClientId").Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N")),
                    Secret = SpecflowTableHelper.GetStringRowValue(tableRow, "Secret"),
                    ClientName = SpecflowTableHelper.GetStringRowValue(tableRow, "Name"),
                    AllowedScopes = string.IsNullOrEmpty(scopes) ? null : scopes.Split(",").ToList(),
                    AllowedGrantTypes = string.IsNullOrEmpty(grantTypes) ? null : grantTypes.Split(",").ToList(),
                    ClientRedirectUris = string.IsNullOrEmpty(redirectUris) ? null : redirectUris.Split(",").ToList(),
                    ClientPostLogoutRedirectUris = string.IsNullOrEmpty(postLogoutRedirectUris) ? null : postLogoutRedirectUris.Split(",").ToList(),
                    ClientDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "Description"),
                    RequireConsent = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireConsent"),
                    AllowOfflineAccess = SpecflowTableHelper.GetBooleanValue(tableRow, "AllowOfflineAccess")

                };

                // Do the replacement on the Uris
                //if (createClientRequest.ClientRedirectUris != null && createClientRequest.ClientRedirectUris.Any())
                //{
                //    foreach (String clientRedirectUri in createClientRequest.ClientRedirectUris)
                //    {
                //        clientRedirectUri
                //    }
                //    createClientRequest.ClientRedirectUris.ForEach(c => c = c.Replace("[port]", this.TestingContext.DockerHelper.SecurityServiceTestUIPort.ToString()));
                //}

                //if (createClientRequest.ClientPostLogoutRedirectUris != null && createClientRequest.ClientPostLogoutRedirectUris.Any())
                //{
                //    createClientRequest.ClientPostLogoutRedirectUris.ForEach(c => c = c.Replace("[port]", this.TestingContext.DockerHelper.SecurityServiceTestUIPort.ToString()));
                //}

                CreateClientResponse createClientResponse = await this.CreateClient(createClientRequest, CancellationToken.None).ConfigureAwait(false);

                createClientResponse.ShouldNotBeNull();
                createClientResponse.ClientId.ShouldNotBeNullOrEmpty();

                this.TestingContext.Clients.Add(createClientResponse.ClientId);
            }
        }

        [Given(@"I am on the application home page")]
        public void GivenIAmOnTheApplicationHomePage()
        {
            this.WebDriver.Navigate().GoToUrl($"http://localhost:{this.TestingContext.DockerHelper.EstateManagementUIPort}");
            this.WebDriver.Title.ShouldBe("Welcome");
        }

        [Given(@"I click on the Sign In Button")]
        public void GivenIClickOnTheSignInButton()
        {
            this.WebDriver.ClickButtonById("loginButton");
        }

        [Then(@"I am presented with a login screen")]
        public void ThenIAmPresentedWithALoginScreen()
        {
            IWebElement loginButton = this.WebDriver.FindButtonByText("Login");
            loginButton.ShouldNotBeNull();
        }

        [When(@"I login with the username '(.*)' and password '(.*)'")]
        public void WhenILoginWithTheUsernameAndPassword(String userName, String password)
        {
            this.WebDriver.FillIn("Username", userName.Replace("[id]", this.TestingContext.DockerHelper.TestId.ToString("N")));
            this.WebDriver.FillIn("Password", password);
            this.WebDriver.ClickButtonByText("Login");
        }

        [Then(@"I am presented with the Estate Administrator Dashboard")]
        public void ThenIAmPresentedWithTheEstateAdministratorDashboard()
        {
            this.WebDriver.Title.ShouldBe("Dashboard");
        }

    }

    public static class Extensions
    {
        public static void FillIn(this IWebDriver webDriver,
                                  String elementName,
                                  String value)
        {
            IWebElement webElement = webDriver.FindElement(By.Name(elementName));
            webElement.ShouldNotBeNull();
            webElement.SendKeys(value);
        }

        public static IWebElement FindButtonById(this IWebDriver webDriver,
                                                   String buttonId)
        {
            IWebElement element = webDriver.FindElement(By.Id(buttonId));

            element.ShouldNotBeNull();

            return element;
        }

        public static IWebElement FindButtonByText(this IWebDriver webDriver,
                                             String buttonText)
        {
            ReadOnlyCollection<IWebElement> elements = webDriver.FindElements(By.TagName("button"));

            List<IWebElement> e = elements.Where(e => e.GetAttribute("innerText") == buttonText).ToList();

            e.ShouldHaveSingleItem();

            return e.Single();
        }

        public static void ClickLink(this IWebDriver webDriver,
                                     String linkText)
        {
            IWebElement webElement = webDriver.FindElement(By.LinkText(linkText));
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static void ClickButtonById(this IWebDriver webDriver,
                                       String buttonId)
        {
            IWebElement webElement = webDriver.FindButtonById(buttonId);
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static void ClickButtonByText(this IWebDriver webDriver,
                                           String buttonText)
        {
            IWebElement webElement = webDriver.FindButtonByText(buttonText);
            webElement.ShouldNotBeNull();
            webElement.Click();
        }
    }

    public class TestingContext
    {
        public DockerHelper DockerHelper { get; set; }

        public NlogLogger Logger { get; set; }

        public Dictionary<String, Guid> Users;
        public Dictionary<String, Guid> Roles;

        public List<String> Clients;

        public List<String> ApiResources;

        public TokenResponse TokenResponse;

        public TestingContext()
        {
            this.Users = new Dictionary<String, Guid>();
            this.Roles = new Dictionary<String, Guid>();
            this.Clients = new List<String>();
            this.ApiResources = new List<String>();
        }
    }

    public static class SpecflowTableHelper
    {
        #region Methods

        public static Decimal GetDecimalValue(TableRow row,
                                              String key)
        {
            String field = SpecflowTableHelper.GetStringRowValue(row, key);

            return Decimal.TryParse(field, out Decimal value) ? value : -1;
        }

        public static Boolean GetBooleanValue(TableRow row,
                                              String key)
        {
            String field = SpecflowTableHelper.GetStringRowValue(row, key);

            return Boolean.TryParse(field, out Boolean value) && value;
        }

        public static Int32 GetIntValue(TableRow row,
                                        String key)
        {
            String field = SpecflowTableHelper.GetStringRowValue(row, key);

            return Int32.TryParse(field, out Int32 value) ? value : -1;
        }

        public static Int16 GetShortValue(TableRow row,
                                          String key)
        {
            String field = SpecflowTableHelper.GetStringRowValue(row, key);

            if (Int16.TryParse(field, out Int16 value))
            {
                return value;
            }
            else
            {
                return -1;
            }
        }

        public static String GetStringRowValue(TableRow row,
                                               String key)
        {
            return row.TryGetValue(key, out String value) ? value : "";
        }

        /// <summary>
        /// Gets the date for date string.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="today">The today.</param>
        /// <returns></returns>
        public static DateTime GetDateForDateString(String dateString,
                                                    DateTime today)
        {
            switch (dateString.ToUpper())
            {
                case "TODAY":
                    return today.Date;
                case "YESTERDAY":
                    return today.AddDays(-1).Date;
                case "LASTWEEK":
                    return today.AddDays(-7).Date;
                case "LASTMONTH":
                    return today.AddMonths(-1).Date;
                case "LASTYEAR":
                    return today.AddYears(-1).Date;
                case "TOMORROW":
                    return today.AddDays(1).Date;
                default:
                    return DateTime.Parse(dateString);
            }
        }

        #endregion
    }
}
