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
    /// Логика взаимодействия для ContactsEditPage.xaml
    /// </summary>
    public partial class ContactsEditPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ContactsEditPage(MainClass mc, Contact contact)
        {
            InitializeComponent();
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ContactsLinkText} -> Редактирование";
            contact_title.Text = contact.ContactText;
            contact_link.Text = contact.ContactLink;

            button_back.Click += delegate
            {
                this.NavigationService.Navigate(new ContactsPage(mc));
            };

            button_edit_contact_api.Click += async delegate
            {
                string result = await context.EditContact(contact.Id, contact_title.Text, contact_link.Text, file_path.Text);
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
