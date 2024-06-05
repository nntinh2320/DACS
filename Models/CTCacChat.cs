using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS.Models
{
    public class CTCacChat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CTCacChatId {  get; set; }
        public float GiaTri {  get; set; }
        public float WQI { get; set; }
        public string? MucDoONhiem { get; set; }
        [Required]
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        //public ICollection<Chat> Chats { get; set; } = new List<Chat>();
        [Required]
        public int PhieuLayMauId { get; set; }
        public PhieuLayMau? PhieuLayMau { get; set; }

    }
}
