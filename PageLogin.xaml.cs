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
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLoginss : Page
    {
        public PageLoginss()
        {
            InitializeComponent();
        }
    

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginInput.Text;
            var password = PasswordInput.Password;
            if (login=="" || password == ""){
                MessageBox.Show($"Введите логин и пароль");
                return;
            }

            
            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            string sql = $"SELECT \"userid\", \"roleid\" FROM \"users\" WHERE login = \'{login}\' AND password = \'{Convert.ToString(password)}\'";
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(connectionString);
                await using var connection = await dataSource.OpenConnectionAsync();
                await using var cmd = new NpgsqlCommand(sql, connection);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (!reader.HasRows){ MessageBox.Show($"Пользователь не найден"); return; }


                while (await reader.ReadAsync())
                {
                    
                        int id = reader.GetInt32(0);
                        int roleid = reader.GetInt32(1);
                        switch (roleid)
                        {
                            case 1:
                                MessageBox.Show($"Вы вошли как администратор");
                                NavigationService.Navigate(new PageAdmin(id));
                                break;
                            case 2:
                                MessageBox.Show($"Вы вошли как ученик");
                                NavigationService.Navigate(new PageStudent(id));
                                break;
                            default:
                                MessageBox.Show($"Неизвестная роль");
                                break;
                    }
                }
                
              
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {Ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageReg());
        }
    }
}
