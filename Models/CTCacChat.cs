using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class CTCacChat
    {
        public string CTCacChatId {  get; set; }
        public float GiaTri {  get; set; }
        public float WQI { get; set; }
        public string? MucDoONhiem { get; set; }
        [Required]
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        [Required]
        public int PhieuLayMauId { get; set; }
        public PhieuLayMau? PhieuLayMau { get; set; }
    }
}
