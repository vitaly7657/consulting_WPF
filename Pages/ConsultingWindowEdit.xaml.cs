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
    /// Логика взаимодействия для ConsultingWindowEdit.xaml
    /// </summary>
    public partial class ConsultingWindowEdit : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ConsultingWindowEdit(MainClass mc)
        {
            InitializeComponent();
            tb_mainpage.Text = mc.siteText.MainPage_MainLinkText;
            tb_services.Text = mc.siteText.MainPage_ServicesLinkText;
            tb_projects.Text = mc.siteText.MainPage_ProjectsLinkText;
            tb_blog.Text = mc.siteText.MainPage_BlogLinkText;
            tb_contacts.Text = mc.siteText.MainPage_ContactsLinkText;
            button_request.Text = mc.siteText.MainPage_RequestButtonText;
            tb_tagline_1.Text = mc.tagLineList[0].TagLineText;
            tb_tagline_2.Text = mc.tagLineList[1].TagLineText;
            tb_tagline_3.Text = mc.tagLineList[2].TagLineText;
            tb_company.Text = mc.siteText.MainPage_CompanyDescriptionText;
            tb_info.Text = mc.siteText.MainPage_RequestText;

            button_save_texts.Click += async delegate
            {
                mc.siteText.MainPage_MainLinkText = tb_mainpage.Text;
                mc.siteText.MainPage_ServicesLinkText = tb_services.Text;
                mc.siteText.MainPage_ProjectsLinkText = tb_projects.Text;
                mc.siteText.MainPage_BlogLinkText = tb_blog.Text;
                mc.siteText.MainPage_ContactsLinkText = tb_contacts.Text;
                mc.siteText.MainPage_RequestButtonText = button_request.Text;
                mc.tagLineList[0].TagLineText = tb_tagline_1.Text;
                mc.tagLineList[1].TagLineText = tb_tagline_2.Text;
                mc.tagLineList[2].TagLineText = tb_tagline_3.Text;
                mc.siteText.MainPage_CompanyDescriptionText = tb_company.Text;
                mc.siteText.MainPage_RequestText = tb_info.Text;
                var message = await context.EditTexts(mc);
                MessageBox.Show(message);
            };

            button_back.Click += delegate
            {
                MainClass mc = context.GetSiteTexts();
                this.NavigationService.Navigate(new MainPage(mc));
            };                        
        }        
    }
}
