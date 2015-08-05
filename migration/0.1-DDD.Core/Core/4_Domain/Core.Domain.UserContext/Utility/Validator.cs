namespace Core.Domain.UserContext
{
    using System.Text.RegularExpressions;

    internal static class Validator
    {
        internal static bool ValidateUserName(string userName, ref string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                errorMessage = "The username can not be null or empty string.";
                return false;
            }

            if (userName.SingleByteLengthInRange(4, 20) == false)
            {
                errorMessage = "The username length must be greater than 4 and less than 20.";
                return false;
            }

            Regex regex = new Regex(@"^[\u4e00-\u9fa5a-zA-Z0-9]+$");
            if (regex.IsMatch(userName) == false)
            {
                errorMessage = "The username can only contain letters, numbers and chinese characters.";
                return false;
            }

            return true;
        }

        internal static bool ValidateUserMail(string userMail, ref string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(userMail))
            {
                errorMessage = "The e-mail can not be null or empty string.";
                return false;
            }

            if (userMail.SingleByteLengthInRange(4, 50) == false)
            {
                errorMessage = "The username length must be greater than 4 and less than 50.";
                return false;
            }

            return true;
        }

        internal static bool ValidatePassword(string password, ref string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "The password can not be null or empty string.";
                return false;
            }

            if (password.Length < 6 || password.Length > 32)
            {
                errorMessage = "The password length must be greater than 5 and less than 20.";
                return false;
            }

            Regex regex = new Regex(@"^[a-zA-Z0-9~!@#$%^&*()_+]+$");
            if (regex.IsMatch(password) == false)
            {
                errorMessage = "The password can only contain letters, numbers and special characters.";
                return false;
            }

            return true;
        }
    }
}