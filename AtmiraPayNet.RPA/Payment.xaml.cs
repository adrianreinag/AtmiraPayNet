using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AtmiraPayNet.RPA
{
    public partial class Payment : Window
    {
        public Payment()
        {
            InitializeComponent();

            UserNameTextBlock.Text = Properties.Settings.Default.UserName;
        }

        private void CerrarTransaccion_Click(object sender, RoutedEventArgs e)
        {
            string sourceIBAN = SourceIBAN.Text;
            string sourceBankName = SourceBankName.Text;
            string sourceBankCountry = SourceBankCountry.Text;
            string postalCode = PostalCode.Text;
            string street = Street.Text;
            string number = Number.Text;
            string city = City.Text;
            string country = Country.Text;
            string amount = Amount.Text;
            string destinationIBAN = DestinationIBAN.Text;
            string destinationBankName = DestinationBankName.Text;
            string destinationBankCountry = DestinationBankCountry.Text;
            string intermediaryIBAN = IntermediaryIBAN.Text;
            string intermediaryBankName = IntermediaryBankName.Text;
            string intermediaryBankCountry = IntermediaryBankCountry.Text;

            ChromeDriver driver = new();

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7038/payment");

                WebDriverWait loadingWait = new(driver, TimeSpan.FromSeconds(20));

                if (!loadingWait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loading"))))
                {
                    return;
                }

                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

                IWebElement txtSourceIBAN = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SourceIBAN")));
                txtSourceIBAN.SendKeys(sourceIBAN);

                IWebElement txtSourceBankName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SourceBankName")));
                txtSourceBankName.SendKeys(sourceBankName);

                IWebElement txtSourceBankCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SourceBankCountry")));
                txtSourceBankCountry.SendKeys(sourceBankCountry);

                IWebElement txtDestinationIBAN = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("DestinationIBAN")));
                txtDestinationIBAN.SendKeys(destinationIBAN);

                IWebElement txtDestinationBankName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("DestinationBankName")));
                txtDestinationBankName.SendKeys(destinationBankName);

                IWebElement txtDestinationBankCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("DestinationBankCountry")));
                txtDestinationBankCountry.SendKeys(destinationBankCountry);

                IWebElement txtIntermediaryIBAN = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("IntermediaryIBAN")));
                txtIntermediaryIBAN.SendKeys(intermediaryIBAN);

                IWebElement txtIntermediaryBankName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("IntermediaryBankName")));
                IWebElement txtIntermediaryBankCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("IntermediaryBankCountry")));

                if (string.IsNullOrWhiteSpace(intermediaryIBAN))
                {
                    txtIntermediaryIBAN.SendKeys(intermediaryIBAN);
                    txtIntermediaryBankName.SendKeys(intermediaryBankName);
                    txtIntermediaryBankCountry.SendKeys(intermediaryBankCountry);
                }

                IWebElement txtStreet = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Street")));
                txtStreet.SendKeys(street);

                IWebElement txtNumber = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Number")));
                txtNumber.SendKeys(number);

                IWebElement txtCity = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("City")));
                txtCity.SendKeys(city);

                IWebElement txtCountry = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Country")));
                txtCountry.SendKeys(country);

                IWebElement txtPostalCode = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("PostalCode")));
                txtPostalCode.SendKeys(postalCode);


                IWebElement txtAmount = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Amount")));
                txtAmount.SendKeys(amount);

                IWebElement btnLogin = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Create")));
                btnLogin.Click();


                MessageBox.Show("Transacción cerrada.");

            }
            finally
            {
                driver.Quit();
            }
        }

        private void GuardarBorrador_Click(object sender, RoutedEventArgs e)
        {
            string sourceIBAN = SourceIBAN.Text;
            string sourceBankName = SourceBankName.Text;
            string sourceBankCountry = SourceBankCountry.Text;
            string postalCode = PostalCode.Text;
            string street = Street.Text;
            string number = Number.Text;
            string city = City.Text;
            string country = Country.Text;
            string amount = Amount.Text;
            string destinationIBAN = DestinationIBAN.Text;
            string destinationBankName = DestinationBankName.Text;
            string destinationBankCountry = DestinationBankCountry.Text;
            string intermediaryIBAN = IntermediaryIBAN.Text;
            string intermediaryBankName = IntermediaryBankName.Text;
            string intermediaryBankCountry = IntermediaryBankCountry.Text;

            // Aquí puedes agregar la lógica para procesar la transacción
            MessageBox.Show("Borrador guardado.");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            SourceIBAN.Clear();
            SourceBankName.Clear();
            SourceBankCountry.Clear(); //
            PostalCode.Clear();
            Street.Clear();
            Number.Clear();
            City.Clear();
            Country.Clear();
            Amount.Clear();
            DestinationIBAN.Clear();
            DestinationBankName.Clear();
            DestinationBankCountry.Clear(); //
            IntermediaryIBAN.Clear();
            IntermediaryBankName.Clear();
            IntermediaryBankCountry.Clear(); //

            MessageBox.Show("Formulario limpiado.");
        }
    }
}
