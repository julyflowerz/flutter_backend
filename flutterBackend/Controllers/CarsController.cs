using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using flutterBackend.Model;

namespace flutterBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CarsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string query = "SELECT * FROM Cars";
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            List<Car> cars = new();

            using (SqlConnection con = new(connectionString))
            {
                con.Open();
                SqlCommand cmd = new(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cars.Add(new Car
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"]?.ToString(),
                        Make = reader["make"]?.ToString()
                    });
                }

                reader.Close();
            }

            return Ok(cars);
        }
    }
}
