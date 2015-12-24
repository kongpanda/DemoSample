using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;

namespace SqlConnDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Thread t = new Thread(TestConn);
                t.Start();

                //Thread.Sleep(3000);
                var demo = new TestDemo();

                demo.Conn.Open();  //Uncomment this line to get double open exception, 
                string sql = "select * from AspNetUsers";
                Console.WriteLine("The DB connection state is {0} in the Main Thread.", demo.Conn.State);
                SqlCommand com = new SqlCommand(sql, demo.Conn);

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("I Got rows in Main Thread!");
                    }
                }

                demo.Conn.Close();
                Console.WriteLine("The DB connection state is {0} in Main Thread.", demo.Conn.State);
            }
            catch (Exception ex)
            {
                Console.WriteLine("I Got one exception from {0} source with {1} in Main Thread.", ex.Source, ex.Message);
            }
            finally {
                Console.ReadLine();
            }
           
        }

        static void TestConn()
        {
            try
            {
                var demo = new TestDemo();
                demo.Conn.Open();
                //Console.WriteLine("The DB connection state is {0} in the second Thread.", TestDemo.con.State);
                //Thread.Sleep(30000);   
                string sql = "select * from AspNetUsers";
                Console.WriteLine("The DB connection state is {0} in the second Thread.", demo.Conn.State);
                SqlCommand com = new SqlCommand(sql, demo.Conn);
                Thread.Sleep(6000);
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("I Got rows in the Second Thread!");
                    }
                }
                demo.Conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("I Got one exception from {0} source with {1} in the second Thread", ex.Source, ex.Message);
            }
            finally {
                Console.ReadLine();
            }
        } 

    }

    
    class TestDemo
    {
        private static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CICWebClientContext"].ConnectionString);

        public SqlConnection Conn { get { return con; } }
        
       
        //private static string connstr = ConfigurationManager.ConnectionStrings["CICWebClientContext"].ConnectionString;

        //public SqlConnection Conn { get; private set; } 
        //public TestDemo()
        //{
        //    Conn = new SqlConnection(connstr);
        //}   

        //public void DemoConnOpen()
        //{            
        //   this.Conn.Open();            
        //}

        //public void DemoConnClose() 
        //{
        //    this.Conn.Close();
        //}
        
    }

}
