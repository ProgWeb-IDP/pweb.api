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

        [HttpPost]
        public JsonResult Post(HelpARefugee.Models.UserDonations donation)
        {
            string query = @"
                        insert into dbo.UserDonations (userId, volunteerId, donationRequestId, quantityDonated, emissionDate, collectionDate, completionDate, donationStatus) 
                        values
                        (
                            '" + donation.userId + @"',
                            '" + donation.volunteerId + @"',
                            '" + donation.donationRequestId + @"',
                            '" + donation.quantityDonated + @"',
                            '" + donation.emissionDate + @"',
                            '" + donation.collectionDate + @"',
                            '" + donation.completionDate + @"',
                            '" + donation.donationStatus + @"'
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
