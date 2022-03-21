using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Fedyaev_Petrov_Namordik.EF;
using static Fedyaev_Petrov_Namordik.EF.AppData;

namespace Fedyaev_Petrov_Namordik.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private List<string> listSort = new List<string>() // Формарование листа для сортировки
        {
            "Наименование (по возрастанию)",
            "Наименование (по убыванию)",
            "Номер производственного цеха (по возрастанию)",
            "Номер производственного цеха (по убыванию)",
            "Стоимость (по возрастанию)",
            "Стоимость (по убыванию)"
        };


        List<ProductType> typeProduct = new List<ProductType>(); // список типов продуктов

        List<Product> product = new List<Product>(); // список продуктов

        List<VM_ProductList> listProduct = new List<VM_ProductList>(); // список для выгрузки на окно

        List<VM_ProductList> selectProduct = new List<VM_ProductList>(); // список для выбранных продуктов


        int numberPage = 0;
        int countProduct;


        private void Filter() // Поиск, фильтрация, сортировка
        {
            listProduct = Context.VM_ProductList.ToList(); // получиния всех материалов из БД

            // Поиск
            listProduct = listProduct.
                Where(i => i.NameProduct.ToLower().Contains(txtSearch.Text.ToLower())).
                ToList();

            // Сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listProduct = listProduct.OrderBy(i => i.NameProduct).ToList(); // сортировка по возрастанию
                    break;

                case 1:
                    listProduct = listProduct.OrderByDescending(i => i.NameProduct).ToList(); // сортировка по убыванию
                    break;

                case 2:
                    listProduct = listProduct.OrderBy(i => i.NumberWorkshop).ToList();
                    break;

                case 3:
                    listProduct = listProduct.OrderByDescending(i => i.NumberWorkshop).ToList();
                    break;

                case 4:
                    listProduct = listProduct.OrderBy(i => i.Cost).ToList();
                    break;

                case 5:
                    listProduct = listProduct.OrderByDescending(i => i.Cost).ToList();
                    break;

                default:
                    listProduct = listProduct.OrderBy(i => i.NameProduct).ToList();
                    break;
            }


            // Фильтрация
            if (cmbFilter.SelectedIndex != 0)
            {
                product = product.Where(i => i.ProductTypeID == cmbFilter.SelectedIndex).ToList();
            }

            countProduct = listProduct.Count;
            // Постраничный вывод

            listProduct = listProduct.
                Skip(numberPage * 20).
                Take(20).
                ToList();

            // Создание кнопок

            if (Convert.ToInt32(btn2.Content) > ((countProduct / 20) + 1))
            {
                btn2.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn2.Visibility = Visibility.Visible;
            }

            if (Convert.ToInt32(btn3.Content) > ((countProduct / 20) + 1))
            {
                btn3.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn3.Visibility = Visibility.Visible;
            }

            lvListProduct.ItemsSource = listProduct;

        }

        public ProductWindow()
        {
            InitializeComponent();

            cmbSort.ItemsSource = listSort; // заполнеие ComboBox для сортировки
            cmbSort.SelectedIndex = 0;

            typeProduct = Context.ProductType.ToList();

            typeProduct.Insert(0, new ProductType { Title = "Все типы" }); // добавление в список элемента "Все типы"
            cmbFilter.ItemsSource = typeProduct; // заполнеие ComboBox для фильтрации
            cmbFilter.DisplayMemberPath = "Name";
            cmbFilter.SelectedIndex = 0;

            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void lvListProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListProduct.SelectedIndex != -1) // если выбран элемент то показать кнопку
            {
                btnEdit.Visibility = Visibility.Visible;
            }
            else
            {
                btnEdit.Visibility = Visibility.Collapsed; // если НЕ выбран элемент то спрятать кнопку
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (numberPage > 0)
            {
                numberPage--;
                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
                Filter();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((countProduct / 20) > numberPage && countProduct != 0)
            {
                numberPage++;

                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();

                Filter();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            foreach (var product in lvListProduct.SelectedItems)
            {
                selectProduct.Add(product as VM_ProductList);
            }

            this.Opacity = 0.4;

            AddEditProductListWindow editWindow = new AddEditProductListWindow();
            editWindow.ShowDialog();

            this.Opacity = 1;

            Filter();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.4;

            AddEditProductListWindow addWindow = new AddEditProductListWindow();
            addWindow.ShowDialog();

            this.Opacity = 1;
        }
    }
}
