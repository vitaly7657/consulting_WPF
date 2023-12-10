using m21_e2_WPF.Models;
using m21_e2_WPF.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace module_21_exercise_2_WPF.Data
{
    public class ConsultingDataApi
    {
        private HttpClient httpClient { get; set; }
        public ConsultingDataApi()
        {
            httpClient = new HttpClient();            
        }

        //РАБОЧИЙ СТОЛ
        //оставить дни от даты
        public DateTime ChangeTime(DateTime dateTime, int hours, int minutes, int seconds = default, int milliseconds = default)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds, milliseconds, dateTime.Kind);
        }        

        //метод подсчёта процента заявок
        public string GetRequestPercent(int reqsAllTime, int reqsPeriod)
        {
            if (reqsAllTime != 0 && reqsPeriod != 0)
            {
                string percentReqsToday = (Convert.ToDouble(reqsPeriod) / Convert.ToDouble(reqsAllTime) * 100).ToString("#.##");
                return percentReqsToday + "%";
            }
            else
            {
                return "0";
            }
        }        

        //ЗАЯВКИ----------------------------------------------------------
        //запрос всех заявок
        public async Task <List<Request>> GetAllRequests()
        {
            string urlRequests = @"https://localhost:44380/api/application/request/";
            string jsonRequests = await httpClient.GetStringAsync(urlRequests);
            var mc = GetSiteTexts();
            mc.requestsList = JsonConvert.DeserializeObject<List<Request>>(jsonRequests);
            return mc.requestsList;
        }

        //создать заявку
        public async Task<string> CreateRequest(string request_name, string request_email, string request_text)
        {
            if (request_name == "" || request_email == "" || request_text =="")
            {
                return "Заполните все поля";
            }
            string url = @"https://localhost:44380/api/application/request/";
            RequestViewModel req = new RequestViewModel
            {
                RequesterName = request_name,
                RequestEmail = request_email,
                RequestText = request_text
            };
            var result = await httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8,
                mediaType: "application/json")
                );
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "Ошибка обработки запроса";
            }
            
            return "Ваша заявка принята";
        }

        //редактировать заявку
        public async Task<string> ChangeRequestStatus(Request request)
        {
            Request req = request;
            string url = @"https://localhost:44380/api/application/request/";
            var result = await httpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8,
                mediaType: "application/json")
                );
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "Ошибка обработки запроса";
            }
            return "ok";
        }
        
        //ТЕКСТЫ------------------------------------------------------------
        //запрос перечня текстов
        public MainClass GetSiteTexts()
        {
            //тексты страницы
            string urlText = @"https://localhost:44380/api/application/sitetext/";
            string jsonText = httpClient.GetStringAsync(urlText).Result;

            //случайный слоган
            string urlTagLine = @"https://localhost:44380/api/application/randomtagline/";
            string jsonTagLine = httpClient.GetStringAsync(urlTagLine).Result;

            //коллекция слоганов
            string urlTagLineList = @"https://localhost:44380/api/application/tagline/";
            string jsonTagLineList = httpClient.GetStringAsync(urlTagLineList).Result;

            MainClass mc = new MainClass();
            mc.siteText = JsonConvert.DeserializeObject<SiteText>(jsonText);
            mc.tagLine = JsonConvert.DeserializeObject<TagLine>(jsonTagLine);
            mc.tagLineList = JsonConvert.DeserializeObject<List<TagLine>>(jsonTagLineList);

            return mc;
        }

        //сохранение текстов с главной страницы
        public async Task<string> EditTexts(MainClass text)
        {            
            //слоганы
            List<TagLine> tl = text.tagLineList;
            tl[0].Id = 1;
            tl[1].Id = 2;
            tl[2].Id = 3;
            text.tagLineList = tl;

            string urlText = @"https://localhost:44380/api/application/sitetext/";
            var result = await httpClient.PutAsync(
                requestUri: urlText,
                content: new StringContent(JsonConvert.SerializeObject(text), Encoding.UTF8,
                mediaType: "application/json")
                );
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "Ошибка редактирования текстов";
            }
            return "Тексты успешно отредактированы";
        }

        //УСЛУГИ------------------------------------------------------------
        //запрос перечня услуг
        public async Task<IEnumerable<Service>> GetAllServices()
        {
            string urlServices = @"https://localhost:44380/api/application/services/";
            string jsonServices = await httpClient.GetStringAsync(urlServices);
            var mc = GetSiteTexts();
            mc.servicesList = JsonConvert.DeserializeObject<IEnumerable<Service>>(jsonServices);
            return mc.servicesList;
        }

        //удалить услугу
        public async Task DeleteService(string id)
        {
            string url = @$"https://localhost:44380/api/application/services/{id}";
            await httpClient.DeleteAsync(url);
        }

        //добавление услуги        
        public async Task<string> AddService(string serviceTitle, string serviceDescription)
        {
            if (serviceTitle == "" || serviceDescription == "")
            {
                return "Введите все данные";
            }
            else
            {
                string url = @"https://localhost:44380/api/application/services/";
                Service newService = new Service
                {
                    ServiceTitle = serviceTitle,
                    ServiceDescription = serviceDescription
                };
                var result = await httpClient.PostAsync(
                    requestUri: url,
                    content: new StringContent(JsonConvert.SerializeObject(newService), Encoding.UTF8,
                    mediaType: "application/json")
                    );
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return "Ошибка добавления услуги";
                }
                return "Услуга успешно добавлена";
            }                
        }

        //редактирование услуги
        public async Task<string> EditService(int id, string serviceTitle, string serviceDescription)
        {
            if (serviceTitle == "" || serviceDescription == "")
            {
                return "Введите все данные";
            }
            else
            {
                string url = @"https://localhost:44380/api/application/services/";
                Service editService = new Service
                {
                    Id = id,
                    ServiceTitle = serviceTitle,
                    ServiceDescription = serviceDescription
                };
                var result = await httpClient.PutAsync(
                    requestUri: url,
                    content: new StringContent(JsonConvert.SerializeObject(editService), Encoding.UTF8,
                    mediaType: "application/json")
                    );
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return "Ошибка редактирования услуги";
                }
                return "Услуга успешно отредактирована";
            }            
        }        

        //ПРОЕКТЫ------------------------------------------------------------
        //запрос перечня проектов
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            string urlProjects = @"https://localhost:44380/api/application/projects/";
            string jsonProjects = await httpClient.GetStringAsync(urlProjects);
            var mc = GetSiteTexts();
            mc.projectsList = JsonConvert.DeserializeObject<IEnumerable<Project>>(jsonProjects);
            return mc.projectsList;
        }

        //запрос проекта по Id
        public async Task<Project> GetProjectById(int id)
        {
            string urlProject = $@"https://localhost:44380/api/application/projects/{id}";
            string jsonProject = await httpClient.GetStringAsync(urlProject);
            var mc = GetSiteTexts();
            mc.project = JsonConvert.DeserializeObject<Project>(jsonProject);
            return mc.project;
        }

        //добавить проект
        public async Task<string> AddProject(string projectTitle, string projectDescription, string picturePath)
        {
            if (projectTitle == "" || projectDescription == "" || picturePath == "")
            {
                return "Введите все данные";
            }
            else
            {
                string url = @"https://localhost:44380/api/application/projects/";
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StreamContent(File.OpenRead(picturePath)), "PictureFile", Path.GetFileName(picturePath));
                multiContent.Add(new StringContent(projectTitle), "ProjectTitle");
                multiContent.Add(new StringContent(projectDescription), "ProjectDescription");
                var result = await httpClient.PostAsync(url, multiContent);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return "Ошибка добавления проекта";
                }
                return "Проект успешно добавлен";
            }
            
        }              

        //удалить проект        
        public async Task DeleteProject(string id)
        {
            string url = @$"https://localhost:44380/api/application/projects/{id}";
            await httpClient.DeleteAsync(url);            
        }

        //редактировать проект
        public async Task<string> EditProject(int id, string projectTitle, string projectDescription, string picturePath)
        {
            if (projectTitle == "" || projectDescription == "")
            {
                return "Введите все данные";
            }
            else
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(id.ToString()), "Id");
                multiContent.Add(new StringContent(projectTitle), "ProjectTitle");
                multiContent.Add(new StringContent(projectDescription), "ProjectDescription");
                if (picturePath == "")
                {
                    string url = @"https://localhost:44380/api/application/projectsnonepix/";
                    var result = await httpClient.PutAsync(url, multiContent);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return "Ошибка редактирования проекта";
                    }
                    return "Проект успешно отредактирован";
                }
                if (picturePath != null)
                {
                    string url = @"https://localhost:44380/api/application/projects/";
                    multiContent.Add(new StreamContent(File.OpenRead(picturePath)), "PictureFile", Path.GetFileName(picturePath));
                    var result = await httpClient.PutAsync(url, multiContent);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return "Ошибка редактирования проекта";
                    }
                    return "Проект успешно отредактирован";
                }
                return "Ошибка редактирования проекта";
            }            
        }

        //БЛОГ------------------------------------------------------------
        //запрос перечня блогов
        public async Task<IEnumerable<Blog>> GetAllBlogs()
        {
            string urlBlogs = @"https://localhost:44380/api/application/blog/";
            string jsonBlogs = await httpClient.GetStringAsync(urlBlogs);
            var mc = GetSiteTexts();
            mc.blogList = JsonConvert.DeserializeObject<IEnumerable<Blog>>(jsonBlogs);
            return mc.blogList;            
        }
        
        //запрос блога по Id
        public async Task<Blog> GetBlogById(int id)
        {
            string urlBlog = $@"https://localhost:44380/api/application/blog/{id}";
            string jsonBlog = await httpClient.GetStringAsync(urlBlog);
            var mc = GetSiteTexts();
            mc.blog = JsonConvert.DeserializeObject<Blog>(jsonBlog);
            return mc.blog;
        }

        //добавить блог
        public async Task<string> AddBlog(string blogTitle, string blogDescription, string picturePath)
        {
            if (blogTitle == "" || blogDescription == "" || picturePath == "")
            {
                return "Введите все данные";
            }
            else
            {
                string url = @"https://localhost:44380/api/application/blog/";
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StreamContent(File.OpenRead(picturePath)), "PictureFile", Path.GetFileName(picturePath));
                multiContent.Add(new StringContent(blogTitle), "BlogTitle");
                multiContent.Add(new StringContent(blogDescription), "BlogDescription");

                var result = await httpClient.PostAsync(url, multiContent);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return "Ошибка добавления блога";
                }
                return "Блог успешно добавлен";
            }
        }

        //редактировать блог
        public async Task<string> EditBlog(int id, string blogTitle, string blogDescription, string picturePath)
        {
            if (blogTitle == "" || blogDescription == "")
            {
                return "Введите все данные";
            }
            else
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(id.ToString()), "Id");
                multiContent.Add(new StringContent(blogTitle), "BlogTitle");
                multiContent.Add(new StringContent(blogDescription), "BlogDescription");
                if (picturePath == "")
                {
                    string url = @"https://localhost:44380/api/application/blognonepix/";
                    var result = await httpClient.PutAsync(url, multiContent);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return "Ошибка редактирования блога";
                    }
                    return "Блог успешно отредактирован";
                }
                if (picturePath != "")
                {
                    string url = @"https://localhost:44380/api/application/blog/";
                    multiContent.Add(new StreamContent(File.OpenRead(picturePath)), "PictureFile", Path.GetFileName(picturePath));
                    var result = await httpClient.PutAsync(url, multiContent);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return "Ошибка редактирования блога";
                    }
                    return "Блог успешно отредактирован";
                }
                return "Ошибка редактирования блога";
            }            
        }

        //удалить блог
        public async Task DeleteBlog(string id)
        {
            string url = @$"https://localhost:44380/api/application/blog/{id}";
            await httpClient.DeleteAsync(url);
        }

        //КОНТАКТЫ------------------------------------------------------------
        //запрос контактов
        public async Task<List<Contact>> GetAllContacts()
        {
            string urlContacts = @"https://localhost:44380/api/application/contacts/";
            string jsonContacts = await httpClient.GetStringAsync(urlContacts);
            var mc = GetSiteTexts();
            mc.contactsList = JsonConvert.DeserializeObject<List<Contact>>(jsonContacts);
            return mc.contactsList;
        }

        //редактирвоание ссылок контактов       
        public async Task<string> EditContact(int id, string contactTitle, string contactLink, string picturePath)
        {
            if (contactTitle == "" || contactLink == "")
            {
                return "Введите все данные";
            }
            else
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(id.ToString()), "Id");
                multiContent.Add(new StringContent(contactTitle), "ContactText");
                multiContent.Add(new StringContent(contactLink), "ContactLink");
                if (picturePath == "")
                {
                    string url = @"https://localhost:44380/api/application/contactnonepix/";
                    var result = await httpClient.PutAsync(url, multiContent);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return "Ошибка редактирования контакта";
                    }
                    return "Контакт успешно отредактирован";
                }
                if (picturePath != "")
                {
                    string url = @"https://localhost:44380/api/application/contactwithpix/";
                    multiContent.Add(new StreamContent(File.OpenRead(picturePath)), "PictureFile", Path.GetFileName(picturePath));
                    var result = await httpClient.PutAsync(url, multiContent);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return "Ошибка редактирования контакта";
                    }
                    return "Контакт успешно отредактирован";
                }
                return "Ошибка редактирования контакта";
            }            
        }

        //удалить ссылку на контакт
        public async Task DeleteContact(string id)
        {
            string url = @$"https://localhost:44380/api/application/contacts/{id}";
            await httpClient.DeleteAsync(url);
        }      

        //добавить ссылку на контакт
        public async Task<string> AddContact(string contactTitle, string contactLink, string picturePath)
        {
            if (contactTitle == "" || contactLink == "" || picturePath == "")
            {
                return "Введите все данные";
            }
            else
            {
                string url = @"https://localhost:44380/api/application/contacts/";
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StreamContent(File.OpenRead(picturePath)), "PictureFile", Path.GetFileName(picturePath));
                multiContent.Add(new StringContent(contactTitle), "ContactText");
                multiContent.Add(new StringContent(contactLink), "ContactLink");
                var result = await httpClient.PostAsync(url, multiContent);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return "Ошибка добавления контакта";
                }
                return "Контакт успешно добавлен";
            }            
        }

        //сохранение текстов страницы контактов
        public async Task<string> EditContactsTexts(MainClass text)
        {
            SiteText st = text.siteText;

            string urlText = @"https://localhost:44380/api/application/contactstexts/";
            var result = await httpClient.PutAsync(
                requestUri: urlText,
                content: new StringContent(JsonConvert.SerializeObject(text), Encoding.UTF8,
                mediaType: "application/json")
                );

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "Ошибка редактирования реквизитов";
            }
            return "Реквизиты успешно отредактированы";
        }
    }
}
