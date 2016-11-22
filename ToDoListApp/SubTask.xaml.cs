using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using WpfApplication1;
using System.Data;
using System.Configuration;
namespace ToDoListApp
{
    /// <summary>
    /// Interaction logic for SubTask.xaml
    /// </summary>
    public partial class SubTask : Window
    {
        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        private string SubTaskDate;
        private string SubTaskName;


        public SubTask(string editTaskName, string editTaskDate)
        {
            InitializeComponent();
            this.SubTaskName = editTaskName;
            this.SubTaskDate = editTaskDate;
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            
            string TaskName = this.TaskName.Text;
            string TaskDescription = this.TaskDescription.Text;
            

            if (TaskName == "")
            {
                MessageBox.Show("Name Field cannot be empty");
            }
            else
            {

                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);

                    SQLiteCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "Select TaskID From AddTask where TaskOwner='" + Environment.UserName + "' And TaskName='" + SubTaskName + "' And TaskDate='" + SubTaskDate + "'";

                    SQLiteDataReader ReadData = command.ExecuteReader();
                    if (ReadData.HasRows)
                    {
                        ReadData.Read();
                        String TaskID = ReadData["TaskID"].ToString();


                        ReadData.Close();
                        command.CommandText = "Insert into SubAddTask(SubTaskOwner, SubTaskName, SubTaskDescription, SubTaskCompleted, SubID) Values(@Owner,@Name, @Description,@Completed, @SubID)";
                        command.Parameters.AddWithValue("@Owner", Environment.UserName);
                        command.Parameters.AddWithValue("@Name", TaskName);
                        command.Parameters.AddWithValue("@Description", TaskDescription);
                        command.Parameters.AddWithValue("@Completed", "No");
                        command.Parameters.AddWithValue("@SubID", TaskID);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                    else { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("SubTask Added Sucesfully");
                this.Close();
            }
        }
    }
}