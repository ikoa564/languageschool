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

                if(selectedClient.GenderCode=="м")
                    GenderMaleRadioBtn.IsEnabled=true;
                if (selectedClient.GenderCode == "ж")
                    GenderFemaleRadioBtn.IsEnabled = true;

            }

            DataContext = _currentClient;
        }

        private void EditPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                string path = myOpenFileDialog.FileName;
                _currentClient.PhotoPath = path;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));

            }
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if(string.IsNullOrWhiteSpace(_currentClient.FirstName))
                errors.AppendLine("First Name is required.");
            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
                errors.AppendLine("Last Name");
            if (string.IsNullOrWhiteSpace(_currentClient.Email))
                errors.AppendLine("Email is required.");
            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Phone is required.");
            if(!BirthdayDP.SelectedDate.HasValue)
                errors.AppendLine("BirthdayDP is required.");
            if (GenderFemaleRadioBtn.IsChecked == false && GenderMaleRadioBtn.IsChecked == false)
                errors.AppendLine("GenderMaleRadioBtn is required");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(_currentClient.ID==0)
                AbdeevLanguageEntities.GetContext().Client.Add(_currentClient);

            try
            {
                AbdeevLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                DialogResult = true;
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
