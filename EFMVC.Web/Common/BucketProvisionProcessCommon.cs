using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public class BucketProvisionProcessCommon
    {
        public void ProvisionProcess(string externalServerConnString, string connctionString, string file, string externalFile, string processBucketAuditSPName, string dbConnectionString)
        {

            //Change the bucket status from Live to Spent and Pending to Live on external server
            using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
            {
                using (MySqlCommand cmd = new MySqlCommand("change_bucket_status", conn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            // Import the local MYSQL dump file data to external MYSQL db
            using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
            {

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 18000;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }


           
            //Update the bucket Item Table
            using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
            {

                using (MySqlCommand cmd = new MySqlCommand("update_bucket_item_data", conn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            // Generate the external MYSQL dump file
            //string externalFile = "D:\\AdtoneLatest\\SQLFileBackup\\MySQL_Bucket_ExternalFile.sql";
            using (MySqlConnection conn = new MySqlConnection(externalServerConnString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 18000;
                        conn.Open();
                        mb.ExportInfo.ExportTableStructure = false;
                        mb.ExportInfo.ExportProcedures = false;
                        mb.ExportInfo.ExportFunctions = false;
                        mb.ExportInfo.ExportViews = false;
                        //mb.ExportInfo.TablesToBeExportedList = new List<string> {
                        //        "bucket",
                        //        "bucket_batch",
                        //        "bucket_item"
                        // };

                        mb.ExportInfo.TablesToBeExportedList = new List<string> {

                                    "spent_bucket_batch",
                                    "spent_bucket",
                                    "spent_bucket_item"
                            };
                        mb.ExportToFile(externalFile);
                        conn.Close();

                    }
                }
            }

            //Truncate the Spent Table Data Of External Server
            using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
            {
                using (MySqlCommand cmd = new MySqlCommand("truncate_spent_records", conn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            // Import the External MYSQL dump file data to local MYSQL db
            using (MySqlConnection conn = new MySqlConnection(connctionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 18000;
                        conn.Open();
                        mb.ImportFromFile(externalFile);
                        conn.Close();
                    }
                }
            }

            // Process the campaign audit table
            ExecuteLinkedSP(processBucketAuditSPName, dbConnectionString);

            // Truncate the Spent Table Data Of Local Server
            using (MySqlConnection conn = new MySqlConnection(connctionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("truncate_spent_records", conn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }


        }

        public void ExecuteLinkedSP(string spname, string conn)
        {

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;                   
                    con.Open();
                    cmd.ExecuteNonQuery();                   
                    con.Close();
                }
            }
        }
    }
}