using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class PhieuLayMau
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float Wo {  get; set; }
        [Required]
        public float Qo {  get; set; }
        public string? EmployeeId { get; set; }
        public ApplicationUser? Employee { get; set; }

        [Required]
        public string ViTriLayMauId { get; set; }
        public ViTriLayMau? ViTriLayMau { get; set; }
    }
}
