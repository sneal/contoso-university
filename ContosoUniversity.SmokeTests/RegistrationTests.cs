﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace ContosoUniversity.SmokeTests
{
    public class RegistrationTests : IDisposable
    {
        private const string DefaultAppUrl = "http://localhost:41787/";

        private readonly IWebDriver _driver;
        private readonly string _appUrl;

        public RegistrationTests()
        {
            var opt = new ChromeOptions();
            opt.AddArgument("headless");
            _driver = new ChromeDriver(opt);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _appUrl = new AppRoute().Url();
            if (string.IsNullOrEmpty(_appUrl))
            {
                _appUrl = DefaultAppUrl;
            }

            Console.WriteLine($"Using app URL: {_appUrl}");
        }

        [Fact]
        public void Student_can_register_for_classes()
        {
            _driver.Navigate().GoToUrl(_appUrl);
            _driver.FindElement(By.LinkText("Students")).Click();
            _driver.FindElement(By.LinkText("Register Now")).Click();

            // should be on login page
            _driver.FindElement(By.Id("Email")).SendKeys("yli@contoso.edu");
            _driver.FindElement(By.Id("Password")).SendKeys("keepitsimple");
            _driver.FindElement(By.ClassName("btn-default")).Click();

            // register for a class
            _driver.FindElement(By.LinkText("Register"));
        }

        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}
