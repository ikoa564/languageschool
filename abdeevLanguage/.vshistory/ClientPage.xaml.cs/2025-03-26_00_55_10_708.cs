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

        }

        public void UpdateClients()
        {
            var currentClient = AbdeevLanguageEntities.GetContext().Client.ToList();

            FirstPageCountTB.Text = currentClient.Count.ToString();
        }
    }
}
