using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YourNamespace
{
    public class AddSampleViewModel
    {
        [Required]
        public string DongSongId { get; set; }
        public IEnumerable<SelectListItem> DongSongs { get; set; }

        [Required]
        public string ViTriLayMauId { get; set; }
        public IEnumerable<SelectListItem> ViTriLayMaus { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public float Wo { get; set; }

        [Required]
        public float Qo { get; set; }

        public string EmployeeId { get; set; }  // Optional Employee ID field

        public Dictionary<string, float?> ChatValues { get; set; } = new Dictionary<string, float?>();

        public float? WQI { get; set; }
        public string? MucDoONhiem { get; set; }
    }
}
