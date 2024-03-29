using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Covid_19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidController : Controller
    {
        private IConfiguration _configuration;
        private string connectionString;
        public CovidController(IConfiguration configuration)
        {
            this._configuration = configuration;
            connectionString = _configuration.GetConnectionString("Covid-19Database"); ;
        }

        public string getStringConnection()
        {
            return _configuration.GetConnectionString("Covid-19Database");
        }

        [HttpGet]
        [Route("GetEmployeePersonalDetails")]
        public List<EmployeePersonalDetail> getEmployeePersonalDetails()
        {
            List<EmployeePersonalDetail> employeePersonalDetails = new List<EmployeePersonalDetail>();

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                string sqlQuery = "select * from dbo.EmployeePersonalDetails";

                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    EmployeePersonalDetail employeePersonalDetail = new EmployeePersonalDetail
                    {
                        FirstName = row.Field<string>("FirstName"),
                        LastName = row.Field<string>("LastName"),
                        IdentityCard = row.Field<string>("IdentityCard"),
                        City = row.Field<string>("City"),
                        Street = row.Field<string>("Street"),
                        HouseNumber = row.Field<string>("HouseNumber"),
                        DateOfBirth = row.Field<DateTime>("DateOfBirth"),
                        Phone = row.Field<string>("Phone"),
                        MobilePhone = row.Field<string>("MobilePhone")
                    };

                    employeePersonalDetails.Add(employeePersonalDetail);
                }
            }

            return employeePersonalDetails;
        }

        [HttpGet]
        [Route("GetEmployeePersonalDetailByID/{employeeID}")]
        public IActionResult GetEmployeePersonalDetailByID(string employeeID)
        {
            if (!IsValidIdentityCard(employeeID))
            {
                return BadRequest("Invalid employee ID format");
            }

            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeePersonalDetails WHERE IdentityCard = @IdentityCard";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdentityCard", employeeID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }
            }

            List<EmployeePersonalDetail> employeePersonalDetail = ConvertDataTable<EmployeePersonalDetail>(table);

            if (employeePersonalDetail.Count == 0)
            {
                return NotFound("Employee ID not found");
            }

            return new JsonResult(employeePersonalDetail);
        }

        [HttpGet]
        [Route("GetEmployeeVaccinationByID/{employeeID}")]
        public IActionResult GetEmployeeVaccinationByID(string employeeID)
        {
            if (!IsValidIdentityCard(employeeID))
            {
                return BadRequest("Invalid employee ID format");
            }

            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeeVaccinations WHERE IdentityCard = @IdentityCard";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdentityCard", employeeID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }
            }

            List<EmployeeVaccination> employeeVaccination = ConvertDataTable<EmployeeVaccination>(table);

            if (employeeVaccination.Count == 0)
            {
                return NotFound("Employee ID not found");
            }

            return new JsonResult(employeeVaccination);
        }



        // Helper method to convert DataTable to a list of objects
        private List<T> ConvertDataTable<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();
                foreach (DataColumn col in table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(col.ColumnName);
                    if (prop != null && row[col] != DBNull.Value)
                    {
                        prop.SetValue(obj, row[col]);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        [HttpGet]
        [Route("GetEmployeeVaccinations")]
        public List<EmployeeVaccination> getEmployeeVaccinations()
        {
            List<EmployeeVaccination> employeeVaccinations = new List<EmployeeVaccination>();

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                string sqlQuery = "select * from dbo.EmployeeVaccinations";

                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    EmployeeVaccination employeeVaccination = new EmployeeVaccination
                    {
                        IdentityCard = row.Field<string>("IdentityCard"),
                        Vaccine1Date = row.Field<DateTime?>("Vaccine1Date"),
                        Vaccine1Manufacturer = row.Field<string>("Vaccine1Manufacturer"),
                        Vaccine2Date = row.Field<DateTime?>("Vaccine2Date"),
                        Vaccine2Manufacturer = row.Field<string>("Vaccine2Manufacturer"),
                        Vaccine3Date = row.Field<DateTime?>("Vaccine3Date"),
                        Vaccine3Manufacturer = row.Field<string>("Vaccine3Manufacturer"),
                        Vaccine4Date = row.Field<DateTime?>("Vaccine4Date"),
                        Vaccine4Manufacturer = row.Field<string>("Vaccine4Manufacturer"),
                        PositiveResultDate = row.Field<DateTime?>("PositiveResultDate"),
                        RecoveryDate = row.Field<DateTime?>("RecoveryDate"),
                    };

                    employeeVaccinations.Add(employeeVaccination);
                }
            }

            return employeeVaccinations;
        }

        [HttpPost]
        [Route("AddEmployeePersonalDetail")]
        public IActionResult addEmployeePersonalDetail(EmployeePersonalDetail employeePersonalDetail)
        {
            // Validate input data integrity
            if (!IsValidEmployeePersonalDetail(employeePersonalDetail))
            {
                return BadRequest("Invalid input data");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.EmployeePersonalDetails (FirstName, LastName, IdentityCard, City, Street, HouseNumber, DateOfBirth, Phone, MobilePhone)
                                    VALUES (@FirstName, @LastName, @IdentityCard, @City, @Street, @HouseNumber, @DateOfBirth, @Phone, @MobilePhone)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", employeePersonalDetail.FirstName);
                        command.Parameters.AddWithValue("@LastName", employeePersonalDetail.LastName);
                        command.Parameters.AddWithValue("@IdentityCard", employeePersonalDetail.IdentityCard);
                        command.Parameters.AddWithValue("@City", employeePersonalDetail.City);
                        command.Parameters.AddWithValue("@Street", employeePersonalDetail.Street);
                        command.Parameters.AddWithValue("@HouseNumber", employeePersonalDetail.HouseNumber);
                        command.Parameters.AddWithValue("@DateOfBirth", employeePersonalDetail.DateOfBirth);
                        command.Parameters.AddWithValue("@Phone", employeePersonalDetail.Phone);
                        command.Parameters.AddWithValue("@MobilePhone", employeePersonalDetail.MobilePhone);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return Ok("employeePersonalDetail added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the employeePersonalDetail: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("AddEmployeeVaccination")]
        public IActionResult addEmployeeVaccination(EmployeeVaccination employeeVaccination)
        {
            if (!IsValidEmployeeVaccination(employeeVaccination))
            {
                return BadRequest("Invalid input data");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO dbo.EmployeeVaccinations (IdentityCard, Vaccine1Date, Vaccine1Manufacturer, Vaccine2Date, Vaccine2Manufacturer, Vaccine3Date, Vaccine3Manufacturer, Vaccine4Date, Vaccine4Manufacturer, PositiveResultDate, RecoveryDate)
                             VALUES (@IdentityCard, @Vaccine1Date, @Vaccine1Manufacturer, @Vaccine2Date, @Vaccine2Manufacturer, @Vaccine3Date, @Vaccine3Manufacturer, @Vaccine4Date, @Vaccine4Manufacturer, @PositiveResultDate, @RecoveryDate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdentityCard", employeeVaccination.IdentityCard);
                        command.Parameters.AddWithValue("@Vaccine1Date", (object)employeeVaccination.Vaccine1Date ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Vaccine1Manufacturer", (object)employeeVaccination.Vaccine1Date == null ? DBNull.Value : (object)employeeVaccination.Vaccine1Manufacturer);
                        command.Parameters.AddWithValue("@Vaccine2Date", (object)employeeVaccination.Vaccine2Date ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Vaccine2Manufacturer", (object)employeeVaccination.Vaccine2Date == null ? DBNull.Value : (object)employeeVaccination.Vaccine2Manufacturer);
                        command.Parameters.AddWithValue("@Vaccine3Date", (object)employeeVaccination.Vaccine3Date ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Vaccine3Manufacturer", (object)employeeVaccination.Vaccine3Date == null ? DBNull.Value : (object)employeeVaccination.Vaccine3Manufacturer);
                        command.Parameters.AddWithValue("@Vaccine4Date", (object)employeeVaccination.Vaccine4Date ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Vaccine4Manufacturer", (object)employeeVaccination.Vaccine4Date == null ? DBNull.Value : (object)employeeVaccination.Vaccine4Manufacturer);
                        command.Parameters.AddWithValue("@PositiveResultDate", (object)employeeVaccination.PositiveResultDate == null ? DBNull.Value : (object)employeeVaccination.PositiveResultDate);
                        command.Parameters.AddWithValue("@RecoveryDate", (object)employeeVaccination.RecoveryDate == null ? DBNull.Value : (object)employeeVaccination.RecoveryDate);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return Ok("EmployeeVaccination added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the employeeVaccination: {ex.Message}");
            }
        }

        private bool IsValidEmployeePersonalDetail(EmployeePersonalDetail employeePersonalDetail)
        {
            if (!IsValidIdentityCard(employeePersonalDetail.IdentityCard))
            {
                return false;
            }

            // Check if DateOfBirth is in the past
            if (employeePersonalDetail.DateOfBirth > DateTime.Now)
            {
                return false;
            }

            return true;
        }

        private bool IsValidIdentityCard(string identityCard)
        {
            // Check if IdentityCard is a valid ID
            return ((identityCard.Count(char.IsDigit) == 9) && // only 9 digits
                    (identityCard[2] != '4' && identityCard[2] != '9')); //3rd digit can not be equal to 4 or 
        }
        
        private bool IsValidEmployeeVaccination(EmployeeVaccination employeeVaccination)
        {
            if (employeeVaccination == null)
            {
                return false;
            }

            if (!IsValidIdentityCard(employeeVaccination.IdentityCard))
            {
                return false;
            }

            DateTime? dateOfBirth = GetDateOfBirth(employeeVaccination.IdentityCard);

            if (dateOfBirth == null)
            {
                return false;
            }

            if (employeeVaccination.Vaccine1Date == null &&
                employeeVaccination.Vaccine2Date == null &&
                employeeVaccination.Vaccine3Date == null &&
                employeeVaccination.Vaccine4Date == null)
            {
                return true;
            }

            if ((employeeVaccination.Vaccine1Date == null &&
                !string.IsNullOrEmpty(employeeVaccination.Vaccine1Manufacturer))
                || (employeeVaccination.Vaccine2Date == null &&
                !string.IsNullOrEmpty(employeeVaccination.Vaccine2Manufacturer))
                || (employeeVaccination.Vaccine3Date == null &&
                !string.IsNullOrEmpty(employeeVaccination.Vaccine3Manufacturer))
                || (employeeVaccination.Vaccine4Date == null &&
                !string.IsNullOrEmpty(employeeVaccination.Vaccine4Manufacturer))
                )
            {
                return false;
            }

            if ((employeeVaccination.Vaccine1Date != null &&
                string.IsNullOrEmpty(employeeVaccination.Vaccine1Manufacturer))
                || (employeeVaccination.Vaccine2Date != null &&
                string.IsNullOrEmpty(employeeVaccination.Vaccine2Manufacturer))
                || (employeeVaccination.Vaccine3Date != null &&
                string.IsNullOrEmpty(employeeVaccination.Vaccine3Manufacturer))
                || (employeeVaccination.Vaccine4Date != null &&
                string.IsNullOrEmpty(employeeVaccination.Vaccine4Manufacturer))
                )
            {
                return false;
            }

            if ((employeeVaccination.Vaccine1Date != null && employeeVaccination.Vaccine1Date.Value < dateOfBirth)
                || (employeeVaccination.Vaccine2Date != null && employeeVaccination.Vaccine2Date.Value < dateOfBirth)
                || (employeeVaccination.Vaccine3Date != null && employeeVaccination.Vaccine3Date.Value < dateOfBirth)
                || (employeeVaccination.Vaccine4Date != null && employeeVaccination.Vaccine4Date.Value < dateOfBirth)
                )
            {
                return false; 
            }

            if ((employeeVaccination.PositiveResultDate != null && employeeVaccination.PositiveResultDate.Value < dateOfBirth)
                || (employeeVaccination.RecoveryDate != null && employeeVaccination.RecoveryDate.Value < dateOfBirth)
                || (employeeVaccination.PositiveResultDate == null && employeeVaccination.RecoveryDate != null)
                )
            {
                return false;
            }

            return true;
        }



        private DateTime? GetDateOfBirth(string identityCard)
        {
            DateTime? dateOfBirth = null;
            string query = "SELECT DateOfBirth FROM EmployeePersonalDetails WHERE IdentityCard = @IdentityCard";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdentityCard", identityCard);
                    object result = command.ExecuteScalar();
                    if (result != null && DateTime.TryParse(result.ToString(), out DateTime parsedDateOfBirth))
                    {
                        dateOfBirth = parsedDateOfBirth;
                    }
                }
            }

            return dateOfBirth;
        }



    }
}
