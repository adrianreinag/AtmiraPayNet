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

        private void GeneratePayment_Click(object sender, RoutedEventArgs e)
        {
            _paymentPage.CreateUpdatePayment(new PaymentModel
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
            }, Status.Generated);

            MessageBox.Show("Pago generado.");
        }


        private void SavePayment_Click(object sender, RoutedEventArgs e)
        {
            _paymentPage.CreateUpdatePayment(new PaymentModel
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
            }, Status.Draft);

            MessageBox.Show("Pago guardado como borrador.");
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
