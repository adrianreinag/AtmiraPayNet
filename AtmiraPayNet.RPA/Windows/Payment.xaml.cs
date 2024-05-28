using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Pages;
using AtmiraPayNet.RPA.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace AtmiraPayNet.RPA.Windows
{
    public partial class Payment : Window
    {
        public List<string> Countries = [];
        private PaymentModel paymentModel { get; set; }
        private readonly LoginPage loginPage;
        private readonly PaymentPage paymentPage;

        public Payment()
        {
            InitializeComponent();

            loginPage = new(new ChromeDriver());

            loginPage.Login(new LoginModel
            {
                UserName = Properties.Settings.Default.UserName,
                Password = Properties.Settings.Default.Password
            });

            paymentPage = new(new ChromeDriver());
            paymentPage.OpenPayment();

            FillForm(paymentPage.GetPayment());

            DataContext = this;
            UserNameTextBlock.Text = Properties.Settings.Default.UserName;
            //Get_Countries();
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
            paymentPage.CreateUpdatePayment(new PaymentModel
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
            paymentPage.CreateUpdatePayment(new PaymentModel
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
            SourceBankCountry.Items.Clear();
            PostalCode.Clear();
            Street.Clear();
            Number.Clear();
            City.Clear();
            Country.Clear();
            Amount.Clear();
            DestinationIBAN.Clear();
            DestinationBankName.Clear();
            DestinationBankCountry.Items.Clear();
            IntermediaryIBAN.Clear();
            IntermediaryBankName.Clear();
            IntermediaryBankCountry.Items.Clear();

            MessageBox.Show("Formulario limpiado.");
        }
    }
}
