using m21_e2_WPF.Models;
using module_21_exercise_2_WPF.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для ContactsPage.xaml
    /// </summary>
    public partial class ContactsPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ContactsPage(MainClass mc)
        {            
            InitializeComponent();

            var task = Task.Run(() => {
                return context.GetAllContacts();
            });

            List<Contact> contacts = task.Result;
            contactsListBox.ItemsSource = contacts;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> {mc.siteText.MainPage_ContactsLinkText}";
            DataContext = mc;

            button_add_contact.Click += delegate
            {
                this.NavigationService.Navigate(new ContactsAddPage(mc));
            };

            button_edit_contacts_texts.Click += delegate
            {
                this.NavigationService.Navigate(new ContactsEditTextsPage(mc));
            };
        }

        private void button_edit_contact_Click(object sender, RoutedEventArgs e)
        {
            Contact selectedContact = ((ListBoxItem)contactsListBox.ContainerFromElement((Button)sender)).Content as Contact;
            MainClass mc = context.GetSiteTexts();
            this.NavigationService.Navigate(new ContactsEditPage(mc, selectedContact));
        }

        private async void button_delete_contact_Click(object sender, RoutedEventArgs e)
        {
            Contact selectedContact = ((ListBoxItem)contactsListBox.ContainerFromElement((Button)sender)).Content as Contact;
            await context.DeleteContact(selectedContact.Id.ToString());
            contactsListBox.ItemsSource = await context.GetAllContacts() as List<Contact>;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Contact selectedContact = ((ListBoxItem)contactsListBox.ContainerFromElement((Image)sender)).Content as Contact;
            var psi = new ProcessStartInfo
            {
                FileName = selectedContact.ContactLink,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
