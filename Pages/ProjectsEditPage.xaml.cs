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
    /// Логика взаимодействия для ProjectsEditPage.xaml
    /// </summary>
    public partial class ProjectsEditPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ProjectsEditPage(MainClass mc, Project project)
        {
            InitializeComponent();
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ProjectsLinkText} -> Редактирование";
            project_title.Text = project.ProjectTitle;
            project_description.Text = project.ProjectDescription;

            button_back.Click += delegate
            {
                this.NavigationService.Navigate(new ProjectsPage(mc));
            };

            button_edit_project_api.Click += async delegate
            {
                string result = await context.EditProject(project.Id, project_title.Text, project_description.Text, file_path.Text);
                MessageBox.Show(result);
            };
        }

        private void select_file_Click(object sender, RoutedEventArgs e)
        {
            //диалоговое окно выбора файла
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                file_path.Text = filename;
            }
        }
    }
}
