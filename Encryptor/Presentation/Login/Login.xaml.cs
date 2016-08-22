using System.Security.Cryptography;
using System.Windows;
using Encryptor.Business;
using Encryptor.Presentation.Home;
using Encryptor.Presentation.Login;

namespace Encryptor
{
    /// <summary>
    ///     Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly UserBUS _userBus;

        public Login()
        {
            InitializeComponent();
            _userBus = new UserBUS();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var user = _userBus.getUserByUserName(UserNameTextBox.Text);
            if (PasswordStorage.VerifyPassword(PasswordBox.Password, user.Password))
            {
                MessageBox.Show("U bent ingelogd!");
               var screen = new Home(user);
                screen.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("U gaf een fout passwoord!");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Register();
            screen.Show();
            this.Close();
        }
    }
}