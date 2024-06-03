using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Pages;
using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium.Chrome;
using System.Windows;

namespace AtmiraPayNet.RPA.Windows
{
    public partial class Payment : Window
    {
        public List<string> Countries = [];
        private readonly ChromeDriver _driver;
        private readonly PaymentPage _paymentPage;

        public Payment(ChromeDriver driver)
        {
            InitializeComponent();

            _driver = driver;
            _paymentPage = new(_driver);

            DataContext = this;

            Countries = _paymentPage.GetCountries();

            SourceBankCountry.ItemsSource = Countries;
            DestinationBankCountry.ItemsSource = Countries;
            IntermediaryBankCountry.ItemsSource = Countries;

            FillForm(_paymentPage.GetPayment());
        }

        private void FillForm(PaymentModel paymentModel)
        {
            SourceIBAN.Text = paymentModel.SourceIBAN;
            SourceBankName.Text = paymentModel.SourceBankName;
            SourceBankCountry.Text = paymentModel.SourceBankCountry;
            PostalCode.Text = paymentModel.PostalCode;
            Street.Text = paymentModel.Street;
            Number.Text = paymentModel.Number;
            City.Text = paymentModel.City;
            Country.Text = paymentModel.Country;
            Amount.Text = paymentModel.Amount;
            DestinationIBAN.Text = paymentModel.DestinationIBAN;
            DestinationBankName.Text = paymentModel.DestinationBankName;
            DestinationBankCountry.Text = paymentModel.DestinationBankCountry;
            IntermediaryIBAN.Text = paymentModel.IntermediaryIBAN;
            IntermediaryBankName.Text = paymentModel.IntermediaryBankName;
            IntermediaryBankCountry.Text = paymentModel.IntermediaryBankCountry;
        }

        private bool ValidateForm(ValidationMessagesPaymentModel validationMessagesPaymentModel)
        {
            ValidationMessageSourceIBAN.Text = validationMessagesPaymentModel.ValidationMessageSourceIBAN;
            ValidationMessageSourceBankName.Text = validationMessagesPaymentModel.ValidationMessageSourceBankName;
            ValidationMessageSourceBankCountry.Text = validationMessagesPaymentModel.ValidationMessageSourceBankCountry;
            ValidationMessagePostalCode.Text = validationMessagesPaymentModel.ValidationMessagePostalCode;
            ValidationMessageStreet.Text = validationMessagesPaymentModel.ValidationMessageStreet;
            ValidationMessageNumber.Text = validationMessagesPaymentModel.ValidationMessageNumber;
            ValidationMessageCity.Text = validationMessagesPaymentModel.ValidationMessageCity;
            ValidationMessageCountry.Text = validationMessagesPaymentModel.ValidationMessageCountry;
            ValidationMessageAmount.Text = validationMessagesPaymentModel.ValidationMessageAmount;
            ValidationMessageDestinationIBAN.Text = validationMessagesPaymentModel.ValidationMessageDestinationIBAN;
            ValidationMessageDestinationBankName.Text = validationMessagesPaymentModel.ValidationMessageDestinationBankName;
            ValidationMessageDestinationBankCountry.Text = validationMessagesPaymentModel.ValidationMessageDestinationBankCountry;
            ValidationMessageIntermediaryIBAN.Text = validationMessagesPaymentModel.ValidationMessageIntermediaryIBAN;
            ValidationMessageIntermediaryBankName.Text = validationMessagesPaymentModel.ValidationMessageIntermediaryBankName;
            ValidationMessageIntermediaryBankCountry.Text = validationMessagesPaymentModel.ValidationMessageIntermediaryBankCountry;

            // Set visibility based on the validation messages
            ValidationMessageSourceIBAN.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageSourceIBAN) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageSourceBankName.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageSourceBankName) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageSourceBankCountry.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageSourceBankCountry) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessagePostalCode.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessagePostalCode) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageStreet.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageStreet) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageNumber.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageNumber) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageCity.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageCity) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageCountry.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageCountry) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageAmount.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageAmount) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageDestinationIBAN.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageDestinationIBAN) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageDestinationBankName.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageDestinationBankName) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageDestinationBankCountry.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageDestinationBankCountry) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageIntermediaryIBAN.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageIntermediaryIBAN) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageIntermediaryBankName.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageIntermediaryBankName) ? Visibility.Collapsed : Visibility.Visible;
            ValidationMessageIntermediaryBankCountry.Visibility = string.IsNullOrEmpty(validationMessagesPaymentModel.ValidationMessageIntermediaryBankCountry) ? Visibility.Collapsed : Visibility.Visible;

            return validationMessagesPaymentModel.IsOkey();
        }


        private void GeneratePayment_Click(object sender, RoutedEventArgs e)
        {
            _paymentPage.SetPayment(new PaymentModel
            {
                SourceIBAN = SourceIBAN.Text,
                SourceBankName = SourceBankName.Text,
                SourceBankCountry = SourceBankCountry.Text,
                PostalCode = PostalCode.Text,
                Street = Street.Text,
                Number = Number.Text,
                City = City.Text,
                Country = Country.Text,
                Amount = Amount.Text,
                DestinationIBAN = DestinationIBAN.Text,
                DestinationBankName = DestinationBankName.Text,
                DestinationBankCountry = DestinationBankCountry.Text,
                IntermediaryIBAN = IntermediaryIBAN.Text,
                IntermediaryBankName = IntermediaryBankName.Text,
                IntermediaryBankCountry = IntermediaryBankCountry.Text
            });

            _paymentPage.GeneratePayment();

            if (ValidateForm(_paymentPage.GetValidationMessages()))
            {
                MessageBox.Show("Pago generado.");
            }
            else
            {
                MessageBox.Show("Por favor, rellene correctamente los campos.");
            }
        }


        private void SavePayment_Click(object sender, RoutedEventArgs e)
        {
            _paymentPage.SetPayment(new PaymentModel
            {
                SourceIBAN = SourceIBAN.Text,
                SourceBankName = SourceBankName.Text,
                SourceBankCountry = SourceBankCountry.Text,
                PostalCode = PostalCode.Text,
                Street = Street.Text,
                Number = Number.Text,
                City = City.Text,
                Country = Country.Text,
                Amount = Amount.Text,
                DestinationIBAN = DestinationIBAN.Text,
                DestinationBankName = DestinationBankName.Text,
                DestinationBankCountry = DestinationBankCountry.Text,
                IntermediaryIBAN = IntermediaryIBAN.Text,
                IntermediaryBankName = IntermediaryBankName.Text,
                IntermediaryBankCountry = IntermediaryBankCountry.Text
            });

            _paymentPage.SavePayment();

            if (ValidateForm(_paymentPage.GetValidationMessages()))
            {
                MessageBox.Show("Pago guardado.");
            }
            else
            {
                MessageBox.Show("Por favor, rellene correctamente los campos.");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            SourceIBAN.Clear();
            SourceBankName.Clear();
            PostalCode.Clear();
            Street.Clear();
            Number.Clear();
            City.Clear();
            Country.Clear();
            Amount.Clear();
            DestinationIBAN.Clear();
            DestinationBankName.Clear();
            IntermediaryIBAN.Clear();
            IntermediaryBankName.Clear();

            SourceBankCountry.SelectedItem = null;
            DestinationBankCountry.SelectedItem = null;
            IntermediaryBankCountry.SelectedItem = null;

            MessageBox.Show("Formulario limpiado.");
        }
    }
}
