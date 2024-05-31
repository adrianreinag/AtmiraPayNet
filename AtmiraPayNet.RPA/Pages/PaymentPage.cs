using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AtmiraPayNet.RPA.Pages
{
    internal class PaymentPage(IWebDriver driver)
    {
        private readonly IWebDriver _driver = driver;

        IWebElement SourceIBAN => _driver.FindElement(By.Id("SourceIBAN"));
        IWebElement SourceBankName => _driver.FindElement(By.Id("SourceBankName"));
        IWebElement SourceBankCountry => _driver.FindElement(By.Id("SourceBankCountry"));
        IWebElement PostalCode => _driver.FindElement(By.Id("PostalCode"));
        IWebElement Street => _driver.FindElement(By.Id("Street"));
        IWebElement Number => _driver.FindElement(By.Id("Number"));
        IWebElement City => _driver.FindElement(By.Id("City"));
        IWebElement Country => _driver.FindElement(By.Id("Country"));
        IWebElement Amount => _driver.FindElement(By.Id("Amount"));
        IWebElement DestinationIBAN => _driver.FindElement(By.Id("DestinationIBAN"));
        IWebElement DestinationBankName => _driver.FindElement(By.Id("DestinationBankName"));
        IWebElement DestinationBankCountry => _driver.FindElement(By.Id("DestinationBankCountry"));
        IWebElement IntermediaryIBAN => _driver.FindElement(By.Id("IntermediaryIBAN"));
        IWebElement IntermediaryBankName => _driver.FindElement(By.Id("IntermediaryBankName"));
        IWebElement IntermediaryBankCountry => _driver.FindElement(By.Id("IntermediaryBankCountry"));
        IWebElement BtnGeneratePayment => _driver.FindElement(By.Id("GeneratePaymentButton"));
        IWebElement BtnSavePayment => _driver.FindElement(By.Id("SavePaymentButton"));


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
            SourceIBAN.Clear();
            SourceIBAN.SendKeys(paymentModel.SourceIBAN);

            SourceBankName.Clear();
            SourceBankName.SendKeys(paymentModel.SourceBankName);

            SourceBankCountry.Clear();
            SourceBankCountry.SendKeys(paymentModel.SourceBankCountry);

            PostalCode.Clear();
            PostalCode.SendKeys(paymentModel.PostalCode);

            Street.Clear();
            Street.SendKeys(paymentModel.Street);

            Number.Clear();
            Number.SendKeys(paymentModel.Number.ToString());

            City.Clear();
            City.SendKeys(paymentModel.City);

            Country.Clear();
            Country.SendKeys(paymentModel.Country);

            Amount.Clear();
            Amount.SendKeys(paymentModel.Amount.ToString());

            DestinationIBAN.Clear();
            DestinationIBAN.SendKeys(paymentModel.DestinationIBAN);

            DestinationBankName.Clear();
            DestinationBankName.SendKeys(paymentModel.DestinationBankName);

            DestinationBankCountry.Clear();
            DestinationBankCountry.SendKeys(paymentModel.DestinationBankCountry);

            if (IntermediaryIBAN.Enabled)
            {
                IntermediaryIBAN.Clear();
                IntermediaryIBAN.SendKeys(paymentModel.IntermediaryIBAN);
            }

            if (IntermediaryBankName.Enabled)
            {
                IntermediaryBankName.Clear();
                IntermediaryBankName.SendKeys(paymentModel.IntermediaryBankName);
            }

            if (IntermediaryBankCountry.Enabled)
            {
                IntermediaryBankCountry.Clear();
                IntermediaryBankCountry.SendKeys(paymentModel.IntermediaryBankCountry);
            }

            if (status == Status.Generated)
            {
                BtnGeneratePayment.Click();
            }
            else
            {
                BtnSavePayment.Click();
            }
        }


        public List<string> GetCountries()
        {
            List<string> countries = [];

            WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(20));

            IWebElement sourceCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SourceBankCountry")));

            SelectElement select = new(sourceCountry);

            foreach (IWebElement option in select.Options)
            {
                countries.Add(option.Text);
            }

            return countries;
        }
    }
}
