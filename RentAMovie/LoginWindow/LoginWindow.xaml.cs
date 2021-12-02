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

namespace RentAMovie.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginPage loginPage = new LoginPage();
        RegisterPage registerPage = new RegisterPage();

        public LoginWindow()
        {
            InitializeComponent();
            LoginWindowFrame.Content = loginPage;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoginPageButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindowFrame.Content = loginPage;
        }

        private void RegisterPageButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindowFrame.Content = registerPage;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Mouse.GetPosition(this).Y < 30)
                this.DragMove();
        }
    }
}
