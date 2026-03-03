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
    /// Логика взаимодействия для PageManager.xaml
    /// </summary>
    public partial class PageManager : Page
    {
        public int userid;
        public PageManager(int id)
        {
            InitializeComponent();
            userid = id;
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            var productPage = new PageProducts(userid); // создаём экземпляр страницы
            var window = new WindowProd();
            window.NavigateToPage(productPage); // помещаем страницу в окно
            window.Show();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            var orderWin = new WindowOrders(userid);
            orderWin.Show();
        }
    }
}
