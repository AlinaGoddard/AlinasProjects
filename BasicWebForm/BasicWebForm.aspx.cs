using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace BasicWebForm
{
    public partial class BasicRecords : Page
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        private Logger _logger;
        private Logger LoggerService
        {
            get
            {
                if(_logger == null)
                {
                    _logger = new Logger(ConfigurationManager.AppSettings["ErrorLogPath"], ConfigurationManager.AppSettings["LogPath"]);
                }
                return _logger;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Models.Person> people = new List<Models.Person>();
            try
            {
                people = GetNames();
                grdRecords.DataSource = people;
                grdRecords.DataBind();
                LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, "Number of people = {0}.", (people.Count()-1).ToString());
            }
            catch(Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Number of people = {0}.", (people.Count()-1).ToString());
            }
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
                                string name = reader.GetString(1);
                                currentId = reader.GetInt32(0);
                                Names.Add(new Models.Person(currentId, name, Models.RecordType.Edit));
                                LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    MethodBase.GetCurrentMethod().Name, "Person Id={0}, Name={1}.", currentId.ToString(), name);
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
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Reading Names.");
            }
            return Names;
        }

        //Get the row index from "e"
        //Remove that row from the database
        //Determine how to update the grid.

        protected void grdRecords_RowUpdating(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Update")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);
            //    //string name = grdRecords.Rows[id]
            //    //TODO: Update the name

            //    //TODO: I'm not sure why its not getting the up to date name
            //    string newName = ((TextBox)grdRecords.Rows[id].FindControl("txtName")).Text; 

            //    //grdRecords.DataSource = GetNames();
            //    //grdRecords.DataBind();
            //}

            //How to get the text in the textbox from the grid (maybe use the row index)
            //Validate the field to update if its not valid then put the error message into a label on the form somewhere
            //Identify the record to update then update in the database
            //If this is a record to add then insert the record
            //Determine how to update the grid.
        }
    }
}