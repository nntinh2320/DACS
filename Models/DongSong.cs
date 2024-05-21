using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class DongSong
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }

        public List<ViTriLayMau>? ViTriLayMau { get; set; }
    }
}
