using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace BasicWebForm
{
    public partial class BasicRecords : Page
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            grdRecords.DataSource = GetNames();
            grdRecords.DataBind();
        }

        private List<Models.Person> GetNames()
        {
            List<Models.Person> Names = new List<Models.Person>();
            try
            {               
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("select id, name from Names order by id", conn))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            int currentId = 0;
                            while (reader.Read())
                            {
                                currentId = reader.GetInt32(0);
                                Names.Add(new Models.Person(currentId, reader.GetString(1), Models.RecordType.Edit));
                            }
                            Names.Add(new Models.Person(currentId + 1, string.Empty, Models.RecordType.Add));
                        }
                        reader.Close();
                    }
                    conn.Close();
                }               
            }
            catch (Exception ex)
            {
                //Some error handeling logic
            }
            return Names;
        }

        protected void grdRecords_RowDeleting(object sender, GridViewDeleteEventArgs e) 
        {
            //Get the row index from "e"
            //Remove that row from the database
            //Determine how to update the grid.
        }

        protected void grdRecords_RowUpdating(object sender, GridViewCommandEventArgs e)
        {
            //Get the row index from "e"
            //How to get the text in the textbox from the grid (maybe use the row index)
            //Validate the field to update if its not valid then put the error message into a label on the form somewhere
            //Identify the record to update then update in the database
            //If this is a record to add then insert the record
            //Determine how to update the grid.
        }
    }
}