using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class ViTriLayMau
    {
        public string Id { get; set; }
        [Required]
        public DongSong? DongSong { get; set; }
        public string DongSongId { get; set; }
    }
}
