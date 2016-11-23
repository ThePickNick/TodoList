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
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        public AddTask()
        {
            InitializeComponent();
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            var main = App.Current.MainWindow as MainWindow;
            string TaskName = this.TaskName.Text;
            string TaskDescription = this.TaskDescription.Text;
            string Date = this.Time.Text;
            DateTime ConvDate = Convert.ToDateTime(Date);
            DateTime TodayDate = DateTime.Now;
            int NumOfDays = (int)(ConvDate - TodayDate).TotalDays;

            if (Date == "" || TaskName=="")
            {
                MessageBox.Show("Date Field cannot be empty");
            }
            else
            {
                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);
                    connection.Open();
                    
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "Insert into AddTask(TaskOwner, TaskName, TaskDescription,TaskCompleted, TaskDate, TaskDays) Values(@Owner,@Name, @Description,@Completed, @Date, @Days)";
                    command.Parameters.AddWithValue("@Owner", Environment.UserName);
                    command.Parameters.AddWithValue("@Name", TaskName);
                    command.Parameters.AddWithValue("@Description", TaskDescription);
                    command.Parameters.AddWithValue("@Date", Date);
                    command.Parameters.AddWithValue("@Completed", "No");
                    command.Parameters.AddWithValue("@Days", NumOfDays);
                    command.ExecuteNonQuery();
                    main.GridRefresh();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Task Already Exist");
                }
                this.Close();
            }
            
        }
    }
}
