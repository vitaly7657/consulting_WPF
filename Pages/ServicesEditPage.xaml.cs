using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
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

namespace module_21_exercise_2_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesEditPage.xaml
    /// </summary>
    public partial class ServicesEditPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ServicesEditPage(MainClass mc, Service service)
        {
            InitializeComponent();
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ServicesLinkText} -> Редактирование";
            service_title.Text = service.ServiceTitle;
            service_description.Text = service.ServiceDescription;

            button_back.Click += delegate
            {
                this.NavigationService.Navigate(new ServicesPage(mc));
            };

            button_edit_service_api.Click += async delegate
            {
                string result = await context.EditService(service.Id, service_title.Text, service_description.Text);
                MessageBox.Show(result);
            };
        }
    }
}
