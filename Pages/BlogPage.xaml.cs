using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using static System.Net.Mime.MediaTypeNames;

namespace module_21_exercise_2_WPF.Pages
{

    /// <summary>
    /// Логика взаимодействия для BlogPage.xaml
    /// </summary>
    public partial class BlogPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public BlogPage(MainClass mc)
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

            button_add_blog.Click += delegate
            {
                this.NavigationService.Navigate(new BlogAddPage(mc));
            };            
        }

        private void button_edit_blog_Click(object sender, RoutedEventArgs e)
        {
            Blog selectedBlog = ((ListBoxItem)blogListBox.ContainerFromElement((Button)sender)).Content as Blog;
            MainClass mc = context.GetSiteTexts();
            this.NavigationService.Navigate(new BlogEditPage(mc, selectedBlog));
        }

        private async void button_delete_blog_Click(object sender, RoutedEventArgs e)
        {
            Blog selectedBlog = ((ListBoxItem)blogListBox.ContainerFromElement((Button)sender)).Content as Blog;
            await context.DeleteBlog(selectedBlog.Id.ToString());
            blogListBox.ItemsSource = await context.GetAllBlogs() as List<Blog>;
        }
    }
}
