using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace MyFirstWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class ValuesController : ApiController
    {
        // GET api/values
        //IEnumerable = List
        public IEnumerable<StudentVM> Get()
        {
            var dt = ShowData();
            List<StudentVM> list = new List<StudentVM>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new StudentVM
                {
                    id = Convert.ToInt16(item["id"]),
                    Name = item["Name"].ToString(),
                    Fee = Convert.ToInt16(item["Fee"]),
                    IsPaid = Convert.ToBoolean(item["IsPaid"]),
                    subjects = Convert.ToString(item["subjects"]),
                    a_date = Convert.ToDateTime(item["a_date"]),
                    userId = Convert.ToInt32(item["userId"])

                });
          
            }

            return list;
        }

        // GET api/values/5
        public StudentVM Get(int id)
        {
            var dt = ShowData();
            List<StudentVM> list = new List<StudentVM>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new StudentVM
                {
                    id = Convert.ToInt16(item["id"]),
                    Name = item["Name"].ToString(),
                    Fee = Convert.ToInt16(item["Fee"]),
                    IsPaid = Convert.ToBoolean(item["IsPaid"]),
                    subjects = Convert.ToString(item["subjects"]),
                    a_date = Convert.ToDateTime(item["a_date"]),
                    userId = Convert.ToInt32(item["userId"])
                });
            }
            StudentVM student = list.FirstOrDefault(x => x.id == id);
            return student;
        }

        // POST api/values
        public void Post(StudentVM model)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
            SqlCommand command = null;
            connection.Open();
            if (model.id > 0)
            {
                command = new SqlCommand("update students set Name = @Name , Fee = @Fee , IsPaid =  @IsPaid, phoneNo = @phoneNo, a_date = @a_date where id = @id ", connection);
                command.Parameters.AddWithValue("id", model.id);
            }
            else
            {
                //hyderabad sindh pakistani 1100 km from islamabad
                command = new SqlCommand("insert into students (Name , Fee , IsPaid , phoneNo, a_date) VALUES (@Name,  @Fee , @IsPaid , @phoneNo, @a_date) ", connection);
            }
            command.Parameters.AddWithValue("Name", model.Name);
            command.Parameters.AddWithValue("Fee", model.Fee);
            command.Parameters.AddWithValue("IsPaid", model.IsPaid);
            command.Parameters.AddWithValue("phoneNo", model.phoneNo);
           // command.Parameters.AddWithValue("subjects", model.subjects);
            command.Parameters.AddWithValue("a_date", model.a_date);
           // command.Parameters.AddWithValue("userId", model.userId);
            command.ExecuteNonQuery();
            connection.Close();
        }

        // PUT api/values/5
        public void Put(int id, StudentVM model)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
            connection.Open();
            SqlCommand command = new SqlCommand("update student Name = @Name , Fee = @Fee , IsPaid =  @IsPaid, phoneNo = @phoneNo where id = @id ", connection);
            command.Parameters.AddWithValue("Name", model.Name);
            command.Parameters.AddWithValue("Fee", model.Fee);
            command.Parameters.AddWithValue("IsPaid", model.IsPaid);
            command.Parameters.AddWithValue("phoneNo", model.phoneNo);
            command.Parameters.AddWithValue("subjects", model.subjects);
            command.Parameters.AddWithValue("a_date", model.a_date);
            command.Parameters.AddWithValue("userId", model.userId);



            command.ExecuteNonQuery();
            connection.Close();
        }

        [HttpGet, Route("Delete/{id}")]
        [EnableCorsAttribute("http://localhost:59407", "*", "*")]
        public int Delete(int id)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
            connection.Open();
            SqlCommand command = new SqlCommand("DELETE from Students where id = @id ", connection);
            command.Parameters.AddWithValue("id", id);
            int r = command.ExecuteNonQuery();
            connection.Close();
            return r;
        }

        protected DataTable ShowData()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
            connection.Open();
            SqlCommand command = new SqlCommand("Select Id, IsNull(NAME, '')NAME, IsNull(FEE, 0)FEE , IsNull(SCHOOLid, 0)SCHOOL,IsNull(GENDER, 0) GENDER,IsNull(IsPAID, 0) IsPAID,IsNull(SUBJECTS, '') SUBJECTS,IsNull(a_date, '1/1/1910')  a_date,IsNull(userId, 0) userId from Students", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            connection.Close();
            return dt;
        }
    }
    public class StudentVM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public decimal Fee { get; set; }
        public bool IsPaid { get; set; }
        public string phoneNo { get; set; }
        public string subjects { get; set; }
        public DateTime a_date { get; set; }
        public int userId { get; set; }
    }

    public static class Extensions
    {
        public static string IfNull(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }
    }
}
