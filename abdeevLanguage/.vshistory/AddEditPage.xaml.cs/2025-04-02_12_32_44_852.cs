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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace abdeevLanguage
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Client _currentClient = new Client();

        public AddEditPage(Client selectedClient)
        {
            InitializeComponent();
            if (selectedClient != null)
            {
                _currentClient = selectedClient;

                if (_currentClient.GenderCode == "м")
                    GenderMaleRadioBtn.IsChecked = true;
                else
                    GenderFemaleRadioBtn.IsChecked = true;
            }
            else
            {
                IDTextBlock.Visibility = Visibility.Hidden;
                IDTextBox.Visibility = Visibility.Hidden;
            }

            DataContext = _currentClient;
        }

        private void EditPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                string path = myOpenFileDialog.FileName;

                int index = path.LastIndexOf("Клиенты");

                // Извлекаем подстроку, начиная от слова "agents"
                path = path.Substring(index);
                _currentClient.PhotoPath = path;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentClient.FirstName))
                errors.AppendLine("First Name is required.");
            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
                errors.AppendLine("Last Name");
            if (string.IsNullOrWhiteSpace(_currentClient.Email))
                errors.AppendLine("Email is required.");
            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Phone is required.");
            if (!BirthdayDP.SelectedDate.HasValue)
                errors.AppendLine("BirthdayDP is required.");
            if (GenderFemaleRadioBtn.IsChecked == false && GenderMaleRadioBtn.IsChecked == false)
                errors.AppendLine("GenderMaleRadioBtn is required");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentClient.ID == 0)
                AbdeevLanguageEntities.GetContext().Client.Add(_currentClient);

            try
            {
                if (GenderFemaleRadioBtn.IsChecked == true)
                    _currentClient.GenderCode = "ж";
                else
                    _currentClient.GenderCode = "м";
                AbdeevLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
