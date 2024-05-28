using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System;

namespace AtmiraPayNet.RPA.Pages
{
    internal class PaymentPage(IWebDriver driver)
    {
        private readonly IWebDriver driver = driver;

        IWebElement SourceIBAN => driver.FindElement(By.Id("SourceIBAN"));
        IWebElement SourceBankName => driver.FindElement(By.Id("SourceBankName"));
        IWebElement SourceBankCountry => driver.FindElement(By.Id("SourceBankCountry"));
        IWebElement PostalCode => driver.FindElement(By.Id("PostalCode"));
        IWebElement Street => driver.FindElement(By.Id("Street"));
        IWebElement Number => driver.FindElement(By.Id("Number"));
        IWebElement City => driver.FindElement(By.Id("City"));
        IWebElement Country => driver.FindElement(By.Id("Country"));
        IWebElement Amount => driver.FindElement(By.Id("Amount"));
        IWebElement DestinationIBAN => driver.FindElement(By.Id("DestinationIBAN"));
        IWebElement DestinationBankName => driver.FindElement(By.Id("DestinationBankName"));
        IWebElement DestinationBankCountry => driver.FindElement(By.Id("DestinationBankCountry"));
        IWebElement IntermediaryIBAN => driver.FindElement(By.Id("IntermediaryIBAN"));
        IWebElement IntermediaryBankName => driver.FindElement(By.Id("IntermediaryBankName"));
        IWebElement IntermediaryBankCountry => driver.FindElement(By.Id("IntermediaryBankCountry"));
        IWebElement BtnGeneratePayment => driver.FindElement(By.Id("GeneratePaymentButton"));
        IWebElement BtnSavePayment => driver.FindElement(By.Id("SavePaymentButton"));

        public PaymentModel GetPayment()
        {
            return new PaymentModel
            {
                SourceIBAN = SourceIBAN.GetAttribute("value"),
                SourceBankName = SourceBankName.GetAttribute("value"),
                SourceBankCountry = SourceBankCountry.GetAttribute("value"),
                PostalCode = PostalCode.GetAttribute("value"),
                Street = Street.GetAttribute("value"),
                Number = Number.GetAttribute("value"),
                City = City.GetAttribute("value"),
                Country = Country.GetAttribute("value"),
                Amount = Amount.GetAttribute("value"),
                DestinationIBAN = DestinationIBAN.GetAttribute("value"),
                DestinationBankName = DestinationBankName.GetAttribute("value"),
                DestinationBankCountry = DestinationBankCountry.GetAttribute("value"),
                IntermediaryIBAN = IntermediaryIBAN.GetAttribute("value"),
                IntermediaryBankName = IntermediaryBankName.GetAttribute("value"),
                IntermediaryBankCountry = IntermediaryBankCountry.GetAttribute("value")
            };
        }

        public void CreateUpdatePayment(PaymentModel paymentModel, Status status)
        {
            SourceIBAN.SendKeys(paymentModel.SourceIBAN);
            SourceBankName.SendKeys(paymentModel.SourceBankName);
            SourceBankCountry.SendKeys(paymentModel.SourceBankCountry);
            PostalCode.SendKeys(paymentModel.PostalCode);
            Street.SendKeys(paymentModel.Street);
            Number.SendKeys(paymentModel.Number.ToString());
            City.SendKeys(paymentModel.City);
            Country.SendKeys(paymentModel.Country);
            Amount.SendKeys(paymentModel.Amount.ToString());
            DestinationIBAN.SendKeys(paymentModel.DestinationIBAN);
            DestinationBankName.SendKeys(paymentModel.DestinationBankName);
            DestinationBankCountry.SendKeys(paymentModel.DestinationBankCountry);
            IntermediaryIBAN.SendKeys(paymentModel.IntermediaryIBAN);
            IntermediaryBankName.SendKeys(paymentModel.IntermediaryBankName);
            IntermediaryBankCountry.SendKeys(paymentModel.IntermediaryBankCountry);

            if (status == Status.Generated)
            {
                BtnGeneratePayment.Click();
            }
            else
            {
                BtnSavePayment.Click();
            }
        }

        public void OpenPayment()
        {
            driver.Navigate().GoToUrl("https://localhost:7038/payment");
        }

        //public List<string> GetCountries()
        //{
        //private void Get_Countries()
        //{
        //    ChromeDriver driver = new();

        //    try
        //    {
        //        driver.Navigate().GoToUrl("https://localhost:7038/login");

        //        WebDriverWait loadingWait = new(driver, TimeSpan.FromSeconds(20));
        //        if (!loadingWait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loading"))))
        //        {
        //            return;
        //        }

        //        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

        //        string userName = Properties.Settings.Default.UserName;
        //        string password = Properties.Settings.Default.Password;

        //        IWebElement txtUserName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("UserName")));
        //        txtUserName.SendKeys(userName);

        //        IWebElement txtPassword = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Password")));
        //        txtPassword.SendKeys(password);

        //        IWebElement btnLogin = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("LoginButton")));
        //        btnLogin.Click();


        //        while (driver.Url.Contains("login"))
        //        {
        //            try
        //            {
        //                if (driver.FindElement(By.ClassName("swal2-container")).Displayed)
        //                {
        //                    return;
        //                }
        //            }
        //            catch (NoSuchElementException)
        //            {
        //                Thread.Sleep(200);
        //            }
        //        }

        //        driver.Navigate().GoToUrl("https://localhost:7038/payment");

        //        loadingWait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loading")));

        //        IWebElement sourceCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SourceCountry")));

        //        SelectElement select = new(sourceCountry);

        //        foreach (IWebElement option in select.Options)
        //        {
        //            Countries.Add(option.Text);
        //        }
        //    }
        //    finally
        //    {
        //        driver.Quit();
        //    }
        //}
        //}
    }
}
