using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Encryptor.Business;
using Encryptor.Encryption;
using Encryptor.Encryption.RSA;
using Encryptor.Models;

namespace Encryptor.Presentation.Home
{
    /// <summary>
    ///     Interaction logic for EncryptString.xaml
    /// </summary>
    public partial class EncryptString : Window
    {
        private readonly UserBUS _userBus;
        private readonly MessageBUS _msgBus;
        private readonly User currUser;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public EncryptString(User _user)
        {
            InitializeComponent();
            _userBus = new UserBUS();
            _msgBus = new MessageBUS();
            currUser = _user;
            UserComboBox.ItemsSource = _userBus.GetUsernameList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Home(currUser);
            screen.Show();
            Close();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageTextBlock.Text != "" || UserComboBox.SelectedIndex == -1)
            {
                try
                {
                    using (var myAes = Aes.Create())
                    {
                        myAes.KeySize = 128;
                        var originalMessage = MessageTextBlock.Text;
                        var toUser = _userBus.getUserByUserName(UserComboBox.SelectedItem.ToString());
                        Message msg = new Message();
                        msg.To =  toUser;
                        msg.From = currUser;

                      
                        //FILE 1 - Encrypt message symetric
                        var file1 = AESCrypto.EncryptStringToBytes(originalMessage, myAes.Key, myAes.IV);
                        msg.File1 = file1;
                        msg.IV = myAes.IV;
                        MessageTextBlock.Text += "\n\n########File 1 - Symmetric encryption########\n\n";
                        for (var i = 0; i < file1.Length; i++)
                        {
                            MessageTextBlock.Text += file1[i].ToString();
                        }

                        //FILE 2 - Encrypt symetric key with PuK from toUser

                        var file2 = RSACrypto.EncryptBytes(myAes.Key, toUser.PuK);
                        msg.File2 = file2;
                        MessageTextBlock.Text += "\n\n########File 2 - RSA encryption: AES key with public key ToUser ########\n\n";
                        for (var i = 0; i < file2.Length; i++)
                        {
                            MessageTextBlock.Text += file2[i].ToString();
                        }

                        //FILE 3 - HASH Original message and encrypt with private from currUser
                        var hash = BCrypt.HashPassword(originalMessage, BCrypt.GenerateSalt());
                        byte[] bytes = new byte[hash.Length * sizeof(char)];
                        System.Buffer.BlockCopy(hash.ToCharArray(), 0, bytes, 0, bytes.Length);
                        var file3 = RSACrypto.EncryptBytes(Encoding.UTF8.GetBytes(hash), toUser.PuK);
                        msg.File3 = file3;
                        MessageTextBlock.Text += "\n\n########File 3 - RSA encryption: Hash message and encrypt with private key FromUser ########\n\n";
                        for (var i = 0; i < file3.Length; i++)
                        {
                            MessageTextBlock.Text += file3[i].ToString();
                        }
                        if (_msgBus.Create(msg))
                        {
                            MessageBox.Show("Message created");
                        }
                        else
                        {
                            MessageBox.Show("Message not saved");
                        }
                         UnicodeEncoding _encoder = new UnicodeEncoding();
                        MessageTextBlock.Text += "\n\n######## AES KEY ########\n\n";
                        MessageTextBlock.Text += _encoder.GetString(myAes.Key);

                        MessageTextBlock.Text += "\n\n######## BCRYPT HASH ########\n\n";
                        MessageTextBlock.Text += Encoding.UTF8.GetString(bytes);

                        //    MessageTextBlock.Text += hash;

                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Er ging iets mis!", "Fout!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("U have to take someone to send it to, and you have to fill in a message!",
                    "Warning!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }
}