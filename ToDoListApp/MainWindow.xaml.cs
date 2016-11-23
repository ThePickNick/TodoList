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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using ToDoListApp;

namespace WpfApplication1

{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private string ConfString = ConfigurationManager.ConnectionStrings["DBSQLite"].ConnectionString;
        public string LoginUser
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }


        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Debt", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public MainWindow()
        {


            InitializeComponent();
            FillDataGrid();
            this.DataContext = this;
            this.LoginUser = "Welcome " + Environment.UserName;        
            //this.Content = textBlock;

        }

        private void Mark_as_Done(object sender, RoutedEventArgs e)
        {
          
            
                DataRowView row = AddTaskData.SelectedItem as DataRowView;
                Mark_As_Done Done = new Mark_As_Done();
            
            if (row != null)
                {
                    Done.Taskdate = row.Row["TaskDate"].ToString();
                    Done.Datetask =  row.Row["TaskName"].ToString();
                    Done.MarkAsDone();
                    FillDataGrid();
            }
                else {
                    MessageBox.Show("No row has been selected");
                }                                                                         

        }

        public void GridRefresh(){
            FillDataGrid();
            }
        private void FillDataGrid()
        {

            try
            {
                SQLiteConnection connection = new SQLiteConnection(ConfString);
                string selectQuery = "SELECT * FROM AddTask where TaskOwner='" + Environment.UserName + "'";
                SQLiteCommand cmd = new SQLiteCommand(selectQuery, connection);
                SQLiteDataAdapter Data = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable("AddTask");
                Data.Fill(dt);
                AddTaskData.ItemsSource = dt.DefaultView;
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //selecting Row from the Grrid View
            
            DataRowView row = AddTaskData.SelectedItem as DataRowView;
            if (row != null)
            {
                string TaskName = row.Row["TaskName"].ToString();
                string TaskDate = row.Row["TaskDate"].ToString();

                try
                {
                    SQLiteConnection connection = new SQLiteConnection(ConfString);
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "Select TaskCompleted From AddTask where TaskOwner='" + Environment.UserName + "' And TaskName='" + TaskName + "' And TaskDate='" + TaskDate + "' AND TaskCompleted='No'";


                    SQLiteDataReader ReadData = command.ExecuteReader();
                    if (ReadData.HasRows)
                    {
                        if (MessageBox.Show("Task is not completed do you still want to remove it?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No){}
                        else
                        {
                            ReadData.Close();
                            
                            command.CommandText = "DELETE FROM AddTask where TaskName='" + TaskName + "' And TaskDate='" + TaskDate + "'";
                            command.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Task Deleted");
                            FillDataGrid();
                        }

                    }
                    else
                    {
                        ReadData.Close();
                       
                        command.CommandText = "DELETE FROM AddTask where TaskName='" + TaskName + "' And TaskDate='" + TaskDate + "'";
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Task Deleted");
                        FillDataGrid();
                    }
                }
                catch(Exception ex){ MessageBox.Show(ex.Message); }
            }
            else {
                MessageBox.Show("No row is selected");
            }                           
        }

        private void Add_Task_Click(object sender, RoutedEventArgs e)
        {
            AddTask AddTaskWindow = new AddTask();
            AddTaskWindow.Show();
        }

        private void refresh_click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();

        }

        private void Edit_Task_Click(object sender, RoutedEventArgs e)
        {
            
            String EnvName = Environment.UserName;
            DataRowView row = AddTaskData.SelectedItem as DataRowView;
           
            if (row != null)
            {
                string EditTaskName = row.Row["TaskName"].ToString();
                string EditTaskDate = row.Row["TaskDate"].ToString();
                EditTask EditTaskWindow = new EditTask(EditTaskName, EditTaskDate);
                EditTaskWindow.Show();
            }
            else { MessageBox.Show("No row has been selected"); }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string SearchName = this.searchbox.Text;

            try
            {
                SQLiteConnection connection = new SQLiteConnection(ConfString);
                connection.Open();
                string sql = "SELECT * FROM AddTask where TaskOwner='" + Environment.UserName + "' AND TaskName=@Name";
                SQLiteCommand command = new SQLiteCommand(sql, connection);      
                command.Parameters.AddWithValue("@Name", SearchName);
                //command.ExecuteNonQuery();
                SQLiteDataAdapter Data = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable("AddTask");
                Data.Fill(dt);
                AddTaskData.ItemsSource = dt.DefaultView;

            }catch(Exception ex){ MessageBox.Show(ex.Message); }
        }

        private void SubTask_Click(object sender, RoutedEventArgs e)
        {
            String EnvName = Environment.UserName;
            DataRowView row = AddTaskData.SelectedItem as DataRowView;

            if (row != null)
            {
                string SubTaskName = row.Row["TaskName"].ToString();
                string SubTaskDate = row.Row["TaskDate"].ToString();
                SubTask EditTaskWindow = new SubTask(SubTaskName, SubTaskDate);
                EditTaskWindow.Show();
            }
            else { MessageBox.Show("No row has been selected"); }
        }

        private void ShowSubTasks_Click(object sender, RoutedEventArgs e)
        {
            String EnvName = Environment.UserName;
            DataRowView row = AddTaskData.SelectedItem as DataRowView;

            if (row != null)
            {
                string ShowSubTaskName = row.Row["TaskName"].ToString();
                string ShowSubTaskDate = row.Row["TaskDate"].ToString();
                ShowSubTask EditTaskWindow = new ShowSubTask(ShowSubTaskName, ShowSubTaskDate);
                EditTaskWindow.Show();
            }
            else { MessageBox.Show("No row has been selected"); }
        }

    }
}
    










