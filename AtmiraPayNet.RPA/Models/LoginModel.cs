namespace AtmiraPayNet.RPA.Models
{
    internal class LoginModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }

        public string GetErrorMessage()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                return "El campo de usuario es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                return "El campo de contraseña es obligatorio.";
            }

            return string.Empty;
        }
    }
}
