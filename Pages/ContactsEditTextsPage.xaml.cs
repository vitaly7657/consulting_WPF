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
using static System.Net.Mime.MediaTypeNames;

namespace module_21_exercise_2_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ContactsEditTextsPage.xaml
    /// </summary>
    public partial class ContactsEditTextsPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public ContactsEditTextsPage(MainClass mc)
        {
            InitializeComponent();
            address.Text = mc.siteText.ContactsPage_Address;
            phone.Text = mc.siteText.ContactsPage_ContactsPhone;
            fax.Text = mc.siteText.ContactsPage_ContactsFax;
            email.Text = mc.siteText.ContactsPage_ContactsEmail;
            fio.Text = mc.siteText.ContactsPage_ContactsFIO;

            button_back.Click += delegate
            {
                this.NavigationService.Navigate(new ContactsPage(mc));
            };

            button_edit_contact_texts_api.Click += async delegate
            {
                mc.siteText.ContactsPage_Address = address.Text;
                mc.siteText.ContactsPage_ContactsPhone = phone.Text;
                mc.siteText.ContactsPage_ContactsFax= fax.Text;
                mc.siteText.ContactsPage_ContactsEmail= email.Text;
                mc.siteText.ContactsPage_ContactsFIO = fio.Text;
                string result = await context.EditContactsTexts(mc);
                MessageBox.Show(result);
            };
        }
    }
}
