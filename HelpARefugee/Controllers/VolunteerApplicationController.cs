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
    public class VolunteerApplication : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VolunteerApplication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select applicationId, userId, applicationStatus, role, summary from dbo.VolunteerApplications";

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
        public JsonResult Post(HelpARefugee.Models.VolunteerApplications application)
        {
            string query = @"
                        insert into dbo.VolunteerApplications (userId, role, summary) values
                        (
                            '" + application.userId + @"',
                            '" + application.role + @"',
                            '" + application.summary + @"'
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
        public JsonResult Put(HelpARefugee.Models.VolunteerApplications application)
        {
            string query = @"update dbo.VolunteerApplications set
                            applicationStatus = '" + application.applicationStatus + @"'         
                            where applicationId = " + application.applicationId + @"
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
            string query = @"delete from dbo.VolunteerApplications where applicationId = " + id + @"";

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
