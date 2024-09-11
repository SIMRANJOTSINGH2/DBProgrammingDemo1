using System.Data.SqlClient;
using System.Data;
namespace DBProgrammingDemo1
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=(local);database=Northwind;Integrated Security=SSPI";

            //CAll the ConnectionDemo1 methord
            ConnectionDemo1(connectionString);

            //Call the ConnectionDemo2 methord
            //Managing the unmanged objects with the using the statement
            ConnectionDemo2(connectionString);

        }

        private static void ConnectionDemo2(string connectionString)
        {
            string sql = "SELECT * FROM Products ORDER BY ProductName";

            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Console.WriteLine($"ProductId: {dr["ProductId"]} Product Name: {dr["ProductName"]} Unit Price: {dr["UnitPrice"]:c}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No records found");
                        }
                    } // close and dispose reader
                } // dispose of command
            } // close and dispose of connections
        }

        private static void ConnectionDemo1(string connectionString)
        {
            //Unmanaged Objects
            //Connection
            //Command
            //DataReader

            //We need to manage the unmanged objects

            //SqlConnection
            SqlConnection conn = new(connectionString);

            //Sql Command 
            SqlCommand cmd = new();

            SqlDataReader dr = null;

            try
            {
                //Set the connection of the command
                cmd.Connection = conn;

                String sql = "SELECT * FROM Shippers ORDER BY CompanyName";

                //Set the command text to execute
                cmd.CommandText = sql;

                //Set the Comman dtype of my command text
                cmd.CommandType = CommandType.Text;

                //Open the Connnection
                conn.Open();

                //Create the DR(Data Reader)
                dr = cmd.ExecuteReader();

                int shipperId;
                string? CompanyName;
                string? phone;

                //Check if DR has rows
                if (dr.HasRows)
                {
                    Console.WriteLine("WE HAVE RECORDS");
                    Console.WriteLine($"There are {dr.FieldCount} Columns in our rows");

                    //Count the number of rows in the stream
                    int rowCount = 0;

                    //Loop the datareader
                    //dr.Reader() returns true if there are still records and moves to the next row in th stream
                    while (dr.Read()) 
                    {
                        //track the row Count
                        rowCount++;
                        shipperId = Convert.ToInt32(dr["ShipperId"]); // Covert the datatype to a integer of 32 bit
                        CompanyName= dr["CompanyName"].ToString();
                        phone = dr["Phone"].ToString();

                        Console.WriteLine($"ShipperId :{shipperId} Company Name = {CompanyName} Phone = {phone}");
                    }
                    Console.WriteLine($"We have {rowCount} shippers");

                }
                else
                {
                    Console.WriteLine("No Records found");
                }
            }
 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //Clean up Unmanged Resources

                if (dr != null)
                {
                    dr.Close(); //Close the reader stream
                    dr.Dispose(); // Frees up memory
                }

                cmd.Dispose();// free up memory

                conn.Close(); // Close the database Connection

                conn.Dispose(); // free up memory
            }
        }
    }
}
