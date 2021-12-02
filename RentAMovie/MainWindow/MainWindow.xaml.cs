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

namespace RentAMovie.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _userID { get; set; }
        private bool _isAdmin { get; set; }

        public MainWindow(int userID, bool isAdmin)
        {
            InitializeComponent();
            _userID = userID;
            _isAdmin = isAdmin;
        }
    }
}
