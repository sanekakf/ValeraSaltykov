using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chop_chOp
{
    /// <summary>
    /// Логика взаимодействия для PageStudent.xaml
    /// </summary>
    public partial class PageStudent : Page
    {
        public PageStudent(int id)
        {
            InitializeComponent();
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            string sql = $"SELECT fio, login FROM \"users\" WHERE userid = \'{id}\'";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var name = reader.GetString(0);
            var login = reader.GetString(1);
            Name.Text = $"Имя пользователя: {name}";
            Login.Text = $"Логин: {login}";
        }
    }
}
