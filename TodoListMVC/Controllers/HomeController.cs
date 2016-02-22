using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.Configuration;
using TodoListMVC.Models;
using System.Reflection;
using System.Data.SqlClient;

namespace TodoListMVC.Controllers
{
    public class HomeController : Controller
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        DataContext db = new DataContext(ConnectionString);
        private Logger _logger;
        private Logger LoggerService
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger(ConfigurationManager.AppSettings["ErrorLogPath"], ConfigurationManager.AppSettings["LogPath"]);
                }
                return _logger;
            }
        }       

        public ActionResult Index()
        {
            return View(db.GetTable<TodoItem>().ToList());
        }

        public ActionResult Edit(string todoId, string taskName, string priority, string dueDate)
        {
            IEnumerable<TodoItem> result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("update TodoList set TaskName = '" + taskName + "', Priority = '" + priority + "', DueDate = '" + dueDate + "' where TodoId=" + todoId, conn))
                    {
                        command.ExecuteNonQuery();
                        LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                            MethodBase.GetCurrentMethod().Name, "Update Complete, TaskName={0}, Priority={1}, DueDate={2}, TodoId={3}.", taskName, priority, dueDate, todoId);
                    }
                    conn.Close();
                }
                result = db.GetTable<TodoItem>().ToList();
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Updating Todo Item.");
            }
            return View(result); 
        }

        public ActionResult Delete(string todoId)
        {
            IEnumerable<TodoItem> result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("delete TodoList where TodoId=" + todoId, conn))
                    {
                        command.ExecuteNonQuery();
                        LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                            MethodBase.GetCurrentMethod().Name, "Delete Complete, todoId={0}.", todoId);
                    }
                    conn.Close();
                }
                result = db.GetTable<TodoItem>().ToList();
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Deleteing Todo Item.");
            }
            return View(result);
        }

        public ActionResult Add(string taskName, string priority, string dueDate)
        {
            IEnumerable<TodoItem> result = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("insert TodoList(TaskName, Priority, DueDate) values('" + taskName + "', " + priority + ", '" + dueDate + "')", conn))
                    {
                        command.ExecuteNonQuery();
                        LoggerService.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.Name,
                            MethodBase.GetCurrentMethod().Name, "Insert Complete, TaskName={0}, Priority={1}, DueDate={2}.", taskName, priority, dueDate);
                    }
                    conn.Close();
                }
                result = db.GetTable<TodoItem>().ToList();
            }
            catch (Exception ex)
            {
                LoggerService.WriteError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                    MethodBase.GetCurrentMethod().Name, ex, "Exception Inserting Todo Item.");
            }
            return View(result); 
        }
    }
}