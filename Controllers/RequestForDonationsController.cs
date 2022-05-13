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
                        select donationRequestId, volunteerId, requestStatus, resourceType,
                        quantityNeeded, shortDescription, emissionDate, processingDate, completionDate
                        from dbo.RequestForDonations";

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
                        insert into dbo.RequestForDonations (volunteerId, requestStatus, resourceType, quantityNeeded,
                        shortDescription, emissionDate, processingDate, completionDate) 
                        values
                        (
                            '" + donationRequest.volunteerId + @"',
                            '" + donationRequest.requestStatus + @"',
                            '" + donationRequest.resourceType + @"',
                            '" + donationRequest.quantityNeeded + @"',
                            '" + donationRequest.shortDescription + @"',
                            '" + donationRequest.emissionDate + @"',
                            '" + donationRequest.processingDate + @"',
                            '" + donationRequest.completionDate + @"'
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
                            volunteerId = '" + donationRequest.volunteerId + @"',
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
