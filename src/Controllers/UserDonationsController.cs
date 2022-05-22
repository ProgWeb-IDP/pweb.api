using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Globalization;
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
                        select UD.donationId, UD.userId, UD.volunteerId, UD.donationRequestId,
                        UD.quantityDonated, UD.emissionDate, UD.collectionDate, UD.completionDate, UD.donationStatus, 
                        U.firstName, U.lastName, U.country, U.city, U.street, U.address, RD.resourceType
                        from dbo.UserDonations UD, dbo.Users U, dbo.RequestForDonations RD
                        where U.userId = UD.userId and UD.donationRequestID = RD.donationRequestId and UD.donationStatus != 0";

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

            string query2 = @"UPDATE dbo.RequestForDonations
                    SET requestStatus = 3 
                    WHERE dbo.RequestForDonations.quantityNeeded = (SELECT SUM(quantityDonated) 
                    FROM dbo.UserDonations 
                    WHERE donationRequestId = dbo.RequestForDonations.donationRequestId)
            ";

            DataTable table = new DataTable();
            DataTable table2 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("UsersAppCon");

            SqlDataReader myReader, myReader2;

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
                myCon.Open();
                using (SqlCommand myCommand2 = new SqlCommand(query2, myCon))
                {
                    myReader2 = myCommand2.ExecuteReader();
                    table2.Load(myReader2);
                    myReader2.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(HelpARefugee.Models.UserDonations donation)
        {
            if (donation.donationStatus == 3) donation.collectionDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            if (donation.donationStatus == 4) donation.completionDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string query = @"update dbo.UserDonations set
                            userId = '" + donation.userId + @"',
                            volunteerId = '" + donation.volunteerId + @"',
                            donationRequestId = '" + donation.donationRequestId + @"',
                            quantityDonated = " + donation.quantityDonated + @",
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

            return new JsonResult("Updated Successfully!");
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
