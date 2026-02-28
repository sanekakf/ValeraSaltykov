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
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        int userid; // Вдруг пригодиться
        public PageAdmin(int id)
        {
            InitializeComponent();
            userid = id;
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            string sql = $"SELECT \"fio\" FROM \"users\" WHERE userid = \'{userid}\'";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            var Name = reader.GetString(0);
            hello.Text = $"Приветствуем вас, {Name}. Хорошего рабочего дня!";
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void addStudent_Click(object sender, RoutedEventArgs e)
        {
            WindowAddStudent student = new WindowAddStudent();
            student.ShowDialog();
        }

        private void listStudents_Click(object sender, RoutedEventArgs e)
        {
            WindowListStudents students = new WindowListStudents();
            students.ShowDialog();
        }
    }
}
