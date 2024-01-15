using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RestaurantMenu
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MenuItems = new ObservableCollection<MenuItem>();
            lstMenu.ItemsSource = MenuItems;
        }

        private void AddFood_Click(object sender, RoutedEventArgs e)
        {
            string name = txtFoodName.Text;
            decimal price;
            if (decimal.TryParse(txtFoodPrice.Text, out price))
            {
                string photo = txtFoodPhoto.Text;
                string category = (cmbFilterCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Uncategorized";

                MenuItems.Add(new MenuItem { Name = name, Price = price, Photo = photo, Category = category });
                // Clear the input fields after adding a food item
                txtFoodName.Clear();
                txtFoodPrice.Clear();
                txtFoodPhoto.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a valid price.", "Invalid Price", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                Popup popup = new Popup();
                popup.Child = new Image { Source = image.Source, Width = 300, Height = 300 };
                popup.PlacementTarget = image;

                // Calculate the mouse position relative to the image
                Point mousePosition = e.GetPosition(image);

                // Set the Placement property based on the mouse position
                popup.Placement = (mousePosition.X > image.ActualWidth / 2)
                    ? System.Windows.Controls.Primitives.PlacementMode.Right
                    : System.Windows.Controls.Primitives.PlacementMode.Left;

                popup.IsOpen = true;
                image.Tag = popup; // Store the Popup reference in the Image's Tag
            }
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null && image.Tag is Popup popup)
            {
                popup.IsOpen = false;
            }
        }

        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                txtFoodPhoto.Text = selectedFilePath; // Display the file path in the TextBox
            }
        }
        private void FilterCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = (cmbFilterCategory.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (selectedCategory == "All")
            {
                lstMenu.ItemsSource = MenuItems;
            }
            else
            {
                lstMenu.ItemsSource = MenuItems.Where(item => item.Category == selectedCategory);
            }
        }

    }

    public class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public string Category { get; set; } // Add this property
    }


}
