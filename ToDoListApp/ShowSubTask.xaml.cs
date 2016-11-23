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
using System.Configuration;
using System.Data.SQLite;
using System.Data;
namespace ToDoListApp
{
    /// <summary>
    /// Interaction logic for ShowSubTask.xaml
    /// </summary>
    public partial class ShowSubTask : Window
    {
        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        private string showSubTaskDate;
        private string showSubTaskName;

        public ShowSubTask(string showSubTaskName, string showSubTaskDate)
        {
            InitializeComponent();
            this.showSubTaskName = showSubTaskName;
            this.showSubTaskDate = showSubTaskDate;
            this.DataContext = this;
            this.LoginUser = "Sub Task of "+this.showSubTaskName;
            SubTaskGrid();
        }
        public string LoginUser
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Debt", typeof(string), typeof(ShowSubTask), new PropertyMetadata(string.Empty));
        /// <summary>
        /// 
        /// </summary>
        private void SubTaskGrid()
        {


            try
            {
                SQLiteConnection connection = new SQLiteConnection(ConfString);
                SQLiteCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "Select TaskID From AddTask where TaskOwner='" + Environment.UserName + "' And TaskName='" + showSubTaskName + "' And TaskDate='" + showSubTaskDate + "'";
                command.ExecuteNonQuery();
                SQLiteDataReader ReadData = command.ExecuteReader();
                if (ReadData.HasRows)
                {
                    ReadData.Read();
                    String SubID = ReadData["TaskID"].ToString();

                    ReadData.Close();                   
                    string selectQuery = "SELECT * FROM SubAddTask where SubID='" + SubID + "'";
                    SQLiteCommand cmd = new SQLiteCommand(selectQuery, connection);
                    SQLiteDataAdapter Data = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("SubAddTask");
                    Data.Fill(dt);
                    SubDataGrid.ItemsSource = dt.DefaultView;
                    connection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void RemoveSub_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = SubDataGrid.SelectedItem as DataRowView;
            

            if (row != null)
            {
                string SubTaskName = row.Row["SubTaskName"].ToString();
                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "Select SubTaskCompleted From SubAddTask where SubTaskOwner='" + Environment.UserName + "' And SubTaskName='" + SubTaskName + "' AND SubTaskCompleted='No'";


                    SQLiteDataReader ReadData = command.ExecuteReader();
                    if (ReadData.HasRows)
                    {
                        if (MessageBox.Show("Task is not completed do you still want to remove it?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No) { }
                        else
                        {
                            ReadData.Close();

                            command.CommandText = "DELETE FROM SubAddTask where SubTaskName='" + SubTaskName + "'";
                            command.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Task Deleted");
                            SubTaskGrid();

                        }

                    }
                    else
                    {
                        ReadData.Close();

                        command.CommandText = "DELETE FROM SubAddTask where SubTaskName='" + SubTaskName + "'";
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Task Deleted");
                        SubTaskGrid();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else { MessageBox.Show("No Row Has been selected"); }
        }

        private void EditSubTask_Click(object sender, RoutedEventArgs e)
        {
            String EnvName = Environment.UserName;
            DataRowView row = SubDataGrid.SelectedItem as DataRowView;

            if (row != null)
            {
                string ShowSubTaskName = row.Row["SubTaskName"].ToString();
                
                EditSubTask EditTaskWindow = new EditSubTask(ShowSubTaskName);
                EditTaskWindow.Show();
            }
            else { MessageBox.Show("No row has been selected"); }
        }

        private void MarkAsCompleted_Click(object sender, RoutedEventArgs e)
        {
            String EnvName = Environment.UserName;
            DataRowView row = SubDataGrid.SelectedItem as DataRowView;
            
            
            if (row != null)
            {
                string TaskName = row.Row["SubTaskName"].ToString();
                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "Select SubTaskCompleted From SubAddTask where SubTaskOwner='" + Environment.UserName + "' And SubTaskName='" + TaskName + "' AND SubTaskCompleted='Yes'";

                    SQLiteDataReader ReadData = command.ExecuteReader();

                    if (ReadData.HasRows)
                    {
                        MessageBox.Show("Task already has been marked as completed");
                        connection.Close();

                    }
                    else
                    {
                        ReadData.Close();
                        command.CommandText = "Update SubAddTask set SubTaskCompleted ='Yes' where SubTaskOwner='" + Environment.UserName + "' And SubTaskName='" + TaskName + "'";
                        command.ExecuteNonQuery();
                        SubTaskGrid();
                        connection.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                MessageBox.Show("No row has been selected");
            }
        }
    }
}
