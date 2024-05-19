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
        [Required]
        public int ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
