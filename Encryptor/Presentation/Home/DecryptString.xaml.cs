using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Encryptor.Business;
using Encryptor.Encryption;
using Encryptor.Encryption.RSA;
using Encryptor.Models;

namespace Encryptor.Presentation.Home
{
    /// <summary>
    /// Interaction logic for DecryptString.xaml
    /// </summary>
    public partial class DecryptString : Window
    {
        private User currUser;
        private MessageBUS _msgBus;
        private UserBUS _userBus;
        public DecryptString(User _user)
        {
            InitializeComponent();
            currUser = _user;
            _msgBus = new MessageBUS();
            _userBus = new UserBUS();
            ReceivedMessagesListView.ItemsSource = _msgBus.GetIncomeMessages(_user.Id);
            UsernameMessagesLabel.Content = currUser.Username + ", this are your received messages:";

        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceivedMessagesListView.SelectedItems.Count < 1)
            {
                MessageBox.Show("U have to select a message first!", "Warning!", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }else if (ReceivedMessagesListView.SelectedItems.Count > 1)
            {
                MessageBox.Show("U can only read one message!", "Warning!", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageTextBlock.Visibility = Visibility.Visible;
                this.Height = 600;
                Message msg = (Message) ReceivedMessagesListView.SelectedItem;
                var fromUserPuK = msg.From.PuK;
                var currUserPvk = currUser.PvK;
                var file1 = msg.File1;
                var file2 = msg.File2;
                var file3 = msg.File3;
                var IV = msg.IV;
                var key =  RSACrypto.DecryptBytes(file2, currUserPvk);
                MessageTextBlock.Text = "\n\n########AES KEY DECRYPT ########\n\n";
                 UnicodeEncoding _encoder = new UnicodeEncoding();
                 MessageTextBlock.Text += _encoder.GetString(key);
              
                var message = AESCrypto.DecryptStringFromBytes(file1, key, IV);
                MessageTextBlock.Text += "\n\n######## MESSAGE ########\n\n";
                MessageTextBlock.Text += message;


                var oldHash = RSACrypto.DecryptBytes(file3, currUserPvk);

                string strOldHash = Encoding.UTF8.GetString(oldHash);
               
                MessageTextBlock.Text += "\n\n######## Decrypted hash ########\n\n";
                MessageTextBlock.Text += strOldHash;

                
                   if (BCrypt.CheckPassword(message, strOldHash))
                {
                    MessageTextBlock.Text += "\n\n######## HASHES ARE EQUAL! ########\n\n";
                }
                else
                {
                    MessageTextBlock.Text += "\n\n######## HASHES ARE NOT EQUAL! ########\n\n";
                }

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Home(currUser);
            screen.Show();
            this.Close();
        }
    }
}
