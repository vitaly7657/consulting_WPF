using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для ServicesAddPage.xaml
    /// </summary>
    public partial class ServicesAddPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ServicesAddPage(MainClass mc)
        {
            InitializeComponent();
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ServicesLinkText} -> Добавление";

            button_back.Click += delegate
            {
                this.NavigationService.Navigate(new ServicesPage(mc));
            };

            button_add_service_api.Click += async delegate
            {
                string result = await context.AddService(service_title.Text, service_description.Text);
                MessageBox.Show(result);
            };
        }
    }
}
