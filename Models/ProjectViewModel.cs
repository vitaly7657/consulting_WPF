using Microsoft.AspNetCore.Http;

namespace m21_e2_WPF.Models
{
    public class ProjectViewModel
    {
        public int? Id { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public IFormFile? PictureFile { get; set; }
    }
}
