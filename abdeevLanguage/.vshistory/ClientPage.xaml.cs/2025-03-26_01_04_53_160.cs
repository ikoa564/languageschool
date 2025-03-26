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
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            var currentClient = AbdeevLanguageEntities.GetContext().Client.ToList();

            ClientListView.ItemsSource = currentClient;
            UpdateClients();
            SecondPageCountTB.Text = " из " + AbdeevLanguageEntities.GetContext().Client.ToList().Count().ToString();
            OutputComboBox.SelectedIndex = 3;
        }

        public void UpdateClients()
        {
            var currentClient = AbdeevLanguageEntities.GetContext().Client.ToList();

            FirstPageCountTB.Text = currentClient.Count.ToString();
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void (object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Service;
            var currentClientService = Abdeev_autoserviceEntities.GetContext().ClientService.ToList();
            currentClientService = currentClientService.Where(p => p.ServiceID == currentService.ID).ToList();
            if (currentClientService.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, т.к. существуют записи на эту услугу");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Abdeev_autoserviceEntities.GetContext().Service.Remove(currentService);
                        Abdeev_autoserviceEntities.GetContext().SaveChanges();
                        ServiceListView.ItemsSource = Abdeev_autoserviceEntities.GetContext().Service.ToList();
                        UpdateServices();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

    }
}
