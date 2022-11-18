using cumulative_project.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        public IEnumerable<Teacher>ListTeachers(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command(query) for out database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";
            
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey+ "%");
            cmd.Prepare();

            //Gather Result set of Query into a variable
            MySqlDataReader ResultSet=cmd.ExecuteReader();

            //Create a empty list of teachers
            List<Teacher> Teachers = new List<Teacher> {};

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

                Teacher NewTeacher=new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary= Salary;
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
            MySqlDataReader ResultSet=cmd.ExecuteReader();

            while(ResultSet.Read())
            {
                //Access column informarion by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname =(string) ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];


                NewTeacher.TeacherId=TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                NewTeacher.EmployeeNumber = EmployeeNumber;


            }

            return NewTeacher;

        }


    }
}
