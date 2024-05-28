using AtmiraPayNet.RPA.Models;
using AtmiraPayNet.RPA.Pages;
using OpenQA.Selenium.Chrome;
using System.Windows;

namespace AtmiraPayNet.RPA.Windows
{
    public partial class Access : Window
    {
        private readonly ChromeDriver _driver;
        private readonly LoginPage _loginPage;

        public Access(ChromeDriver driver)
        {
            InitializeComponent();

            _driver = driver;
            _loginPage = new(_driver);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Text = string.Empty;

            LoginModel loginModel = new()
            {
                UserName = UsernameTextBox.Text,
                Password = PasswordBox.Password
            };

            ErrorMessageTextBlock.Text = loginModel.getErrorMessage();
            if (ErrorMessageTextBlock.Text != string.Empty)
            {
                return;
            }

            if (_loginPage.Login(loginModel))
            {
                Properties.Settings.Default.UserName = loginModel.UserName;
                Properties.Settings.Default.Save();

                Payments payments = new(_driver);
                payments.Show();
                Hide();
            }
            else
            {
                ErrorMessageTextBlock.Text = "No se ha podido iniciar sesión. Por favor, inténtelo de nuevo.";
            }
        }

        private void SignUpTextBlock_MouseDown(object sender, RoutedEventArgs e)
        {
            _loginPage.OpenRegister();
        }
    }
}
