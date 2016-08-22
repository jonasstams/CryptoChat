using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Media;
using Encryptor.Business;
using Encryptor.Extensions;
using Encryptor.Models;

namespace Encryptor.Presentation.Login
{
    /// <summary>
    ///     Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private readonly UserBUS _userBus;

        public Register()
        {
            InitializeComponent();
            _userBus = new UserBUS();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordStrengthScore = PasswordAdvisor.CheckStrength(PasswordBox.Password);

            switch (passwordStrengthScore)
            {
                case PasswordScore.Blank:
                    PasswordHelpLabel.Content = "Blank";
                    RegisterButton.IsEnabled = false;
                    break;
                case PasswordScore.VeryWeak:
                    PasswordHelpLabel.Content = "Very Weak";
                    RegisterButton.IsEnabled = false;
                    break;
                case PasswordScore.Weak:
                    PasswordHelpLabel.Content = "Weak";
                    RegisterButton.IsEnabled = false;
                    break;
                case PasswordScore.Medium:
                    PasswordHelpLabel.Content = "Almost Strong";
                    RegisterButton.IsEnabled = false;
                    break;
                case PasswordScore.Strong:
                    PasswordHelpLabel.Content = "Strong";
                    RegisterButton.IsEnabled = true;
                    break;
                case PasswordScore.VeryStrong:
                    PasswordHelpLabel.Content = "Very Strong";
                    RegisterButton.IsEnabled = true;
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Encryptor.Login();
            screen.Show();
            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text == "" || PasswordBox.Password == "" || ConfirmPasswordBox.Password == "" || EmailTextBox.Text == "")
            {
                if (UserNameTextBox.Text == "")
                {
                    UserNameTextBox.Background = Brushes.Red;
                }
                if (PasswordBox.Password == "")
                {
                    PasswordBox.Background = Brushes.Red;
                }
                if (ConfirmPasswordBox.Password == "")
                {
                    ConfirmPasswordBox.Background = Brushes.Red;
                }
                if (EmailTextBox.Text == "")
                {
                    EmailTextBox.Background = Brushes.Red;
                }
                MessageBox.Show("U moet een Username en Wachtwoord invoeren!", "Let op!", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else
            {
                if (_userBus.UsernameExist(UserNameTextBox.Text) == false)
                {
                    if (Validation.IsValidEmail(EmailTextBox.Text))
                    {
                        var _user = new User();
                        _user.Username = UserNameTextBox.Text;
                        _user.Password = PasswordStorage.CreateHash(PasswordBox.Password);
                        _user.Email = EmailTextBox.Text;
                        _user.Role = "User";
                        _userBus.Create(_user);
                        MessageBox.Show("User created");
                    }
                    else
                    {
                        EmailTextBox.Background = Brushes.Red;
                        EmailTextBox.SelectAll();
                        EmailTextBox.Focus();
                    }
                   
                }
                else
                {
                    MessageBox.Show("Deze Username is al in gebruik!", "Let op!", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    UserNameTextBox.Background = Brushes.Red;
                    UserNameTextBox.SelectAll();
                    UserNameTextBox.Focus();
                }
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == ConfirmPasswordBox.Password)
            {
                PasswordSameLabel.Content = "Confirmed!";
                RegisterButton.IsEnabled = true;
            }
            else
            {
                PasswordSameLabel.Content = "Not the same!";
                RegisterButton.IsEnabled = false;
            }
        }

       
    }
}