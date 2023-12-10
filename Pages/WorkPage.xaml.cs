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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace module_21_exercise_2_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : Page
    {
        ConsultingDataApi context = ConsultingWindow.context;
        public static DateTime dateFrom;
        public static DateTime dateTo;
        public async Task<List<Request>> RequestsByDate(DateTime dateFrom, DateTime dateTo)
        {
            List<Request> requestsByDates = new List<Request>();
            List<Request> requests = await context.GetAllRequests();
            DateTime dateTimeFrom = context.ChangeTime(dateFrom, 0, 0, 0, 0); ;
            DateTime dateTimeTo = context.ChangeTime(dateTo, 0, 0, 0, 0);
            foreach (Request req in requests)
            {
                if (req.RequestTime >= dateTimeFrom && req.RequestTime <= dateTimeTo)
                {
                    requestsByDates.Add(req);
                }
            }
            return requestsByDates;
        }
        
        public async void PostRequestsDataToPage(ConsultingDataApi context)
        {
            lv_requests.SelectedItem = null;
            button_change_status.IsEnabled = false;
            cb_request_status.Text = "";
            lv_requests.Visibility = Visibility.Visible;
            List<Request> requests = await RequestsByDate(dateFrom, dateTo);
            lv_requests.ItemsSource = requests;
            tb_selected_period.Text = $"выбранный период: {context.ChangeTime(dateFrom, 0, 0, 0, 0)} - {context.ChangeTime(dateTo, 0, 0, 0, 0)}";
            tb_requests_in_category.Text = $"заявок в выбранной категории: {requests.Count}";
            tb_requests_percent.Text = $"процент от общего количества: {context.GetRequestPercent((await context.GetAllRequests()).Count, requests.Count)}";
        }

        public WorkPage(MainClass mc)
        {
            InitializeComponent();

            var task = Task.Run(() => {
                return context.GetAllRequests();
            });

            List<Request> requests = task.Result;
            lv_requests.ItemsSource = requests;
            lv_requests.Visibility = Visibility.Hidden;
            cb_request_status.Visibility = Visibility.Hidden;
            button_change_status.Visibility = Visibility.Hidden;
            text_path.Text = $"{mc.siteText.MainPage_MainLinkText} -> Рабочий стол";
            tb_all_requests.Text = $"сумма заявок за все время: {requests.Count}";
            DataContext = mc;
            button_change_status.IsEnabled = false;

            lv_requests.SelectionChanged += delegate
            {
                cb_request_status.Visibility = Visibility.Visible;
                button_change_status.Visibility = Visibility.Visible;
                button_change_status.IsEnabled = true;
                if (lv_requests.SelectedItem != null)
                {
                    Request req = lv_requests.SelectedItem as Request;
                    cb_request_status.Text = req.RequestStatus;
                }                
            };
 
            button_requests_today.Click += delegate
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now.AddDays(1);
                PostRequestsDataToPage(context);
            };

            button_requests_yesterday.Click += delegate
            {
                dateFrom = DateTime.Now.AddDays(-1);
                dateTo = DateTime.Now;
                PostRequestsDataToPage(context);
            };

            button_requests_week.Click += delegate
             {
                 dateFrom = DateTime.Now.AddDays(-7);
                 dateTo = DateTime.Now.AddDays(1);
                 PostRequestsDataToPage(context);
             };

            button_requests_month.Click += delegate
             {
                 dateFrom = DateTime.Now.AddDays(-31);
                 dateTo = DateTime.Now.AddDays(1);
                 PostRequestsDataToPage(context);
             };

            button_requests_period.Click += delegate
             {
                 dateFrom = (DateTime)period_date_from.SelectedDate;
                 DateTime dateToTemp = (DateTime)period_date_to.SelectedDate;
                 dateTo = dateToTemp.AddDays(1);
                 PostRequestsDataToPage(context);
             };

            button_change_status.Click += async delegate
            {
                Request req = lv_requests.SelectedItem as Request;
                req.RequestStatus = cb_request_status.Text;
                await context.ChangeRequestStatus(req);
                lv_requests.ItemsSource = await RequestsByDate(dateFrom, dateTo);
                cb_request_status.Text = null;
                button_change_status.IsEnabled = false;
            };
        }
    }
}
