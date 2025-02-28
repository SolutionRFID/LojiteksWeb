namespace LojiteksWeb.Models
{
    public class ShipmentDetailsViewModel
    {
        public long? BoxNo { get; set; }
        public string BoxBarcode { get; set; }
        public int? BoxInCount { get; set; }
        public DateTime? ReadingDate { get; set; }
        public List<EpcDetailsViewModel> EpcList { get; set; }
    }

    public class EpcDetailsViewModel
    {
        public string EPC { get; set; }
        public string UPC { get; set; }
        public string Size { get; set; }
    }
}
