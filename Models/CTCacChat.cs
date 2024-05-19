using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class CTCacChat
    {
        public string Id {  get; set; }
        public float WQI { get; set; }
        public string MucDoONhiem { get; set; }
        [Required]
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        [Required]
        public string CTPhieuLayMauId { get; set; }
        public CTPhieuLayMau? CTPhieuLayMau { get; set; }
        [Required]
        public string ViTriLayMauId { get; set; }
        public ViTriLayMau? ViTriLayMau { get; set; }
    }
}
