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
                string rowIdDelete = Request.QueryString["id"];                
                if (!string.IsNullOrEmpty(rowIdDelete))
                    DeleteRecord(GetPersonId(rowIdDelete));
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

        private string GetPersonId(string rowId)
        {
            return ((List<Models.Person>)Session["Names"]).Skip(Convert.ToInt32(rowId)).First().Id.ToString();
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
                        int currentId = 0;
                        if (reader.HasRows)
                        {                           
                            while (reader.Read())
                            {
                                string name = reader.GetString(1);
                                currentId = reader.GetInt32(0);
                                Names.Add(new Models.Person(currentId, name, Models.RecordType.Edit));
                                LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    MethodBase.GetCurrentMethod().Name, "Person Id={0}, Name={1}.", currentId.ToString(), name);
                            }                           
                        }
                        Names.Add(new Models.Person(currentId + 1, string.Empty, Models.RecordType.Add));
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
            Session["Names"] = Names;
            return Names;
        }

        private bool NameIsValid(string name)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(name))
                message = "Name is a required field.";
            else if (name.Trim().Length < 2)
                message = "Name must have at least 1 character.";

            lblErrors.Text = message;
            if (string.IsNullOrEmpty(message))
            {
                LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, "Name={0} is valid.", name);
                return true;
            }
            else 
            {
                LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, "Name={0} is not valid, message={1}.", name, message);
                return false;
            }
        }

        protected void grdRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try 
            {
                int id = e.NewEditIndex;
                string newName = ((TextBox)grdRecords.Rows[id].FindControl("txtName")).Text;
                if (NameIsValid(newName))
                {
                    string type = ((Button)grdRecords.Rows[id].Controls[2].Controls[0]).Text;
                    if (type == "Edit")
                        UpdateName(newName, GetPersonId(id.ToString()));
                    else if (type == "Add")
                        InsertName(newName);
                }
                grdRecords.EditIndex = ((List<Models.Person>)Session["Names"]).Count + 4;
                grdRecords.DataSource = GetNames();
                grdRecords.DataBind();
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, string.Empty);
            }           
        }

        private void UpdateName(string newName, string id)
        {
            try 
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("update Names set name = '" + newName + "' where id=" + id, conn))
                    {
                        command.ExecuteNonQuery();
                        LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                            MethodBase.GetCurrentMethod().Name, "Update Complete, Name={0} and Id={1}.", newName, id);
                    }
                    conn.Close();
                }               
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Updating Names.");
            }
        }

        private void InsertName(string name)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("insert Names(name) values('" + name + "')", conn))
                    {
                        command.ExecuteNonQuery();
                        LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                            MethodBase.GetCurrentMethod().Name, "Insert Complete, Name={0}.", name);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Inserting Names.");
            }
        }

        private void DeleteRecord(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("delete Names where id=" + id, conn))
                    {
                        command.ExecuteNonQuery();
                        LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                            MethodBase.GetCurrentMethod().Name, "Delete Complete, Id={0}.", id);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Deleting Records.");
            }           
        }
    }
}