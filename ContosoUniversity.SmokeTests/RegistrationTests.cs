using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace ContosoUniversity.SmokeTests
{
    public class RegistrationTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private string _appUrl = "http://localhost:41787/";

        public RegistrationTests()
        {
            _driver = new ChromeDriver();
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
