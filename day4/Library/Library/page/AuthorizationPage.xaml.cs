using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Library.service;

namespace Library.page
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        public AuthorizationPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == "")
            {
                MessageBox.Show("Введите логин");
            }
            else
            {


                if (tbPassword.Password == "")
                {
                    MessageBox.Show("Введите пароль");
                }
                else
                {
                    try
                    {

                        var request = (HttpWebRequest)WebRequest.Create("https://localhost:7256/Login");

                        var postData = "login=" + Uri.EscapeDataString(this.Login.Trim());
                        postData += "&password=" + Uri.EscapeDataString(Secure.Sha256(this.tbPassword.Password.Trim()));
                        var data = Encoding.ASCII.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        var response = (HttpWebResponse)request.GetResponse();
                        var resp = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        this.NavigationService.Navigate(new ReadersPage(resp));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
            }
        }

        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Visibility==Visibility.Visible)
            {
                tbVisiblePassword.Text = tbPassword.Password;
                tbPassword.Visibility = Visibility.Collapsed;
                tbVisiblePassword.Visibility = Visibility.Visible; 
            }
            else if (tbPassword.Visibility == Visibility.Collapsed)
            {
                
                tbPassword.Visibility = Visibility.Visible;
                tbVisiblePassword.Visibility = Visibility.Collapsed;
            }
        }

        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbVisiblePassword.Text = tbPassword.Password;
        }

        private void tbVisiblePassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPassword.Password = tbVisiblePassword.Text;
        }
    }
}
