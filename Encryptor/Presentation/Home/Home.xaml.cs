using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Encryptor.Models;

namespace Encryptor.Presentation.Home
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private User currUser;
        public Home(User _user)
        {
            InitializeComponent();
            currUser = _user;
            NameLabel.Content = "Hey, " + _user.Username;
        }

        private void EncryptStringButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new EncryptString(currUser);
            screen.Show();
            this.Close();
        }

        private void DescryptStringButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new DecryptString(currUser);
            screen.Show();
            this.Close();
        }
    }
}
