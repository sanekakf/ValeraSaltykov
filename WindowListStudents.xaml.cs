using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chop_chOp
{
    /// <summary>
    /// Логика взаимодействия для WindowListStudents.xaml
    /// </summary>
    public partial class WindowListStudents : Window
    {
        private async void GetStudents() {
        
        }

        public WindowListStudents()
        {
            InitializeComponent();
            DataTable students = new DataTable();
            students.Columns.Add("Login", typeof(string));
            students.Columns.Add("Name", typeof(string));
            students.Columns.Add("Role",typeof(string));
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            string sql = "SELECT u.login, u.fio, r.role, u.userid " +
                "FROM \"users\" as u " +
                "JOIN \"roles\" AS r " +
                "ON r.roleid = u.roleid " +
                "ORDER BY u.userid";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                students.Rows.Add(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2)
                    );
            }
            studentsTable.ItemsSource = students.DefaultView;
        }
    }
}
