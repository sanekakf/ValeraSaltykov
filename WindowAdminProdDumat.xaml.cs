using Npgsql;
using NpgsqlTypes;
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
    /// Логика взаимодействия для WindowAdminProdDumat.xaml
    /// </summary>
    public partial class WindowAdminProdDumat : Window
    {
        public int ProdId;
        public WindowAdminProdDumat(int prodid)
        {
            ProdId = prodid;
            InitializeComponent();
        }

        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
                await using var dataSource = NpgsqlDataSource.Create(connectionString);
                await using var connection = await dataSource.OpenConnectionAsync();
                string sql = "DELETE FROM products WHERE id = $1";
                await using var cmd1 = new NpgsqlCommand(sql, connection);
                cmd1.Parameters.AddWithValue(NpgsqlDbType.Integer, ProdId);
                int rowsAffected = await cmd1.ExecuteNonQueryAsync();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("СУЩЕСТВУЮТ АКТИВНЫЕ ЗАКАЗЫ С ДАННЫМ ЗАКАЗОМ!");
                return;
            }
        }

        
    }
}
