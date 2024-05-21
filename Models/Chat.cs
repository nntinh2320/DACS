using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class Chat
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}
