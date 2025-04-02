using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            // Путь к корню проекта (без имени проекта)
            string projectDirectory = GetProjectRootDirectory();
            string clientsFolderPath = System.IO.Path.Combine(projectDirectory, "Клиенты");

            // Создаем папку, если её нет
            if (!Directory.Exists(clientsFolderPath))
            {
                Directory.CreateDirectory(clientsFolderPath);
            }

            OpenFileDialog myOpenFileDialog = new OpenFileDialog
            {
                InitialDirectory = clientsFolderPath
            };

            if (myOpenFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = myOpenFileDialog.FileName;

                // Сохраняем относительный путь ОТНОСИТЕЛЬНО КОРНЯ ПРОЕКТА
                _currentClient.PhotoPath = System.IO.Path.Combine("Клиенты", System.IO.Path.GetFileName(selectedFilePath));

                // Загружаем изображение по полному пути
                LogoImage.Source = new BitmapImage(new Uri(selectedFilePath));
            }
        }

        // Метод для получения корня проекта
        private string GetProjectRootDirectory()
        {
            // Путь к исполняемому файлу (bin/Debug)
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // Поднимаемся на 3 уровня: bin/Debug → bin → Корень проекта
            return System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(exePath)));
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        bool isValidFIOString(string str)
        {
            bool isValid = true;

            foreach (char c in str)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
        bool isValidPhoneString(string str)
        {
            bool isValid = true;

            foreach (char c in str)
            {
                if (!char.IsDigit(c) && c!=' ' && c != '+' && c != '-' && c != '(' && c != ')')
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
                errors.AppendLine("Введите фамилию");
            else if (_currentClient.LastName.Length >= 50)
                errors.AppendLine("Фамилия должно быть меньше 50 символов!");
            else
            {
                if (!isValidFIOString(_currentClient.LastName))
                    errors.AppendLine("Фамилия может содержать только буквы, пробел и дефис!");
            }

            if (string.IsNullOrWhiteSpace(_currentClient.FirstName))
                errors.AppendLine("Введите имя");
            else if (_currentClient.FirstName.Length >= 50)
                errors.AppendLine("Имя должно быть меньше 50 символов!");
            else
            {
                if (!isValidFIOString(_currentClient.FirstName))
                    errors.AppendLine("Имя может содержать только буквы, пробел и дефис!");
            }

            if (!string.IsNullOrWhiteSpace(_currentClient.Patronymic))
            {
                if (_currentClient.Patronymic.Length >= 50)
                    errors.AppendLine("Отчество должно быть меньше 50 символов!");
                if (!isValidFIOString(_currentClient.Patronymic))
                    errors.AppendLine("Отчество может содержать только буквы, пробел и дефис!");
            }

            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Введите телефон");
            else
            {
                if (!isValidPhoneString(_currentClient.Phone))
                    errors.AppendLine("Телефон может содержать только цифры, символы: плюс, минус, открывающая и закрывающая круглые скобки и пробел!");
                string ph = _currentClient.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (ph.Length > 11 || ph.Length < 11)
                    errors.AppendLine("Введите правильный телефон (несовпадение длины)!");
            }

            if (GenderFemaleRadioBtn.IsChecked == false && GenderMaleRadioBtn.IsChecked == false)
                errors.AppendLine("Укажите пол");

            if (!string.IsNullOrWhiteSpace(_currentClient.Email))
            {
                if (!IsValidEmail(_currentClient.Email))
                    errors.AppendLine("Некорректный email");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (GenderFemaleRadioBtn.IsChecked == true)
                _currentClient.GenderCode = "ж";
            else
                _currentClient.GenderCode = "м";

            _currentClient.RegistrationDate = DateTime.Now;

            if (_currentClient.ID == 0)
                AbdeevLanguageEntities.GetContext().Client.Add(_currentClient);

            try
            {

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
