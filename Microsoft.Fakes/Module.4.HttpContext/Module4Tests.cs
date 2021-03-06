﻿namespace Module._4.HttpContext
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Web.Fakes;
    using System.Web.SessionState.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Module4Tests
    {
        [TestMethod]
        public void IsVisible_WithNoRole_ShouldReturnFalse()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var user = new GenericPrincipal(
                    new GenericIdentity(WindowsIdentity.GetAnonymous().Name),
                    Enumerable.Empty<string>()
                        .ToArray());

                var context = new ShimHttpContext { UserGet = () => user };
                ShimHttpContext.CurrentGet = () => context;

                var node = new Node { Path = "path/to/the/node", Role = "Administrator" };

                var visibilityProvider = new VisibilityProvider();
                
                // act
                var isVisible = visibilityProvider.IsVisible(node);

                // assert
                Assert.IsFalse(isVisible);
            }
        }

        [TestMethod]
        public void IsVisible_WithRequiredRole_ShouldReturnTrue()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var user = new GenericPrincipal(WindowsIdentity.GetCurrent(), new[] { "Administrator" });

                var context = new ShimHttpContext { UserGet = () => user };
                ShimHttpContext.CurrentGet = () => context;

                var node = new Node { Path = "path/to/the/node", Role = "Administrator" };

                var visibilityProvider = new VisibilityProvider();
                
                // act
                var isVisible = visibilityProvider.IsVisible(node);

                // assert
                Assert.IsTrue(isVisible);
            }
        }

        [TestMethod]
        public void IsAuthorized_AuthToken123_ShouldReturnTrue()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var session = new ShimHttpSessionState
                                  {
                                      SessionIDGet = () => "MyTestSessionId",
                                      ItemGetString = (key) => key == "auth-token" ? "123" : null 
                                  };

                var context = new ShimHttpContext { SessionGet = () => session };
                ShimHttpContext.CurrentGet = () => context;

                // act
                var isAuthorized = Session123AuthProivder.IsAuthorized();

                // assert
                Assert.IsTrue(isAuthorized);
            }
        }

        [TestMethod]
        public void IsAuthorized_AuthToken456_ShouldReturnFalse()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var session = new ShimHttpSessionState
                {
                    SessionIDGet = () => "MyTestSessionId",
                    ItemGetString = (key) => key == "auth-token" ? "456" : null
                };

                var context = new ShimHttpContext { SessionGet = () => session };
                ShimHttpContext.CurrentGet = () => context;

                // act
                var isAuthorized = Session123AuthProivder.IsAuthorized();

                // assert
                Assert.IsFalse(isAuthorized);
            }
        }

        [TestMethod]
        public void ResponseCookie_Named_ShouldBeGenerated()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var cookieSet = false;
                var cookies = new ShimHttpCookieCollection
                                  {
                                      AddHttpCookie = (cookie) =>
                                          {
                                              if (cookie.Name == "MyTestCookie")
                                              {
                                                  cookieSet = true;
                                              }
                                          }
                                  };

                var response = new ShimHttpResponse
                {
                    CookiesGet = () => cookies
                };

                var context = new ShimHttpContext { ResponseGet = () => response };
                ShimHttpContext.CurrentGet = () => context;

                // act
                CookieGenerator.GenerateCookie("MyTestCookie", "123", new DateTime(2014, 06, 30));

                // assert
                Assert.IsTrue(cookieSet);
            }
        }

        [TestMethod]
        public void ResponseCookie_TooOld_ShouldBeExpired()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var hasExpired = false;
                var cookies = new ShimHttpCookieCollection
                {
                    AddHttpCookie = (cookie) =>
                        {
                            hasExpired = cookie.Expires <= DateTime.Now;
                        }
                };

                var response = new ShimHttpResponse
                {
                    CookiesGet = () => cookies
                };

                var context = new ShimHttpContext { ResponseGet = () => response };
                ShimHttpContext.CurrentGet = () => context;

                // act
                CookieGenerator.GenerateCookie("MyTestCookie", "123", new DateTime(2014, 05, 31));

                // assert
                Assert.IsTrue(hasExpired);
            }
        }
    }
}
