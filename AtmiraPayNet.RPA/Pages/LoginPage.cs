using AtmiraPayNet.RPA.Models;
using OpenQA.Selenium;

namespace AtmiraPayNet.RPA.Pages
{
    internal class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            driver.Navigate().GoToUrl("https://localhost:7038/login");
        }

        IWebElement TxtUserName => _driver.FindElement(By.Id("UserName"));
        IWebElement TxtPassword => _driver.FindElement(By.Id("Password"));
        IWebElement BtnLogin => _driver.FindElement(By.Id("LoginButton"));

        public bool Login(LoginModel loginModel)
        {
            TxtUserName.SendKeys(loginModel.UserName);
            TxtPassword.SendKeys(loginModel.Password);
            BtnLogin.Click();

            while (_driver.Url.Contains("login"))
            {
                try
                {
                    if (_driver.FindElement(By.ClassName("swal2-container")).Displayed)
                    {
                        return false;
                    }
                }
                catch (NoSuchElementException)
                {
                    Thread.Sleep(200);
                }
            }

            return true;
        }

        public void OpenRegister()
        {
            _driver.Navigate().GoToUrl("https://localhost:7038/register");
        }
    }
}
