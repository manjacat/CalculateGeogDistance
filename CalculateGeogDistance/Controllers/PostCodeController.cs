using CalculateGeogDistance.Models;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CalculateGeogDistance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCodeController : ControllerBase
    {
        [HttpGet(Name = "GetPostcodes")]
        public IActionResult Get()
        {
            using var reader = new StreamReader("Data/ukpostcodes.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<PostCode>()
                .Take(100).ToList();

            return Ok(records);
        }

        [Authorize]
        [HttpPost("CalculateDistance", Name = "CalculateDistance")]
        public IActionResult CalculateDistance(CalculateDistanceRequest req)
        {
            using var reader = new StreamReader("Data/ukpostcodes.csv");                       
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<PostCode>().ToList();            
            PostCode locationA = records
                .FirstOrDefault(x => x.postcode == req.postCode1);
            PostCode locationB = records
                .FirstOrDefault(x => x.postcode == req.postCode2);
            double distance = CalculateDistanceInKM(locationA, locationB);

            CalculateDistanceResponse response = new CalculateDistanceResponse
            {
                postCodeA = locationA ?? new PostCode { id= 0, postcode="", latitude="0", longitude="0"},
                postCodeB = locationB ?? new PostCode { id = 0, postcode = "", latitude = "0", longitude = "0" },
                distance = distance,
            };
            return Ok(response);
        }

        #region private Methods
        private double CalculateDistanceInKM(PostCode? locationA, PostCode? locationB)
        {
            if (locationA == null || locationB == null) 
            {
                return 0;
            }

            const double NAUTICAL_MILES = 1.1515;
            const double KM_IN_MILES = 1.609344;

            double longA = Convert.ToDouble(locationA.longitude);
            double longB = Convert.ToDouble(locationB.longitude);
            double latA = Convert.ToDouble(locationA.latitude);
            double latB = Convert.ToDouble(locationB.latitude);

            double theta = longA - longB ;
            double dist = Math.Acos(
                (Math.Sin(double.DegreesToRadians(latA)) *
                Math.Sin(double.DegreesToRadians(latB))) +
                (Math.Cos(double.DegreesToRadians(latA)) *
                Math.Cos(double.DegreesToRadians(latB))) *
                Math.Cos(double.DegreesToRadians(theta))
                );

            dist = double.RadiansToDegrees(dist);
            dist = dist * 60;
            return dist * NAUTICAL_MILES * KM_IN_MILES;
        }
        #endregion
    }
}
