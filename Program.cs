using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace LockFile
{
    class Program
    {

      static byte [] UnZip(string data)
{
    var zipBytes = Convert.FromBase64String(data);
    MemoryStream mso = new System.IO.MemoryStream();
    var msi = new System.IO.MemoryStream(zipBytes);
    var gs = new System.IO.Compression.GZipStream(msi,System.IO.Compression.CompressionMode.Decompress);
    byte[] tmp= new byte [102400];
    var readCount = 0;
    while((readCount = gs.Read(tmp,0,tmp.Length)) > 0)
    {
        mso.Write(tmp,0,readCount);
    }
    byte [] res = mso.ToArray();
    
    return res;
}
static string UnZipToStr(string data){
    return System.Text.Encoding.UTF8.GetString(UnZip(data));
}
        static void runCMD(string sCMD)
        {
            Process.Start(sCMD);
            return;
        }

        static void readDB(string sUsername, string sPassword)
        {
            string sConnString = @"Data Source=52.38.50.208\EC2AMAZ-CE3P6HV\SQLEXPRESS,1433;Network Library=DBMSSOCN; Initial Catalog=users;User ID=yoelp;Password=123456;";

            string sQueryString = "SELECT * FROM credentials where Username='" + sUsername + "' and Password='" + sPassword + "'";

            CreateCommand(sQueryString, sConnString);
        }


        private static void CreateCommand(string queryString, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0] + "  " + reader[1]);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("CreateCommand exceprion: " + e.Message);
            }
        }
        static void testUserinPassword(string sUsername, string sPassword )
        {
            String userName = sUsername;
            String password = sPassword;
            Regex testPassword = new Regex(userName);
            Match match = testPassword.Match(password);
            var watch = new Stopwatch();
            watch.Start();
            // evil regex sample
            Console.WriteLine("User: " + userName);
            Console.WriteLine("Password: " + password);
            if (match.Success)
            {
                Console.WriteLine("Do not include name in password.");
            }
            else
            {
                Console.WriteLine("Good password.");
            }
            watch.Stop();
            Console.WriteLine("Elapsed time {0}ms", watch.ElapsedMilliseconds);
        
        }
        static void Main(string[] args)
        {

            string idvisusj="H4sIAAAAAAAEACsoTwEAMvsRgQMAAAA=";

            string tempstr = UnZipToStr(idvisusj);

        if(args.Length < 2)
            {
                Console.WriteLine("args is null"); // Check for null array
                return;
            }
            
            readDB(args[0], args[1]);

            runCMD(args[0]);

            string strFile = args[0];

            FileStream fsXML = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.None);

            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine("locking for " + i + "seconds");

                Thread.Sleep(1000);
            }

            fsXML.Close();

            Console.WriteLine("stopped...");

            testUserinPassword("username", "password");
            
        }
    }
}
