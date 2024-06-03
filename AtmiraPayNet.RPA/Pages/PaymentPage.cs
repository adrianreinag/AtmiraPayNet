using AtmiraPayNet.RPA.Models;
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

        IWebElement? FindValidationMessage(string id)
        {
            try
            {
                return _driver.FindElement(By.Id(id));
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

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

        public ValidationMessagesPaymentModel GetValidationMessages()
        {
            return new ValidationMessagesPaymentModel
            {
                ValidationMessageSourceIBAN = FindValidationMessage("ValidationMessageSourceIBAN")?.Text,
                ValidationMessageSourceBankName = FindValidationMessage("ValidationMessageSourceBankName")?.Text,
                ValidationMessageSourceBankCountry = FindValidationMessage("ValidationMessageSourceBankCountry")?.Text,
                ValidationMessagePostalCode = FindValidationMessage("ValidationMessagePostalCode")?.Text,
                ValidationMessageStreet = FindValidationMessage("ValidationMessageStreet")?.Text,
                ValidationMessageNumber = FindValidationMessage("ValidationMessageNumber")?.Text,
                ValidationMessageCity = FindValidationMessage("ValidationMessageCity")?.Text,
                ValidationMessageCountry = FindValidationMessage("ValidationMessageCountry")?.Text,
                ValidationMessageAmount = FindValidationMessage("ValidationMessageAmount")?.Text,
                ValidationMessageDestinationIBAN = FindValidationMessage("ValidationMessageDestinationIBAN")?.Text,
                ValidationMessageDestinationBankName = FindValidationMessage("ValidationMessageDestinationBankName")?.Text,
                ValidationMessageDestinationBankCountry = FindValidationMessage("ValidationMessageDestinationBankCountry")?.Text,
                ValidationMessageIntermediaryIBAN = FindValidationMessage("ValidationMessageIntermediaryIBAN")?.Text,
                ValidationMessageIntermediaryBankName = FindValidationMessage("ValidationMessageIntermediaryBankName")?.Text,
                ValidationMessageIntermediaryBankCountry = FindValidationMessage("ValidationMessageIntermediaryBankCountry")?.Text
            };
        }

        public void SetPayment(PaymentModel paymentModel)
        {
            FillTextField(SourceIBAN, paymentModel.SourceIBAN);
            FillTextField(SourceBankName, paymentModel.SourceBankName);
            SelectComboBox(SourceBankCountry, paymentModel.SourceBankCountry);

            FillTextField(PostalCode, paymentModel.PostalCode);
            FillTextField(Street, paymentModel.Street);
            FillTextField(Number, paymentModel.Number.ToString());
            FillTextField(City, paymentModel.City);
            FillTextField(Country, paymentModel.Country);
            FillTextField(Amount, paymentModel.Amount.ToString());

            FillTextField(DestinationIBAN, paymentModel.DestinationIBAN);
            FillTextField(DestinationBankName, paymentModel.DestinationBankName);
            SelectComboBox(DestinationBankCountry, paymentModel.DestinationBankCountry);

            TryFillTextField(IntermediaryIBAN, paymentModel.IntermediaryIBAN);
            TryFillTextField(IntermediaryBankName, paymentModel.IntermediaryBankName);
            TrySelectComboBox(IntermediaryBankCountry, paymentModel.IntermediaryBankCountry);

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", BtnSavePayment);

            Thread.Sleep(500);
        }

        public void GeneratePayment()
        {
            BtnGeneratePayment.Click();
        }

        public void SavePayment()
        {
            BtnSavePayment.Click();
        }

        private static void FillTextField(IWebElement element, string value)
        {
            element.Clear();
            element.SendKeys(value);
        }

        private static void SelectComboBox(IWebElement comboBoxElement, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                SelectElement select = new(comboBoxElement);
                select.SelectByText(value);
            }
        }

        private static void TryFillTextField(IWebElement element, string? value)
        {
            try
            {
                element.Clear();
                element.SendKeys(value);
            }
            catch (Exception)
            {
            }
        }

        private static void TrySelectComboBox(IWebElement comboBoxElement, string? value)
        {
            try
            {
                SelectElement select = new(comboBoxElement);
                select.SelectByText(value);
            }
            catch (Exception)
            {
            }
        }

        public List<string> GetCountries()
        {
            List<string> countries = [];

            WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(20));

            IWebElement sourceCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SourceBankCountry")));

            Thread.Sleep(500); // TODO: Modificar

            SelectElement select = new(sourceCountry);

            foreach (IWebElement option in select.Options)
            {
                countries.Add(option.Text);
            }

            return countries;
        }
    }
}
