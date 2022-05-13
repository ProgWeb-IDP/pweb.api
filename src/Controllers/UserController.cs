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
                        select userId, authToken, isVolunteer, isAdmin, firstName, lastName, birthDate, gender, phoneNumber, country, city, street, address, zipCode, registerDate from dbo.Users";

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
            string query = @"
                        insert into dbo.Users (authToken, firstName, lastName, gender, phoneNumber, country, city, street, address, zipCode) values
                        (
                            '" + user.authToken + @"',
                            '" + user.firstName + @"',
                            '" + user.lastName + @"',
                            '" + user.gender + @"',
                            '" + user.phoneNumber + @"',
                            '" + user.country + @"',
                            '" + user.city + @"',
                            '" + user.street + @"',
                            '" + user.address + @"',
                            '" + user.zipCode + @"'
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
                            country = '" + user.country + @"',
                            city = '" + user.city + @"',
                            street = '" + user.street + @"',
                            address = '" + user.address + @"',
                            zipCode = '" + user.zipCode + @"',
                            where authToken = " + user.authToken + @"
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
