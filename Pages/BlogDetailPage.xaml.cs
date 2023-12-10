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
    /// Логика взаимодействия для BlogDetailPage.xaml
    /// </summary>
    public partial class BlogDetailPage : Page
    {       
        ConsultingDataApi context = ConsultingWindow.context;
        public BlogDetailPage(int id, MainClass mc)
        {
            InitializeComponent();
            
            var task = Task.Run(() => {
                return context.GetBlogById(id);
            });

            Blog currentBlog = task.Result;
            this.DataContext = currentBlog;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_BlogLinkText} -> {currentBlog.BlogTitle}";

            button_back.Click += delegate
            {
                if (ConsultingWindow.currentUser.ToLower() == "admin")
                {
                    this.NavigationService.Navigate(new BlogPage(mc));
                }
                else
                {
                    this.NavigationService.Navigate(new BlogPageUser(mc));
                }
            };
        }
    }
}
