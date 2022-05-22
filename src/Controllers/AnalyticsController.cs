using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using HelpARefugee.Models;

namespace HelpARefugee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AnalyticsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT 
                (SELECT COUNT(*) from dbo.Users) AS numberOfUsers,
                (SELECT COUNT(*) from dbo.Users WHERE isVolunteer = 1) AS numberOfVolunteers,
                (SELECT COUNT(*) from dbo.UserDonations WHERE donationStatus = 0) AS canceledDonations,
                (SELECT COUNT(*) from dbo.UserDonations WHERE donationStatus = 1) AS pendingDonations,
                (SELECT COUNT(*) from dbo.UserDonations WHERE donationStatus = 2) AS acceptedDonations,
                (SELECT COUNT(*) from dbo.UserDonations WHERE donationStatus = 3) AS collectedDonations,
                (SELECT COUNT(*) from dbo.UserDonations WHERE donationStatus = 4) AS deliveredDonations,
                (SELECT COUNT(*) from dbo.RequestForDonations WHERE requestStatus = 0) AS rejectedDonationRequests,
                (SELECT COUNT(*) from dbo.RequestForDonations WHERE requestStatus = 1) AS pendingDonationRequests,
                (SELECT COUNT(*) from dbo.RequestForDonations WHERE requestStatus = 2) AS runningDonationRequests,
                (SELECT COUNT(*) from dbo.RequestForDonations WHERE requestStatus = 3) AS completedDonationRequests,
                (SELECT SUM(quantityDonated) from dbo.UserDonations) AS totalResourcesDonated,
                ((SELECT SUM(quantityDonated) from dbo.UserDonations) / (SELECT COUNT(*) from dbo.Users)) AS averageResourcesDonatedPerUser";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("UsersAppCon");

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
    }
}
