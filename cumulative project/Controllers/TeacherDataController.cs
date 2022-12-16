using cumulative_project.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace cumulative_project.Controllers
{
    public class TeacherDataController : ApiController
    {
        //The database context class which allows us to access our MySQL database.
        private SchoolDbContext school = new SchoolDbContext();

        //This controller will access the school table of our school database.
        ///<summary>
        ///Return a list of Teachers in the system
        /// </summary>
        /// <example>
        /// GET api/TeacherData/ListTeachers
        /// </example>
        /// <returns>
        /// A List of Teachers (First name, last name)
        /// </returns>


        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for out database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create a empty list of teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access information by the DB column name as an index

                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                NewTeacher.EmployeeNumber = EmployeeNumber;

                //Add the teacher name to the list
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySql database and the webserver
            Conn.Close();


            //Return the final list of teacher names
            return Teachers;


        }




        ///<summary>
        ///Find an teacher in the system given and ID
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <returns>a teacher project</returns>


        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open The connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //Sql Query
            cmd.CommandText = "select * from Teachers where teacherid= " + id;

            //Gather Result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access column informarion by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];


                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                NewTeacher.EmployeeNumber = EmployeeNumber;


            }

            return NewTeacher;

        }
        /// <summary>
        /// Delete an Teacher from the connected Mysql database if the Id of the teacher exsists.
        /// </summary>
        /// <param name="id"></param>
        /// <example>POST: /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //create an instance of a connection
            MySqlConnection conn = school.AccessDatabase();

            //open the connection between server and database
            conn.Open();

            //Establish a new command for our databse
            MySqlCommand cmd = conn.CreateCommand();

            //sql query
            cmd.CommandText = "delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            conn.Close();


        }
        /// <summary>
        /// adds an teacher to mysql database.
        /// </summary>
        /// <param name="NewTeacher"> an object with fields that map to the columns of the teacher's table. Non-Deterministic 
        /// </param>
        /// <example>
        /// POST api/TeacherData/AddTeacher
        /// {
        /// "TeacherFname" : "Manali",
        /// "TeacherLname" : "Patel",
        /// "HireDate": 21/10/2022 12:00,
        /// "salary" : 22.77,
        /// "EmployeeNumber": T202
        ///}
        /// </example>
        [HttpPost]
        //[EnableCors(origin: "*", methods: "*", headers: "*")]
        public void AddTeacher(Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            Debug.WriteLine(NewTeacher.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, hiredate, salary, employeenumber) values (@TeacherFname,@TeacherLname,@HireDate, @salary, @EmployeeNumber)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
        ///<summary>
        ///Update a Teacher in the system
        ///<param name="TeacherId"> The id of the teacher in the system</param>
        ///<param name="UpdatedTeacher">Post content, teacher body include name,date and salary</param>
        ///</summary>
        ///<example>
        ///api/teacherdata/updateteacher/16
        ///</example>
        ///POST: POST CONTENT / FORM BODY/ REQUEST BODY

        [HttpPost]
        [Route("api/TeacherData/UpdateTeacher/{TeacherId}")]

        public void UpdateTeacher(int TeacherId, [FromBody] Teacher UpdatedTeacher)
        {
            MySqlConnection Conn = school.AccessDatabase();
           
            Debug.WriteLine("updating teacher with an id of " + TeacherId);
            Debug.WriteLine(UpdatedTeacher.TeacherId);
            Debug.WriteLine(UpdatedTeacher.TeacherFname);
            Debug.WriteLine(UpdatedTeacher.Salary);

            
            Conn.Open();
            
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "upadate teacher set teacherfname=@TeacherFname, TeacherLname=@TeacherLname, HireDate=@HireDate, Salary=@salary, EmployeeNumber=@EmployeeNumber where teacherid=@id";
            cmd.Parameters.AddWithValue("@TeacherId", UpdatedTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@TeacherFname", UpdatedTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", UpdatedTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@HireDate", UpdatedTeacher.HireDate);
            cmd.Parameters.AddWithValue("@EmployeeNumber", UpdatedTeacher.EmployeeNumber);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();



        }

    }
}

