namespace DACS.Models
{
    public class SampleViewModel
    {
        public string DongSongName { get; set; }
        public string ViTriLayMau { get; set; }
        public DateTime PhieuLayMauDate { get; set; }
        public float Wo { get; set; }
        public float Qo { get; set; }
        public float WQI { get; set; }
        public string MucDo { get; set; }
        public Dictionary<string, float?> ChatValues { get; set; } = new Dictionary<string, float?>();
    }
}
