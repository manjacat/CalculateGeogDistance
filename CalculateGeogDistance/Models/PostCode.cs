namespace CalculateGeogDistance.Models
{
    public class PostCode
    {
        public PostCode()
        {
            id = 0;
            postcode = string.Empty;
            latitude = "0";
            longitude = "0";
        }

        //id,postcode,latitude,longitude
        public long id { get; set; }
        public required string postcode { get; set; }
        public required string latitude { get; set; }
        public required string longitude { get; set; }
    }
}
