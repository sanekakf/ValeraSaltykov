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
    /// Логика взаимодействия для WindowAddStudent.xaml
    /// </summary>
    public partial class WindowAddStudent : Window
    {
        public WindowAddStudent()
        {
            InitializeComponent();

            Random res = new Random();

            // Случайный логин
            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 6;
            String ran = "";
            for (int i = 0; i < size; i++)
            {
                int x = res.Next(26); // 26 т.к. набор сиволов 27-1
                ran = ran + str[x];
            }


            LoginInput.Text = ran;
        }





        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Reg_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInput.Text;
            string login = LoginInput.Text;
            var password = PasswordInput.Password;
            var checkpass = PasswordRepeat.Password;
            if (name == "" || login == "" || password == "" || checkpass == "")
            {
                MessageBox.Show($"Введите имя, логин и пароль");
                return;
            }
            if (password != checkpass) { MessageBox.Show("Пароли должны совпадаль"); return; }

            string connectionString = "Host=5.42.104.242;Port=5432;Database=default_db;Username=main;password=99mir216";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            await using var connection = await dataSource.OpenConnectionAsync();
            //Проверка на пользователя с таким же логином через Boolean запрос
            try
            {
                string sql1 = $"SELECT EXISTS(SELECT 1 FROM \"users\" WHERE \"login\" = \'{login}\')";
                await using var cmd1 = new NpgsqlCommand(sql1, connection);
                await using var reader = await cmd1.ExecuteReaderAsync();
                //MessageBox.Show(Convert.ToString(reader.HasRows)); //ПРОВЕРКА НА ЛЕГИТНОСТЬ ЗАПРОСА
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var hasLogin = reader.GetBoolean(0);
                        //MessageBox.Show(Convert.ToString(hasLogin)); //ПРОВЕРКА НА TRUE FALSE
                        switch (hasLogin)
                        {
                            case true:
                                MessageBox.Show("Пользователь с таким логином уже существует");
                                return;

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {Ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //Если проходит, то добавляем пользователя как ученика
            
            int role=0;
            try
            {
                switch (IsAdmin.IsChecked)
                {
                    case true:
                        role = 1;
                        break;
                    case false:
                        role = 2;
                        break;
                }
                string sql = "INSERT INTO \"users\" (\"login\", \"fio\", \"roleid\", \"password\") VALUES ($1, $2, $3, $4)";
                await using var cmd = new NpgsqlCommand(sql, connection);

                cmd.Parameters.AddWithValue(NpgsqlDbType.Varchar, login);
                cmd.Parameters.AddWithValue(NpgsqlDbType.Varchar, name);
                cmd.Parameters.AddWithValue(NpgsqlDbType.Integer, role);
                cmd.Parameters.AddWithValue(NpgsqlDbType.Varchar, password);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                //MessageBox.Show($"Добавлено записей: {rowsAffected}");  //ЧЕК ЗАПИСИ
                MessageBox.Show($"Регистрация прошла успешно! Ваш логин: {login}");




            }
            catch (Exception Ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {Ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
