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
    /// Логика взаимодействия для ProjectsPageUser.xaml
    /// </summary>
    public partial class ProjectsPageUser : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ProjectsPageUser(MainClass mc)
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

                    Frame tempFrame = new Frame();
                    tempFrame.Content = new ProjectsDetailPage(selectedProject.Id, mc);
                    this.Content = tempFrame;
                }
            };            
        }        
    }
}
