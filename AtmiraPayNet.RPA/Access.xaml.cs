using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Windows;

namespace AtmiraPayNet.RPA
{
    public partial class Access : Window
    {
        public Access()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Text = string.Empty;

            string userName = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(userName))
            {
                ErrorMessageTextBlock.Text = "El campo de usuario es obligatorio.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorMessageTextBlock.Text = "El campo de contraseña es obligatorio.";
                return;
            }

            if (SeleniumLogin(userName, password))
            {
                Properties.Settings.Default.UserName = userName;
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.Save();

                Payment payment = new();
                payment.Show();
                Close();
            }
            else
            {
                ErrorMessageTextBlock.Text = "No se ha podido iniciar sesión. Por favor, inténtelo de nuevo.";
            }
        }

        private void SignUpTextBlock_MouseDown(object sender, RoutedEventArgs e)
        {
            SeleniumRegister();
        }

        private static bool SeleniumLogin(string userName, string password)
        {
            ChromeDriver driver = new();

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7038/login");

                WebDriverWait loadingWait = new(driver, TimeSpan.FromSeconds(20));
                if (!loadingWait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loading"))))
                {
                    return false;
                }

                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

                IWebElement txtUserName = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("UserName")));
                txtUserName.SendKeys(userName);

                IWebElement txtPassword = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Password")));
                txtPassword.SendKeys(password);

                IWebElement btnLogin = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("LoginButton")));
                btnLogin.Click();


                while (driver.Url.Contains("login"))
                {
                    try
                    {
                        if (driver.FindElement(By.ClassName("swal2-container")).Displayed)
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
            finally
            {
                driver.Quit();
            }
        }

        private static void SeleniumRegister()
        {
            ChromeDriver driver = new();
            driver.Navigate().GoToUrl("https://localhost:7038/register");
        }
    }
}
