namespace LojiteksApi.Models
{
    public class BoxModel
    {
        public long BoxID { get; set; } = 0;
        public long TotalEPCCount { get; set; } = 0;
        public bool? Deleted { get; set; } = false;
        public DateTime? CreatedDate { get; set; } = DateTime.MinValue;
    }
}
