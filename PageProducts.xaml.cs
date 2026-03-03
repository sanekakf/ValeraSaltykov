using Npgsql;
using NpgsqlTypes;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
namespace Chop_chOp
{
    /// <summary>
    /// Логика взаимодействия для PageProducts.xaml
    /// </summary>

    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                using (var stream = new MemoryStream(byteArray))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class PageProducts : Page
    {
        public int userid;
        public int roleid;

        public class Product
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public int Price { get; set; }
            public string Supplier { get; set; }
            public string Manufacturer { get; set; }
            public string Category { get; set; }
            public int Remains { get; set; }
            public string Description { get; set; }
            public byte[] img { get; set; }
            
        }
        public PageProducts(int userid)
        {
            this.userid = userid;
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
            if (roleid != 1) { addProd.Visibility = Visibility.Collapsed; }
            var products = new ObservableCollection<Product>{
                //new Product
                //{
                //    Id = 21,
                //    Type = "asdas",
                //    Price = 0,
                //    Supplier = "Microsoft",
                //    Manufacturer = "Microsoft",
                //    Category = "Microsoft",
                //    Remains = 0,
                //    Description = "Microsoft"
                //}
            };
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            string sql = "SELECT p.id, p2.type, p.price, s.name, m.name, p3.category, p.remains, p.description,  p.picture FROM \"products\" p INNER JOIN public.supplier s on s.id = p.supplier INNER JOIN public.manufacturer m on m.id = p.manufacturer INNER JOIN public.prodtype p2 on p2.id = p.prodtype INNER JOIN public.prodcategory p3 on p3.id = p.prodcategory";
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            //if (reader.HasRows) { MessageBox.Show("True"); }
            while (reader.Read())
            {
                products.Add(new Product {
                    Id=reader.GetInt32(0),
                    Type=reader.GetString(1),
                    Price=reader.GetInt32(2),
                    Supplier=reader.GetString(3),
                    Manufacturer=reader.GetString(4),
                    Category=reader.GetString(5),
                    Remains=reader.GetInt32(6),
                    Description=reader.GetString(7),
                    img =  reader.GetFieldValue<byte[]>(8)
                    
                });
            }


            
            
            productsList.ItemsSource = products;
        }

        

        private async void productsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product p = (Product)productsList.SelectedItem;
            
             
            switch (roleid)
            {
                case 1:
                    var w1 = new WindowAdminProdDumat(p.Id);
                    w1.ShowDialog();
                    break;
                default:
                    try
                    {
                        string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
                        await using var dataSource = NpgsqlDataSource.Create(connectionString);
                        await using var connection1 = await dataSource.OpenConnectionAsync();
                        string sql1 = "INSERT INTO \"orders\"(\"userid\", \"productid\") VALUES ($1, $2)";
                        await using var cmd1 = new NpgsqlCommand(sql1, connection1);
                        cmd1.Parameters.AddWithValue(NpgsqlDbType.Integer, userid);
                        cmd1.Parameters.AddWithValue(NpgsqlDbType.Integer, p.Id);
                        int rowsAffected = await cmd1.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка - {ex}");
                        return;
                    }
                    MessageBox.Show($"Заказ создан!");
                    break;
            }
            
        }

        private void addProd_Click(object sender, RoutedEventArgs e)
        {
            var productPage = new PageAddProd(); // создаём экземпляр страницы
            var window = new WindowAddProd();
            window.NavigateToPage(productPage); // помещаем страницу в окно
            window.Show();
        }
    }
}
