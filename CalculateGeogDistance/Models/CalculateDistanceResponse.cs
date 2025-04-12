namespace CalculateGeogDistance.Models
{
    public class CalculateDistanceResponse
    {
        public required PostCode postCodeA { get; set; }
        public required PostCode postCodeB { get; set; }
        public double distance { get; set; }
        public string unitOfMeasureMent { get { return "KM"; } }
    }
}
