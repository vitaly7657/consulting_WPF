using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
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

namespace module_21_exercise_2_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ServicesPage(MainClass mc)
        {             
            InitializeComponent();

            var task = Task.Run(() => {
                return context.GetAllServices();
            });

            List<Service> services = task.Result as List<Service>;
            serviceListBox.ItemsSource = services;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ServicesLinkText}";
            DataContext = mc;

            button_add_service.Click += delegate
            {
                this.NavigationService.Navigate(new ServicesAddPage(mc));
            };
        }

        private void button_edit_service_Click(object sender, RoutedEventArgs e)
        {
            Service selectedService = ((ListBoxItem)serviceListBox.ContainerFromElement((Button)sender)).Content as Service;
            MainClass mc = context.GetSiteTexts();
            this.NavigationService.Navigate(new ServicesEditPage(mc, selectedService));
        }

        private async void button_delete_service_Click(object sender, RoutedEventArgs e)
        {
            Service selectedService = ((ListBoxItem)serviceListBox.ContainerFromElement((Button)sender)).Content as Service;
            await context.DeleteService(selectedService.Id.ToString());
            serviceListBox.ItemsSource = await context.GetAllServices() as List<Service>;
        }
    }
}
