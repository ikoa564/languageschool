using Microsoft.Win32;
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

namespace abdeevLanguage
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Client _currentClient = new Client();
        public AddEditWindow(Client selectedClient)
        {
            InitializeComponent();

            if (selectedClient != null)
            {
                _currentClient = selectedClient;
            }

            //FirstNameTextBox.Text = _currentAgent.FirstName;
            //LastNameTextBox.Text = _currentAgent.LastName;
            //PhoneTextBox.Text = _currentAgent.Phone;
            //EmailTextBox.Text = _currentAgent.Email;
            //LogoImage.Source = new BitmapImage(new Uri(_currentAgent.PhotoPath));

            DataContext = _currentClient;
        }

        private void EditPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                string path = myOpenFileDialog.FileName;

                int index = path.LastIndexOf("Клиенты");

                path = "\\" + path.Substring(index);
                _currentAgent.Logo = path;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));

            }
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
