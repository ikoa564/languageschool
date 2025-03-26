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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Client> CurrentPageList = new List<Client>();
        List<Client> TableList;


        private void ChangePage(int direction, int? selectedPage)
        {
            int selectedOutput = OutputComboBox.SelectedIndex;
            int currentOutputPages = 0;
            CurrentPageList.Clear();

            CountRecords = TableList.Count;

            switch (selectedOutput)
            {
                case 0:
                    currentOutputPages = 10; break;
                case 1:
                    currentOutputPages = 50; break;
                case 2:
                    currentOutputPages = 200; break;
                case 3:
                    currentOutputPages = CountRecords; break;
                default:
                    MessageBox.Show("ошибка!");
                    break;
            }
            if (CountRecords % currentOutputPages > 0)
                CountPage = CountRecords / currentOutputPages + 1;
            else
                CountPage = CountRecords / currentOutputPages;

            Boolean ifUpdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * currentOutputPages + currentOutputPages < CountRecords ? CurrentPage * currentOutputPages + currentOutputPages : CountRecords;
                    for (int i = CurrentPage * currentOutputPages; i < min; i++)
                        CurrentPageList.Add(TableList[i]);
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * currentOutputPages + currentOutputPages < CountRecords ? CurrentPage * currentOutputPages + currentOutputPages : CountRecords;
                            for (int i = CurrentPage * currentOutputPages; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else
                            ifUpdate = false;
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * currentOutputPages + currentOutputPages < CountRecords ? CurrentPage * currentOutputPages + currentOutputPages : CountRecords;
                            for (int i = CurrentPage * currentOutputPages; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else
                            ifUpdate = false;
                        break;
                }
            }
            if (ifUpdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                    PageListBox.Items.Add(i);
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * currentOutputPages + currentOutputPages < CountRecords ? CurrentPage * currentOutputPages + currentOutputPages : CountRecords;
                //FirstPageCountTB.Text = min.ToString();

                ClientListView.ItemsSource = CurrentPageList;
                ClientListView.Items.Refresh();
            }
        }



        public ClientPage()
        {
            InitializeComponent();
            var currentClient = AbdeevLanguageEntities.GetContext().Client.ToList();

            ClientListView.ItemsSource = currentClient;
            OutputComboBox.SelectedIndex = 3;
            SortComboBox.SelectedIndex = 0;
            FilterComboBox.SelectedIndex = 0;
            UpdateClients();
        }

        public void UpdateClients()
        {
            var currentClient = AbdeevLanguageEntities.GetContext().Client.ToList();

            switch (FilterComboBox.SelectedIndex)
            {
                //case 0:
                //    currentClient = currentClient.Where(p => p.Gen);
                //    break;
                case 1:
                    currentClient = currentClient.Where(p => p.GenderCode == "ж").ToList();
                    break;
                case 2:
                    currentClient = currentClient.Where(p => p.GenderCode == "м").ToList();
                    break;
            }

            currentClient = currentClient.Where(p => p.FirstName.ToLower().Contains(SearchTB.Text.ToLower()) ||
            p.LastName.ToLower().Contains(SearchTB.Text.ToLower()) ||
            p.Patronymic.ToLower().Contains(SearchTB.Text.ToLower()) ||
            p.Email.ToLower().Contains(SearchTB.Text.ToLower()) 
            || p.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(SearchTB.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))).ToList();


            if (SortComboBox.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderBy(p => p.LastName).ToList();
            }
            if (SortComboBox.SelectedIndex == 2)
                currentClient = currentClient.OrderBy(p => p.LastVisit).ToList();
            if (SortComboBox.SelectedIndex == 3)
                currentClient = currentClient.OrderByDescending(p => p.CountVisit).ToList();

            ClientListView.ItemsSource = currentClient;


            FirstPageCountTB.Text = currentClient.Count.ToString();
            SecondPageCountTB.Text = " из " + AbdeevLanguageEntities.GetContext().Client.ToList().Count().ToString();
            TableList = currentClient;
            ChangePage(0, 0);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);

        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;
            var currentClientService = AbdeevLanguageEntities.GetContext().ClientService.ToList();
            currentClientService = currentClientService.Where(p => p.ClientID == currentClient.ID).ToList();
            if (currentClientService.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, т.к. существуют информация о посещениях!");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AbdeevLanguageEntities.GetContext().Client.Remove(currentClient);
                        AbdeevLanguageEntities.GetContext().SaveChanges();
                        ClientListView.ItemsSource = AbdeevLanguageEntities.GetContext().Client.ToList();
                        UpdateClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void OutputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }
    }
}
