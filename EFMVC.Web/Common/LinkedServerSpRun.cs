using System.Data;
using System.Data.SqlClient;

namespace EFMVC.Web.Common
{
   
    public static class LinkedServerSpRun
    {

        public static void  ExecuteLinkedSP(string spname, string conn)
        {
            

            using (SqlConnection con = new SqlConnection(conn))
            {

                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //ViewBag.ExecuteNonQuery = "Yes";
                }
            }
        }
       

       


    }
}