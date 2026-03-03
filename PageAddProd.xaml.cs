using Microsoft.Win32;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace Chop_chOp
{
    /// <summary>
    /// Логика взаимодействия для PageAddProd.xaml
    /// </summary>
    
    public class ProdCategories
    {
        public int id { get; set; } = 0;
        public string category { get; set; } = "";
        public override string ToString() => $"{category}";
    }
    public class ProdTypes
    {
        public int id { get; set; } = 0;
        public string type { get; set; } = "";
        public override string ToString() => $"{type}";

    }
    public class Manufacturers
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public override string ToString() => $"{name}";

    }
    public class Suppliers
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public override string ToString() => $"{name}";

    }

    public partial class PageAddProd : Page
    {
        public BitmapImage bi = null; // Global
        byte[]? data = null; // Changed from 'byte' to 'byte[]'

        
        public PageAddProd()
        {
            
            //List
            List<ProdCategories> pCatList= [];
            List<ProdTypes> pTypeList= [];
            List<Manufacturers> mList= [];
            List<Suppliers> sList= [];
            

            InitializeComponent();
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var connection = dataSource.OpenConnection();
            
            // PRODUCT CATEGORY
            string sql = "SELECT * FROM prodcategory";
            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) 
            {
                int id = reader.GetInt32(0);
                string category = reader.GetString(1);
                pCatList.Add(new ProdCategories { id=id, category=category });

            }
            connection.Close();

            // PRODUCT TYPE
            using var connection1 = dataSource.OpenConnection();
            string sql1 = "SELECT * FROM prodtype";
            using var cmd1 = new NpgsqlCommand(sql1, connection1);
            using var reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                int id = reader1.GetInt32(0);
                string type = reader1.GetString(1);
                pTypeList.Add(new ProdTypes { id=id, type=type});

            }
            connection1.Close();

            // MANUFACTURERS
            using var connection2 = dataSource.OpenConnection();
            string sql2 = "SELECT * FROM manufacturer";
            using var cmd2 = new NpgsqlCommand(sql2, connection2);
            using var reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                int id = reader2.GetInt32(0);
                string name = reader2.GetString(1);
                mList.Add(new Manufacturers { id = id, name = name });

            }
            connection2.Close();

            // SUPPLIERS
            using var connection3 = dataSource.OpenConnection();
            string sql3 = "SELECT * FROM supplier";
            using var cmd3 = new NpgsqlCommand(sql3, connection3);
            using var reader3 = cmd3.ExecuteReader();
            while (reader3.Read())
            {
                int id = reader3.GetInt32(0);
                string name = reader3.GetString(1);
                sList.Add(new Suppliers { id = id, name = name });

            }
            connection3.Close();

            //Тесты
            //pCatList.Add(new ProdCategories { id = 1,category= "bobik"});
            //sList.Add(new Suppliers { id=123, name = "второйбобик" });

            ProdCategory.ItemsSource = pCatList.ToArray();
            ProdType.ItemsSource = pTypeList.ToArray();
            Manufacturer.ItemsSource = mList.ToArray();
            Supplier.ItemsSource = sList.ToArray();
        }

        private async void addPicture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Open Picture";
            open.Multiselect = false;
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            if (open.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(open.FileName, UriKind.RelativeOrAbsolute);
                    // Recommended: Use OnLoad to ensure the file is fully read into memory
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    addPicture.Source = bi;
                    
                }
                catch (System.Exception c) { Console.Write("Exception" + c); }
                if(addPicture.Source != null) { help.Visibility = Visibility.Collapsed; }
            }
        }

        private async void SEND_Click(object sender, RoutedEventArgs e)
        {
            
            if (addPicture.Source == null) { MessageBox.Show("ДОБАВЬТЕ КАРТИНКУ!"); return; };
            
            
            string name = Name.Text;
            string price = Price.Text;
            string rem = Remain.Text;
            int newprice, newrem = 0;
            var productCategory = (ProdCategories)ProdCategory.SelectedItem;
            var productType = (ProdTypes)ProdType.SelectedItem;
            var manufacturer = (Manufacturers)Manufacturer.SelectedItem;
            var supplier = (Suppliers)Supplier.SelectedItem;
            
            if (
                name == "" || 
                price == "" || 
                ProdCategory.SelectedValue == null ||
                ProdType.SelectedValue == null ||
                Manufacturer.SelectedValue == null ||
                Supplier.SelectedValue == null
            ) 
            {
                MessageBox.Show("ЗАПОЛНИТЕ ВСЕ ПОЛЯ!");
                return;
            }
            try {
                newprice = Convert.ToInt32(price);
                newrem = Convert.ToInt32(rem);
            }
            catch 
            {
                MessageBox.Show("Цена и остатки должны быть указаны только цифрами!");
                return;
            }

            var res = MessageBox.Show("СОЗДАЕМ НОВЫЙ ТОВАР \n" +
                $"Имя - {name}\n" +
                $"Цена - {newprice}\n" +
                $"Категория товара - {productCategory.category}\n" +
                $"Тип товара - {productType.type}\n" +
                $"Производитель - {manufacturer.name}\n" +
                $"Поставщик - {supplier.name}",
                "Вопрос",
                MessageBoxButton.YesNo
                );
            if (res == MessageBoxResult.Yes)
            {


                AddProduct(name, newprice, newrem, productCategory.id, productType.id, manufacturer.id, supplier.id, data);
                
            }
        }

        private async void AddProduct(string name, int newprice, int newrem, int idPCat, int idPType, int idM, int idS, byte[] data)
        {
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            await using var connection = await dataSource.OpenConnectionAsync();
            string sql = "INSERT INTO \"products\" (\"prodtype\", \"price\", \"supplier\", \"manufacturer\", \"prodcategory\", \"remains\", \"description\", \"picture\") VALUES ($1, $2, $3, $4, $5, $6, $7, $8)";
            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, idPType);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, newprice);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, idS);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, idM);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, idPCat);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, newrem);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Varchar, name);
            cmd.Parameters.AddWithValue(NpgsqlDbType.Bytea, data);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
        }
    }
}
