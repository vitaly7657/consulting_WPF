using Microsoft.AspNetCore.Http;

namespace m21_e2_WPF.Models
{
    public class ContactViewModel
    {
        public int? Id { get; set; }
        public string ContactText { get; set; }
        public string ContactLink { get; set; }
        public IFormFile? PictureFile { get; set; }
    }
}
