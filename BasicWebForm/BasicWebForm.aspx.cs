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
                    using (SqlCommand command = new SqlCommand("select id, name from Names", conn))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Names.Add(new Models.Person(reader.GetInt32(0), reader.GetString(1)));
                            }
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

    }
}