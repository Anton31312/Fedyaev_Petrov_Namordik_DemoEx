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
using System.Windows.Shapes;
using Fedyaev_Petrov_Namordik.EF;
using Microsoft.Win32;

namespace Fedyaev_Petrov_Namordik.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductListWindow.xaml
    /// </summary>
    public partial class AddEditProductListWindow : Window
    {

        EF.VM_ProductList editProduct = new EF.VM_ProductList();
        bool isEdit = true; // изменяем или добавляем продукт
        string pathPhoto = null; // Для сохранения пути к изображению

        public AddEditProductListWindow()
        {
            InitializeComponent();

            cmbTypeProduct.ItemsSource = AppData.Context.ProductType.ToList();
            cmbTypeProduct.DisplayMemberPath = "Title";
            cmbTypeProduct.SelectedIndex = 0;

            isEdit = false;
        }

        public AddEditProductListWindow(EF.VM_ProductList product)
        {
            InitializeComponent();

            //edit combobox
            cmbTypeProduct.ItemsSource = AppData.Context.Product.ToList();
            cmbTypeProduct.DisplayMemberPath = "Title";

            //edit TItle and content button
            tbTitle.Text = "Изменить карточку товара";
            btnAdd.Content = "Изменить";

            //Get value
            editProduct = product;

            txtTitle.Text = editProduct.NameProduct;
            txtProductionPersonCount.Text = editProduct.ProductionPersonCount.ToString();
            txtNumberWorkshop.Text = editProduct.ProductionWorkshopNumber.ToString();
            txtMinCostForAgent.Text = editProduct.Cost.ToString();
            txtDescription.Text = editProduct.Description;
            cmbTypeProduct.SelectedIndex = editProduct.ProductTypeID - 1;

            isEdit = true;
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imgProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                pathPhoto = openFileDialog.FileName;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            #region Validation
            //Check is null
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Поле Название товара не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArticul.Text))
            {
                MessageBox.Show("Поле Артикул не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMinCostForAgent.Text))
            {
                MessageBox.Show("Поле Минимальная стоимость для агента не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Check lenght

            if (txtTitle.Text.Length > 100)
            {
                MessageBox.Show("В поле Название товара недопустимое количество символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion

            
            if (isEdit)
            {
                try //Проверка на ошибки в БД
                {
                    //изменение продукта
                    editProduct.NameProduct = txtTitle.Text;
                    editProduct.Articul = txtArticul.Text;
                    editProduct.Description = txtDescription.Text;
                    editProduct.Image = pathPhoto;
                    editProduct.NumberWorkshop = Convert.ToInt32(txtNumberWorkshop.Text);
                    editProduct.ProductionPersonCount = Convert.ToInt32(txtProductionPersonCount.Text);
                    editProduct.ProductTypeID = cmbTypeProduct.SelectedIndex + 1;

                    AppData.Context.SaveChanges();
                    MessageBox.Show("Данные о продукте успешно изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                try
                {
                    var resultClick = MessageBox.Show("Вы уверены?", "Подтвердите добавление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultClick == MessageBoxResult.Yes)
                    {
                        //Добавление нового продукта
                        EF.Product product = new EF.Product();
                        product.Title = txtTitle.Text;
                        product.ArticleNumber = txtArticul.Text;
                        product.Description = txtDescription.Text;
                        product.Image = pathPhoto;
                        product.ProductionWorkshopNumber = Convert.ToInt32(txtNumberWorkshop.Text);
                        product.ProductTypeID = cmbTypeProduct.SelectedIndex + 1;

                        AppData.Context.Product.Add(product);
                        AppData.Context.SaveChanges();
                        MessageBox.Show("Продукт успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}

