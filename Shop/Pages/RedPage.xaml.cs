using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RedPage.xaml
    /// </summary>
    public partial class RedPage : Page
    {
        public static Product constProd;
        public RedPage(Product n)
        {
            InitializeComponent();
            constProd = n;
            this.DataContext = constProd;
            tb_id.Text = n.Id.ToString();
            tb_name.Text = n.Name;
            tb_description.Text = n.Description;

            cb_country.ItemsSource = DBconnection.connection.Country.ToList();
            cb_country.DisplayMemberPath = "Color";

            if (n.UnitId == 1)
            {
                rb_kg.IsChecked = true;
            }
            else if (n.UnitId == 2)
            {
                rb_st.IsChecked = true;
            }
            else if (n.UnitId == 3)
            {
                rb_l.IsChecked = true;
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListPage(ListPage.user));
        }

        public static bool DeleteProduct(Product product)
        {
            product.IsDeleted = true;
            try
            {
                DBconnection.connection.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btn_delite_Click(object sender, RoutedEventArgs e)
        {
            DeleteProduct(constProd);
            MessageBox.Show($"Продукт {constProd.Name} удалён");
            NavigationService.Navigate(new ListPage(ListPage.user));
        }

        private void tb_name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0) && e.Text != "-")
            {
                e.Handled = true;
            }
        }

        private void btn_newphoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = "*.jpg|*.jpg|*.png|*.png"
            };
            if (openFile.ShowDialog().GetValueOrDefault())
            {
                constProd.Photo = File.ReadAllBytes(openFile.FileName);
                img_prod.Source = new BitmapImage(new Uri(openFile.FileName));
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            constProd.Name = tb_name.Text;
            constProd.Description = tb_description.Text;
            constProd.AddDate = DateTime.Now;
            if (rb_kg.IsChecked == true)
            {
                constProd.UnitId = 1;
            }
            else if (rb_l.IsChecked == true)
            {
                constProd.UnitId = 3;
            }
            else
            {
                constProd.UnitId = 2;
            }
            DBconnection.connection.SaveChanges();
            NavigationService.Navigate(new ListPage(ListPage.user));
        }

        private void btn_add_country_Click(object sender, RoutedEventArgs e)
        {
            if (cb_country.SelectedIndex >= 0)
            {
                var countryProd = new ProductCountry();
                var selectCountry = cb_country.SelectedItem as Country;
                countryProd.ProductId = constProd.Id;
                countryProd.CountryId = selectCountry.Id;
                var isCountry = DBconnection.connection.ProductCountry.Where(a => a.CountryId == selectCountry.Id && a.ProductId == constProd.Id).Count();
                if (isCountry == 0)
                {
                    DBconnection.connection.ProductCountry.Add(countryProd);
                    DBconnection.connection.SaveChanges();
                    UpdateCountry();
                }
            }
        }

        public void UpdateCountry()
        {
            lv_country.ItemsSource = DBconnection.connection.ProductCountry.Where(a => a.ProductId == constProd.Id).ToList();
        }

        private void btn_del_country_Click(object sender, RoutedEventArgs e)
        {
            if (lv_country.SelectedItem != null)
            {
                var selectProdCountry = DBconnection.connection.ProductCountry.ToList().Find(a => a.ProductId == constProd.Id && a.CountryId == (lv_country.SelectedItem as ProductCountry).CountryId);
                DBconnection.connection.ProductCountry.Remove(selectProdCountry);
                DBconnection.connection.SaveChanges();
                UpdateCountry();
            }
        }
    }
}
