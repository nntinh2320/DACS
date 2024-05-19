using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class CTPhieuLayMau
    {
        public string CTPhieuLayMauId {  get; set; }
        [Required]
        public int PhieuLayMauId { get; set; }
        public PhieuLayMau? PhieuLayMau { get; set; }
        [Required]
        public string ViTriLayMauId { get; set; }
        public ViTriLayMau? ViTriLayMau { get; set; }
    }
}
