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
    /// Логика взаимодействия для BlogPageUser.xaml
    /// </summary>
    public partial class BlogPageUser : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public BlogPageUser(MainClass mc)
        {
            InitializeComponent();

            var task = Task.Run(() => {
                return context.GetAllBlogs();
            });

            List<Blog> blogs = task.Result as List<Blog>;
            blogListBox.ItemsSource = blogs;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_BlogLinkText}";
            DataContext = mc;

            blogListBox.SelectionChanged += delegate
            {
                if (blogListBox.SelectedItem != null)
                {
                    Blog selectedBlog = blogListBox.SelectedItem as Blog;
                    this.NavigationService.Navigate(new BlogDetailPage(selectedBlog.Id, mc));
                }
            };
        }
    }
}
