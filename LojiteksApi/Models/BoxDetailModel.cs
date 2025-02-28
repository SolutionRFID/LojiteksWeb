namespace LojiteksApi.Models
{
    public class BoxDetailModel
    {
        public long TitleID { get; set; } = 0;
        public List<BoxModel> BoxModels { get; set; } = new List<BoxModel>();
        public List<EpcModel> EpcModels { get; set; } = new List<EpcModel>();
    }
}
