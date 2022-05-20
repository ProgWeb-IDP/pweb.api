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
    public class RequestForDonationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RequestForDonationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select RD.donationRequestId, U.firstName, U.lastName, RD.resourceType, RD.emissionDate
                        from dbo.RequestForDonations RD, dbo.Users U
                        where RD.volunteerId = U.userId and requestStatus = 1";

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
                        select U.firstName, U.lastName, RD.resourceType, RD.quantityNeeded, RD.shortDescription, RD.emissionDate,
                        RD.volunteerId, RD.requestStatus, RD.processingDate, RD.completionDate, RD.donationRequestId,
                        COALESCE((SELECT SUM(quantityDonated) FROM dbo.UserDonations WHERE donationRequestId = RD.donationRequestId), 0) AS quantityGathered
                        from dbo.RequestForDonations RD, dbo.Users U
                        where RD.volunteerId = U.userId
                        and RD.donationRequestId = " + id + @"";

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
        public JsonResult Post(HelpARefugee.Models.RequestForDonations donationRequest)
        {
            string query = @"
                        insert into dbo.RequestForDonations (volunteerId, resourceType, quantityNeeded,
                        shortDescription) 
                        values
                        (
                            '" + donationRequest.volunteerId + @"',
                            '" + donationRequest.resourceType + @"',
                            '" + donationRequest.quantityNeeded + @"',
                            '" + donationRequest.shortDescription + @"'
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
        public JsonResult Put(HelpARefugee.Models.RequestForDonations donationRequest)
        {
            string query = @"update dbo.RequestForDonations set
                            requestStatus = '" + donationRequest.requestStatus + @"',
                            resourceType = '" + donationRequest.resourceType + @"',
                            quantityNeeded = '" + donationRequest.quantityNeeded + @"',
                            shortDescription = '" + donationRequest.shortDescription + @"',
                            emissionDate = '" + donationRequest.emissionDate + @"',
                            processingDate = '" + donationRequest.processingDate + @"',
                            completionDate = '" + donationRequest.completionDate + @"'
                            where donationRequestId = " + donationRequest.donationRequestId + @"
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
            string query = @"delete from dbo.RequestForDonations where donationRequestId = " + id + @"";

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
