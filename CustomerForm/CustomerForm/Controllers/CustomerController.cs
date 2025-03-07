using CustomerForm.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CustomerForm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly string _connectionString;

        public CustomerController(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("defaultconnection");
        }

        [HttpGet("getallsubmission")]
        public ActionResult Getallsubmission()
        {
            List<CustomerSubmission1> submissions = new List<CustomerSubmission1>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetCustomerSubmissions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submissions.Add(new CustomerSubmission1
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                PhoneNo = reader["PhoneNo"].ToString(),
                                Email = reader["Email"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
            }
            return Ok(submissions); // Return as JSON response
        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomerSubmission1 submission)
        {
            if (submission == null || string.IsNullOrWhiteSpace(submission.Name))
            {
                return BadRequest(new { error = "Invalid submission data." });
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CreateCustomerSubmission", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", submission.Name);
                cmd.Parameters.AddWithValue("@PhoneNo", submission.PhoneNo);
                cmd.Parameters.AddWithValue("@Email", submission.Email);
                cmd.Parameters.AddWithValue("@Subject", submission.Subject);
                cmd.Parameters.AddWithValue("@Description", submission.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok(new { message = "Submission created successfully.", data = submission });
        }


        [HttpPut("update/{id}")]
        public IActionResult Update(int id, CustomerSubmission1 submission)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateCustomerSubmission", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", submission.Name);
                cmd.Parameters.AddWithValue("@PhoneNo", submission.PhoneNo);
                cmd.Parameters.AddWithValue("@Email", submission.Email);
                cmd.Parameters.AddWithValue("@Subject", submission.Subject);
                cmd.Parameters.AddWithValue("@Description", submission.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteCustomerSubmission", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok();
        }
    }
}
