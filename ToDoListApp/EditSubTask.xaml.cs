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
using System.Configuration;


namespace ToDoListApp
{
    /// <summary>
    /// Interaction logic for EditSubTask.xaml
    /// </summary>
    public partial class EditSubTask : Window
    {
        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        private string showSubTaskName;

        public EditSubTask(string showSubTaskName)
        {
            InitializeComponent();
            this.showSubTaskName = showSubTaskName;
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            string TaskName = this.SubEditTaskName.Text;
            string TaskDescription = this.SubEditTaskDescription.Text;
            


            // MessageBox.Show(Date);
            if (TaskName == "" && TaskDescription == "")
            {
                MessageBox.Show("No Edits Were Made ");
                this.Close();
            }
            else
            {
                //creating custom  sql string
                string QueryString = "Update SubAddTask set ";

                if (TaskName != "")
                {
                    QueryString += "SubTaskName=@name,";
                }
                if (TaskDescription != "")
                {
                    QueryString += " SubTaskDescription=@Description,";
                }
               
                QueryString = QueryString.Remove(QueryString.Length - 1);

                QueryString += " where SubTaskOwner='" + Environment.UserName + "' And SubTaskName='" + showSubTaskName + "'";

                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = QueryString;
                    command.Parameters.AddWithValue("@Owner", Environment.UserName);
                    command.Parameters.AddWithValue("@Name", TaskName);
                    command.Parameters.AddWithValue("@Description", TaskDescription);
                   

                    command.ExecuteNonQuery();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                this.Close();
            }
        }

    }

}
