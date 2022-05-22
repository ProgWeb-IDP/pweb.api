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
    public class VolunteerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VolunteerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select U.userId, U.roleId, U.locationId, U.authToken, U.isVolunteer, U.isAdmin, U.firstName, U.lastName, U.birthDate, U.gender, 
                        U.phoneNumber, U.country, U.city, U.street, U.address, U.zipCode, U.registerDate, VR.roleName, L.locationName
                        from dbo.Users U, dbo.Locations L, dbo.VolunteerRoles VR
                        where U.roleId = VR.roleId and L.locationId = U.locationId and U.isVolunteer = 1";

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
        public JsonResult Get(int id) // Get Volunteer Donation Requests
        {
            string query = @"
                        select RD.donationRequestId, RD.resourceType, RD.emissionDate, RD.quantityNeeded, RD.requestStatus,
                        COALESCE((SELECT SUM(quantityDonated) FROM dbo.UserDonations WHERE donationRequestId = RD.donationRequestId), 0) AS quantityGathered
                        from dbo.RequestForDonations RD, dbo.Users U
                        where RD.volunteerId = U.userId and U.userId = '" + id + @"'";

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

        [HttpPut]
 

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"UPDATE dbo.Users SET isVolunteer = 0, roleId = NULL, locationId = NULL WHERE userId = " + id + @"";

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
