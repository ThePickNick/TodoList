using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using System.Windows;

namespace ToDoListApp
{
    
    class Mark_As_Done
    {
        // private
        private string taskdate;
        private string taskname;

        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        private  string EnvName = Environment.UserName;

        public string Taskdate { set { taskdate = value; }}
        public string Datetask { set { taskname = value; }}

        //Class for Back-end database button
        public void MarkAsDone()
        {
           
            try
            {
                SQLiteConnection connection = new SQLiteConnection(ConfString);
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "Select TaskCompleted From AddTask where TaskOwner='" + EnvName + "' And TaskName='" + taskname + "' And TaskDate='" + taskdate + "' AND TaskCompleted='Yes'";

                //MessageBox.Show(command.CommandText);
                SQLiteDataReader ReadData = command.ExecuteReader();

                if (ReadData.HasRows)
                {
                    //MessageBox.Show("Going into if");
                    MessageBox.Show("Task already has been marked as completed");
                    connection.Close();

                }
                else
                {
                    //MessageBox.Show("Going into else");
                    ReadData.Close();
                    command.CommandText = "Update AddTask set TaskCompleted ='Yes' where TaskOwner='" + EnvName + "' And TaskName='" + taskname + "' And TaskDate='" + taskdate + "'";
                   // MessageBox.Show(command.CommandText);
                    command.ExecuteNonQuery();
                    
                    connection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

    }

    
}
