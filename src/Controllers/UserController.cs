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
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select U.userId, U.roleId, U.locationId, U.authToken, U.isVolunteer, U.isAdmin, U.firstName, 
                        U.lastName, U.birthDate, U.gender, U.phoneNumber, U.country, U.city,
                        U.street, U.address, U.zipCode, U.registerDate,
                        COALESCE((SELECT COUNT(*) FROM dbo.UserDonations WHERE userId = U.userId), 0) AS numberOfDonations
                        from dbo.Users U";

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
        public JsonResult Post(Users  user)
        {
            string checkQuery = @"SELECT userId FROM dbo.Users WHERE authToken = '" + user.authToken + @"'";
            string query = @"
                        insert into dbo.Users (authToken, firstName, lastName, gender, phoneNumber, roleId, locationId, country, city, street, address, zipCode) values
                        (
                            '" + user.authToken + @"',
                            '" + user.firstName + @"',
                            '" + user.lastName + @"',
                            '" + user.gender + @"',
                            '" + user.phoneNumber + @"',
                            " + user.roleId + @",
                            " + user.locationId + @",
                            '" + user.country + @"',
                            '" + user.city + @"',
                            '" + user.street + @"',
                            '" + user.address + @"',
                            '" + user.zipCode + @"'
                        )";
            
            DataTable table = new DataTable();
            DataTable table2 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("UsersAppCon");

            SqlDataReader myReader, myReader2;

            bool noRows = false;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(checkQuery, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    if(myReader.HasRows == false)
                    {
                        noRows = true;
                    }
                    myReader.Close();
                }
                try
                {
                    if (noRows)
                    {
                        using (SqlCommand myCommand2 = new SqlCommand(query, myCon))
                        {
                            myReader2 = myCommand2.ExecuteReader();
                            table2.Load(myReader2);
                            myReader2.Close();
                            myCon.Close();
                        }
                    }
                }
                catch
                {

                }

            }

            return new JsonResult("Added Successfully");
        }
        
        [HttpPut]
        public JsonResult Put(Users  user)
        {
            string query = @"update dbo.Users set
                            firstName = '" + user.firstName + @"',
                            lastName = '" + user.lastName + @"',
                            authToken = '" + user.authToken + @"',
                            isVolunteer = '" + user.isVolunteer + @"',
                            isAdmin = '" + user.isAdmin + @"',
                            gender = '" + user.gender + @"',
                            phoneNumber = '" + user.phoneNumber + @"',
                            roleId = " + user.roleId + @",
                            locationId = " + user.locationId + @",
                            country = '" + user.country + @"',
                            city = '" + user.city + @"',
                            street = '" + user.street + @"',
                            address = '" + user.address + @"',
                            zipCode = '" + user.zipCode + @"' 
                            where authToken = '" + user.authToken + @"'
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
            string query = @"delete from dbo.Users where userId = " + id + @"";

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
