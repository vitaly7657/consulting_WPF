using Microsoft.AspNetCore.Http;

namespace m21_e2_WPF.Models
{
    public class MultipartClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Pix { get; set; }
    }
}
