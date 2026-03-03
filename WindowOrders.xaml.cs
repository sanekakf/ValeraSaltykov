using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Chop_chOp.PageProducts;

namespace Chop_chOp
{
    /// <summary>
    /// Логика взаимодействия для WindowOrders.xaml
    /// </summary>
    public class Order()
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public partial class WindowOrders : Window
    {
        public ObservableCollection<Order> orders = new ObservableCollection<Order> { };
        public int Usid;
        public int roleid;
        public WindowOrders(int userid)
        {
            Usid = userid;
            string connectionString23 = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            using var dataSource23 = NpgsqlDataSource.Create(connectionString23);
            using var connection23 = dataSource23.OpenConnection();
            string sql23 = "SELECT \"roleid\" FROM users WHERE \"userid\" = $1";
            using var cmd23 = new NpgsqlCommand(sql23, connection23);
            cmd23.Parameters.AddWithValue(NpgsqlDbType.Integer, userid);
            using var reader23 = cmd23.ExecuteReader();
            while (reader23.Read()) { roleid = reader23.GetInt32(0); }
            connection23.Close();
            InitializeComponent();
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            string sql = "SELECT o.id, u.fio, p.description FROM orders o INNER JOIN public.users u on u.userid = o.userid INNER JOIN public.products p on p.id = o.productid";
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows) { MessageBox.Show("Заказов не найдено!");return; }
            while (reader.Read()) {
                orders.Add(new Order {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2)
                });
            }
            orderList.ItemsSource = orders;
            connection.Close();
        }

        private async void orderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order o = (Order)orderList.SelectedItem;

            if (roleid != 1) { return; }
            var res = MessageBox.Show("Вы хотите удалить данный заказ?","УДАЛЕНИЕ",MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes) {
                string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
                await using var dataSource = NpgsqlDataSource.Create(connectionString);
                await using var connection = await dataSource.OpenConnectionAsync();
                string sql = "DELETE FROM orders WHERE id = $1";
                await using var cmd1 = new NpgsqlCommand(sql, connection);
                cmd1.Parameters.AddWithValue(NpgsqlDbType.Integer, o.Id);
                int rowsAffected = await cmd1.ExecuteNonQueryAsync();
            }
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            orders.Clear();
            int intid;
            if (id.Text=="") { MessageBox.Show("Укажите айди");return; }
            try { intid = Convert.ToInt32(id.Text); } catch { MessageBox.Show("Укажите только цифры");return;}
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            string sql = "SELECT o.id, u.fio, p.description FROM orders o INNER JOIN public.users u on u.userid = o.userid INNER JOIN public.products p on p.id = o.productid WHERE o.id = $1";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, intid);
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows) { MessageBox.Show("Заказов не найдено!"); return; }
            while (reader.Read())
            {
                orders.Add(new Order
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2)
                });
            }
            orderList.ItemsSource = orders;
        }
    }
}
