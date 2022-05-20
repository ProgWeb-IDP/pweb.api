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
    public class UserDonationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserDonationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select donationId, userId, volunteerId, donationRequestId,
                        quantityDonated, emissionDate, collectionDate, completionDate, donationStatus
                        from dbo.UserDonations";

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

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"
                        select UD.donationId, UD.userId, UD.volunteerId, UD.donationRequestId,
                        UD.quantityDonated, UD.emissionDate, UD.collectionDate, UD.completionDate, UD.donationStatus, RD.resourceType
                        from dbo.UserDonations UD, dbo.RequestForDonations RD
                        where UD.donationRequestId = RD.donationRequestId and UD.userId = " + id + @"";

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

        [HttpPost]
        public JsonResult Post(HelpARefugee.Models.UserDonations donation)
        {
            string query = @"
                        insert into dbo.UserDonations (userId, donationRequestId, quantityDonated, donationStatus, volunteerId) 
                        values
                        (
                            '" + donation.userId + @"',
                            '" + donation.donationRequestId + @"',
                            '" + donation.quantityDonated + @"',
                            '" + donation.donationStatus + @"', NULL
                        )";

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

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(HelpARefugee.Models.UserDonations donation)
        {
            string query = @"update dbo.UserDonations set
                            userId = '" + donation.userId + @"',
                            volunteerId = '" + donation.volunteerId + @"',
                            donationRequestId = '" + donation.donationRequestId + @"',
                            quantityDonated = '" + donation.quantityDonated + @"',
                            emissionDate = '" + donation.emissionDate + @"',
                            collectionDate = '" + donation.collectionDate + @"',
                            completionDate = '" + donation.completionDate + @"',
                            donationStatus = '" + donation.donationStatus + @"'
                            where donationId = " + donation.donationId + @"
                            ";

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

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.UserDonations where donationId = " + id + @"";

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

            return new JsonResult("Deleted Successfully");
        }

    }
}
