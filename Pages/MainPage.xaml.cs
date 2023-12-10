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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public MainPage(MainClass mc)
        {            
            InitializeComponent();
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText}";
            text_company.Text = mc.siteText.MainPage_CompanyDescriptionText;
            text_info.Text = mc.siteText.MainPage_RequestText;      

            button_send_request.Click += async delegate
            {
                string request_status = await context.CreateRequest(tb_name.Text, tb_email.Text, tb_request.Text);
                MessageBox.Show(request_status);
                if (request_status == "Ваша заявка принята")
                {
                    tb_name.Text = "";
                    tb_email.Text = "";
                    tb_request.Text = "";
                }
            };                        
        }
    }
}
