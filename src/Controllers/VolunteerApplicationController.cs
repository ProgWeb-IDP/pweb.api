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
                        select VA.applicationId, VA.userId, VA.applicationStatus, VA.roleId, VA.locationId, VA.summary, U.firstName, U.lastName, VR.roleName
                        from dbo.VolunteerApplications VA, dbo.Users U, dbo.VolunteerRoles VR
                        where VA.userId = U.userId and VR.roleId = VA.roleId and VA.applicationStatus = 1";

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
                        select VA.applicationId, VA.userId, VA.applicationStatus, VA.roleId, VA.locationId, VA.summary, U.firstName, U.lastName, VR.roleName, L.locationName
                        from dbo.VolunteerApplications VA, dbo.Users U, dbo.VolunteerRoles VR, dbo.Locations L
                        where VA.userId = U.userId and VR.roleId = VA.roleId and L.locationId = VA.locationId and VA.applicationId = " + id + @"";

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
                        insert into dbo.VolunteerApplications (userId, roleId, locationId, summary) values
                        (
                            '" + application.userId + @"',
                            '" + application.roleId + @"',
                            '" + application.locationId + @"',
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
            string query2 = null;
            if(application.applicationStatus == 2) // Approve
            {
                  query2 = @"update dbo.Users set
                            roleId = '" + application.roleId + @"',
                            locationId = '" + application.locationId + @"',
                            isVolunteer = 1 where userId = '" + application.userId + @"'";
            }


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
                if(query2 != null)
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query2, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
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
