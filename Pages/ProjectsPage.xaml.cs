using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
using System;
using System.Collections.Generic;
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

namespace module_21_exercise_2_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ProjectsPage(MainClass mc)
        {
            InitializeComponent();

            var task = Task.Run(() => {
                return context.GetAllProjects();
            });

            List<Project> projects = task.Result as List<Project>;
            projectsListBox.ItemsSource = projects;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ProjectsLinkText}";
            DataContext = mc;

            projectsListBox.SelectionChanged += delegate
            {
                if (projectsListBox.SelectedItem != null)
                {
                    Project selectedProject = projectsListBox.SelectedItem as Project;
                    this.NavigationService.Navigate(new ProjectsDetailPage(selectedProject.Id, mc));
                }
            };

            button_add_project.Click += delegate
            {
                this.NavigationService.Navigate(new ProjectsAddPage(mc));
            };
        }

        private void button_edit_project_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = ((ListBoxItem)projectsListBox.ContainerFromElement((Button)sender)).Content as Project;
            MainClass mc = context.GetSiteTexts();
            this.NavigationService.Navigate(new ProjectsEditPage(mc, selectedProject));
        }

        private async void button_delete_project_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = ((ListBoxItem)projectsListBox.ContainerFromElement((Button)sender)).Content as Project;
            await context.DeleteProject(selectedProject.Id.ToString());            
            projectsListBox.ItemsSource = await context.GetAllProjects() as List<Project>;
        }
    }
}
