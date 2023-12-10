using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
using module_21_exercise_2_WPF.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
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

namespace module_21_exercise_2_WPF
{
    /// <summary>
    /// Логика взаимодействия для ConsultingWindow.xaml
    /// </summary>
    public partial class ConsultingWindow : Window
    {
        public static ConsultingDataApi context = new ConsultingDataApi();
        public static AccountDataApi contextAccount = new AccountDataApi();
        public static string currentUser = "";
        
        public MainClass GetTexts(ConsultingDataApi context)
        {
            MainClass mc = context.GetSiteTexts();
            button_mainpage.Content = mc.siteText.MainPage_MainLinkText;
            button_services.Content = mc.siteText.MainPage_ServicesLinkText;
            button_projects.Content = mc.siteText.MainPage_ProjectsLinkText;
            button_blog.Content = mc.siteText.MainPage_BlogLinkText;
            button_contacts.Content = mc.siteText.MainPage_ContactsLinkText;
            button_workpage.Content = "Рабочий стол";
            button_request.Content = mc.siteText.MainPage_RequestButtonText;
            tb_tagline.Text = mc.tagLine.TagLineText;
            return mc;
        }

        public ConsultingWindow()
        {            
            InitializeComponent();
            sp_login_or_reg.Visibility= Visibility.Visible;
            sp_login.Visibility= Visibility.Collapsed;
            sp_reg.Visibility= Visibility.Collapsed;
            sp_login_ok.Visibility= Visibility.Collapsed;
            button_edit_texts.Visibility= Visibility.Collapsed;
            button_workpage.Visibility= Visibility.Collapsed;
            Thread.Sleep(3000);
            MainClass mc = GetTexts(context);            
            MainFrame.Content = new MainPage(mc);
            
            MainFrame.ContentRendered += delegate
            {                
                GetTexts(context);
            };

            button_request.Click += delegate
            {
                MainFrame.Content = new MainPage(mc);
                GetTexts(context);
            };

            button_mainpage.Click +=  delegate
            {
                MainFrame.Content = new MainPage(mc);
                GetTexts(context);
            };

            button_services.Click +=  delegate
            {                
                if (currentUser.ToLower() == "admin")
                {
                    MainFrame.Content = new ServicesPage( mc);
                    GetTexts(context);
                }
                else
                {
                    MainFrame.Content = new ServicesPageUser(mc);
                    GetTexts(context);
                }
            };

            button_projects.Click +=  delegate
            {                
                if (currentUser.ToLower() == "admin")
                {
                    MainFrame.Content = new ProjectsPage(mc);
                    GetTexts(context);
                }
                else
                {
                    MainFrame.Content = new ProjectsPageUser(mc);
                    GetTexts(context);
                }
            };

            button_blog.Click +=  delegate
            {               
                if (currentUser.ToLower() == "admin")
                {
                    MainFrame.Content = new BlogPage(mc);
                    GetTexts(context);
                }
                else
                {
                    MainFrame.Content = new BlogPageUser(mc);
                    GetTexts(context);
                }
            };

            button_contacts.Click +=  delegate
            {               
                if (currentUser.ToLower() == "admin")
                {
                    MainFrame.Content = new ContactsPage(mc);
                    GetTexts(context);
                }
                else
                {
                    MainFrame.Content = new ContactsPageUser(mc);
                    GetTexts(context);
                }
            };

            button_workpage.Click +=  delegate
            {
                MainFrame.Content = new WorkPage(mc);
                 GetTexts(context);
            };

            button_edit_texts.Click += delegate
            {
                MainFrame.Content = new ConsultingWindowEdit(mc);
            };

            button_choice_login.Click += delegate
            {
                sp_login_or_reg.Visibility = Visibility.Collapsed;
                sp_login.Visibility = Visibility.Visible;
            };

            button_login.Click += delegate
            {
                string resultLogin = contextAccount.Login(tb_enter_login.Text, tb_enter_password.Text);

                if (resultLogin == "fail")
                {
                    MessageBox.Show("Логин неуспешен, проверьте данные для ввода");
                }

                if (resultLogin == "ok")
                {
                    currentUser = tb_enter_login.Text;
                    tb_enter_login.Text = null;
                    tb_enter_password.Text = null;
                    sp_login.Visibility = Visibility.Collapsed;
                    sp_login_ok.Visibility = Visibility.Visible;
                    MainFrame.Content = new MainPage(mc);
                    GetTexts(context);
                    if (currentUser.ToLower() == "admin")
                    {
                        button_edit_texts.Visibility = Visibility.Visible;
                        button_workpage.Visibility = Visibility.Visible;
                        tb_current_user.Text = $"Текущий пользователь: {currentUser}  ";
                    }
                    tb_current_user.Text = $"Текущий пользователь: {currentUser}  ";
                }
            };

            button_back_1.Click += delegate
            {
                tb_enter_login.Text = null;
                tb_enter_password.Text = null;
                sp_login.Visibility = Visibility.Collapsed;
                sp_login_or_reg.Visibility = Visibility.Visible;

            };

            button_choice_reg.Click += delegate
            {
                sp_login_or_reg.Visibility = Visibility.Collapsed;
                sp_reg.Visibility = Visibility.Visible;     
            };

            button_reg.Click += delegate
            {
                string resultReg = contextAccount.Registartion(tb_reg_login.Text, tb_reg_password.Text);                
                if (resultReg == "fail")
                {
                    MessageBox.Show("Ошибка при создании аккаунта\r\n" +
                        "Пользователь уже существует, либо неверно составлен пароль:\r\n" +
                        "- пароль должен состоять как минимум из 6 символов\r\n" +
                        "- пароль должен содержать как минимум одну цифру от 0 до 9\r\n" +
                        "- пароль должен содержать как минимум одну букву нижнего регистра a-z\r\n" +
                        "- пароль должен содержать как минимум одну букву верхнего регистра A-Z\r\n" +
                        "- пароль должен содержать как минимум один не буквенно-цифровой символ");
                }

                if (resultReg == "ok")
                {
                    tb_reg_login.Text = null;
                    tb_reg_password.Text = null;
                    MessageBox.Show("Регистрация успешна");
                    sp_reg.Visibility = Visibility.Collapsed;
                    sp_login_or_reg.Visibility = Visibility.Visible;
                }
            };

            button_back_2.Click += delegate
            {
                tb_reg_login.Text = null;
                tb_reg_password.Text = null;
                sp_reg.Visibility = Visibility.Collapsed;
                sp_login_or_reg.Visibility = Visibility.Visible;
            };

            button_exit.Click += delegate
            {
                currentUser = "";
                MainFrame.Content = new MainPage(mc);
                GetTexts(context);
                sp_login_ok.Visibility = Visibility.Collapsed;
                sp_login_or_reg.Visibility = Visibility.Visible;
                button_edit_texts.Visibility = Visibility.Collapsed;
                button_workpage.Visibility = Visibility.Collapsed;
            };
        }
    }
}
