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
using System.Windows.Shapes;

namespace Chop_chOp
{
    /// <summary>
    /// Логика взаимодействия для WindowAddProd.xaml
    /// </summary>
    public partial class WindowAddProd : Window
    {
        public WindowAddProd()
        {
            InitializeComponent();
        }
        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
