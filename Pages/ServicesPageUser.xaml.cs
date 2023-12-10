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
    /// Логика взаимодействия для ServicesPageUser.xaml
    /// </summary>
    public partial class ServicesPageUser : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ServicesPageUser(MainClass mc)
        {
            InitializeComponent();

            var task = Task.Run(() => {
                return context.GetAllServices();
            });

            List<Service> services = task.Result as List<Service>;
            serviceListBox.ItemsSource = services;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ServicesLinkText}";
            DataContext = mc;                        
        }        
    }
}
