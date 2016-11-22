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
using WpfApplication1;
namespace ToDoListApp
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        private string editTaskDate;
        private string editTaskName;

        public EditTask(string editTaskName, string editTaskDate)
        {
            this.editTaskName = editTaskName;
            this.editTaskDate = editTaskDate;
            InitializeComponent();
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            string TaskName = this.EditTaskName.Text;
            string TaskDescription = this.EditTaskDescription.Text;
            string Date = this.EditTime.Text;
            int NumOfDays=0;


           // MessageBox.Show(Date);
            if (TaskName=="" && TaskDescription=="" && Date=="") {
                MessageBox.Show("No Edits Were Made ");
                this.Close();
            }
            else{
                //creating custom string
                string QueryString = "Update AddTask set ";

                if (TaskName != "") {
                    QueryString += "TaskName=@name,";
                }
                if (TaskDescription != "") {
                    QueryString += " TaskDescription=@Description,";
                }
                if (Date != "")
                {
                    DateTime ConvDate = Convert.ToDateTime(Date);
                    DateTime TodayDate = DateTime.Now;
                    NumOfDays = (int)(ConvDate - TodayDate).TotalDays;

                    QueryString += " TaskDate=@Date, TaskDays=@Days,";
                }
                QueryString = QueryString.Remove(QueryString.Length - 1);

                QueryString += " where TaskOwner='" + Environment.UserName + "' And TaskName='" + editTaskName + "' And TaskDate='" + editTaskDate + "'";

                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = QueryString;
                    command.Parameters.AddWithValue("@Owner", Environment.UserName);
                    command.Parameters.AddWithValue("@Name", TaskName);
                    command.Parameters.AddWithValue("@Description", TaskDescription);
                    command.Parameters.AddWithValue("@Date", Date);
                    command.Parameters.AddWithValue("@Days", NumOfDays);
                    
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                main.GridRefresh();
                this.Close();
            }

        }
    }
}
