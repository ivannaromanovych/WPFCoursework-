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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;

namespace Coursework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(nameUserFile))
            {
                BinaryFormatter BF = new BinaryFormatter();
                using (FileStream FS = new FileStream(nameUserFile, FileMode.Open, FileAccess.Read))
                {
                    user = (User)BF.Deserialize(FS);
                }
                tboxHeight.Text = user.Height.ToString();
                tboxHeight.IsEnabled = false;
                tboxWeight.Text = user.Weight.ToString();
                tboxWeight.IsEnabled = false;
                tboxAge.Text = user.Age.ToString();
                tboxAge.IsEnabled = false;
                tblCalories.Text = user.Calories.ToString();
                if (user.Sex == UserSex.Male)
                    rbSexMale.IsChecked = true;
                else
                    rbSexFemale.IsChecked = true;
                rbSexFemale.IsEnabled = false;
                rbSexMale.IsEnabled = false;
                tboxName.Text = user.Name;
                tboxName.IsEnabled = false;
                tblCalories.Text = user.Calories.ToString();
            }
            else
            {
                Calendar1.IsEnabled = false;
                butAddNewIngestion.IsEnabled = false;
            }
            if (File.Exists(nameProductFile))
            {
                BinaryFormatter BF = new BinaryFormatter();
                using (FileStream FS = new FileStream(nameProductFile, FileMode.Open, FileAccess.Read))
                {
                    products = (ObservableCollection<Product>)BF.Deserialize(FS);
                }
            }
            cbProducts.ItemsSource = products;
            Calendar1.SelectedDate = DateTime.Now.Date;
            if(File.Exists(nameDaysFile))
            {
                BinaryFormatter BF = new BinaryFormatter();
                using (FileStream FS = new FileStream(nameDaysFile, FileMode.Open, FileAccess.Read))
                {
                    days = (List<Day>)BF.Deserialize(FS); 
                }
                for (int i = 0; i < days.Count; i++)
                {
                    if (days[i].curDate.Date == Calendar1.SelectedDate)
                    {
                        curDay = days[i];
                        days.Remove(curDay);
                        break;
                    }
                }
                days.Remove(curDay);
                
            }
            cbNewProductCategory.ItemsSource = new ProductCategory[] { ProductCategory.Proteinous, ProductCategory.Adipose, ProductCategory.Carbohydrate, ProductCategory.Mineral };
            lbBreakfast.ItemsSource = curDay[0];
            lbDinner.ItemsSource = curDay[1];
            lbSupper.ItemsSource = curDay[2];
            tblTakedCalories.Text = curDay.CaloriesPerDay.ToString();
        }
        User user = new User();
        List<Day> days = new List<Day>();
        Day curDay = new Day();
        ObservableCollection<Product> products = new ObservableCollection<Product>();
        string nameUserFile = "User_File.dat";
        string nameProductFile = "Products_File.dat";
        string nameDaysFile = "Days_File.dat";
        string errorString = "You didn't fill field ";
        private void ButSaveClick(object sender, RoutedEventArgs e)
        {
            bool right = true;
            bool ok = true;
            if (rbSexMale.IsChecked == true)
            {
                user.Sex = UserSex.Male;
                rbSexMale.IsEnabled = false;
                rbSexFemale.IsEnabled = false;
            }
            else if (rbSexFemale.IsChecked == true)
            {
                user.Sex = UserSex.Female;
                rbSexMale.IsEnabled = false;
                rbSexFemale.IsEnabled = false;
            }
            else
            {
                MessageBox.Show(errorString + "\"Sex\"");
                ok = false;
            }
            if (String.IsNullOrWhiteSpace(tboxWeight.Text))
            {
                MessageBox.Show(errorString + "\"Weight\"");
                ok = false;
            }
            else
            {
                right = true;
                for (int i = 0; i < tboxWeight.Text.Length; i++)
                    if (!Char.IsDigit(tboxWeight.Text[i]))
                    {
                        right = false;
                        ok = false;
                        break;
                    }
                if (!right)
                    MessageBox.Show(errorString + "\"Weight\" right");
                else if (uint.Parse(tboxWeight.Text) < 30 || uint.Parse(tboxWeight.Text) > 400)
                {
                    MessageBox.Show("You entered impossible weight");
                    ok = false;
                    right = false;
                }
                else
                {
                    user.Weight = uint.Parse(tboxWeight.Text);
                    tboxWeight.IsEnabled = false;
                }
            }
            if (String.IsNullOrWhiteSpace(tboxHeight.Text))
            {
                MessageBox.Show(errorString + "\"Height\"");
                ok = false;
            }
            else
            {
                right = true;
                for (int i = 0; i < tboxHeight.Text.Length; i++)
                    if (!Char.IsDigit(tboxHeight.Text[i]))
                    {
                        right = false;
                        ok = false;
                        break;
                    }
                if (!right)
                    MessageBox.Show(errorString + "\"Height\" right");
                else if (uint.Parse(tboxHeight.Text) < 120 || uint.Parse(tboxHeight.Text) > 272)
                {
                    MessageBox.Show("You entered impossible height");
                    ok = false;
                }
                else
                {
                    user.Height = uint.Parse(tboxHeight.Text);
                    tboxHeight.IsEnabled = false;
                }
            }
            if (String.IsNullOrWhiteSpace(tboxAge.Text))
            {
                MessageBox.Show(errorString + "\"Age\"");
                ok = false;
            }
            else
            {
                right = true;
                for (int i = 0; i < tboxAge.Text.Length; i++)
                    if (!Char.IsDigit(tboxAge.Text[i]))
                    {
                        right = false;
                        ok = false;
                        break;
                    }
                if (!right)
                    MessageBox.Show(errorString + "\"Age\" right");
                else if (uint.Parse(tboxAge.Text) < 12 || uint.Parse(tboxAge.Text) > 120)
                {
                    MessageBox.Show("You entered impossible age");
                    ok = false;
                }
                else
                {
                    user.Age = uint.Parse(tboxAge.Text);
                    tboxAge.IsEnabled = false;
                }
            }
            if (String.IsNullOrWhiteSpace(tboxName.Text))
            {
                MessageBox.Show(errorString + "\"Name\"");
                ok = false;
            }
            else
            {
                user.Name = tboxName.Text;
                tboxName.IsEnabled = false;
            }
            if (ok)
            {
                if (user.Sex == UserSex.Female)
                    user.Calories = (10 * user.Weight + 6.25 * user.Height - 5 * user.Age - 161) * 1.3;
                else
                    user.Calories = (10 * user.Weight + 6.25 * user.Height - 5 * user.Age + 5) * 1.3;
                tblCalories.Text = user.Calories.ToString();
                Calendar1.IsEnabled = true;
                butAddNewIngestion.IsEnabled = true;
                BinaryFormatter BF = new BinaryFormatter();
                using (FileStream FS = new FileStream(nameUserFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    BF.Serialize(FS, user);
                }
            }
        }
        private void ButEditClick(object sender, RoutedEventArgs e)
        {
            rbSexFemale.IsEnabled = true;
            rbSexMale.IsEnabled = true;
            tboxWeight.IsEnabled = true;
            tboxHeight.IsEnabled = true;
            tboxAge.IsEnabled = true;
            tboxName.IsEnabled = true;
        }
        private void ButAddNewProductClick(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product();
            bool ok = true;
            if (String.IsNullOrWhiteSpace(tboxNewProductName.Text))
            {
                MessageBox.Show(errorString + "\"Name\"");
                ok = false;
            }
            else
                newProduct.Name = tboxNewProductName.Text;
            if (cbNewProductCategory.SelectedIndex < 0)
            {
                MessageBox.Show(errorString + "\"Category\"");
                ok = false;
            }
            else
                newProduct.Category = (ProductCategory)cbNewProductCategory.SelectedItem;
            if (String.IsNullOrWhiteSpace(tboxNewProductCalories.Text))
            {
                MessageBox.Show(errorString + "\"Calories\"");
                ok = false;
            }
            else
            {
                bool right = true;
                for (int i = 0; i < tboxNewProductCalories.Text.Length; i++)
                    if (!Char.IsDigit(tboxNewProductCalories.Text[i]))
                    {
                        right = false;
                        ok = false;
                        break;
                    }
                if (!right)
                    MessageBox.Show(errorString + "\"Calories\" right");
                else if (uint.Parse(tboxNewProductCalories.Text) > 2404)
                {
                    MessageBox.Show("You entered impossible calories");
                    ok = false;
                }
                else
                {
                    newProduct.Calories = uint.Parse(tboxNewProductCalories.Text);
                }
            }
            if (ok)
            {
                products.Add((Product)newProduct);
                tboxNewProductName.Text = "";
                tboxNewProductCalories.Text = "";
                cbNewProductCategory.SelectedIndex = -1;
                BinaryFormatter BF = new BinaryFormatter();
                using (FileStream FS = new FileStream(nameProductFile, FileMode.OpenOrCreate))
                {
                    BF.Serialize(FS, products);
                }
            }
        }
        private void ButAddNewIngestionClick(object sender, RoutedEventArgs e)
        {
            bool ok = true;
            Product newProduct = new Product();
            if (cbProducts.SelectedIndex < 0)
            {
                MessageBox.Show(errorString + "\"Product\"");
                ok = false;
            }
            else
                newProduct = products[cbProducts.SelectedIndex];
            if (cbIngestionTime.SelectedIndex < 0)
            {
                MessageBox.Show(errorString + "\"Time\"");
                ok = false;
            }
            if (String.IsNullOrWhiteSpace(tboxProductWeight.Text))
            {
                MessageBox.Show(errorString + "\"Product Weight\"");
                ok = false;
            }
            else
            {
                bool right = true;
                for (int i = 0; i < tboxProductWeight.Text.Length; i++)
                    if (!Char.IsDigit(tboxProductWeight.Text[i]))
                    {
                        right = false;
                        ok = false;
                        break;
                    }
                if (!right)
                    MessageBox.Show(errorString + "\"Product Weight\" right");
                else if (uint.Parse(tboxProductWeight.Text) > 3000)
                {
                    MessageBox.Show("You entered impossible weight");
                    ok = false;
                }
                else
                {
                    newProduct.Weight = uint.Parse(tboxProductWeight.Text);
                    newProduct.Calories = newProduct.Calories * newProduct.Weight / 100;
                }
            }
            if (ok)
            {
                curDay[cbIngestionTime.SelectedIndex].Add(newProduct);
                curDay.CaloriesPerDay += newProduct.Calories;
                tboxProductWeight.Text = "";
                cbProducts.SelectedIndex = -1;
                cbIngestionTime.SelectedIndex = -1;
                tblTakedCalories.Text = curDay.CaloriesPerDay.ToString();
            }
        }
        private void CalendarSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            days.Add(curDay);
            curDay = new Day();
            curDay.curDate = Calendar1.SelectedDate.Value;
            for (int i = 0; i < days.Count; i++)
            {
                if (days[i].curDate.Date == Calendar1.SelectedDate)
                {
                    curDay = days[i];
                    days.Remove(curDay);
                    break;
                }
            }
            lbBreakfast.ItemsSource = curDay[0];
            lbDinner.ItemsSource = curDay[1];
            lbSupper.ItemsSource = curDay[2];
            tblTakedCalories.Text = curDay.CaloriesPerDay.ToString();
        }
        private void ButSaveAllClick(object sender, RoutedEventArgs e)
        {
            days.Add(curDay);
            BinaryFormatter BF = new BinaryFormatter();
            using (FileStream FS = new FileStream(nameDaysFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BF.Serialize(FS, days);
            }
        }
        [Serializable]
        public enum ProductCategory { Proteinous = 1, Adipose, Carbohydrate, Mineral }
        [Serializable]
        public class Product
        {
            public string Name { set; get; }
            public ProductCategory Category { set; get; }
            public double Calories { get; set; }
            public override string ToString()
            {
                return $"{Name} {Category.ToString()} {Calories} cal in {Weight} gr";
            }
            public uint Weight { set; get; }
            public Product()
            {
                Weight = 100;
            }
        }
        [Serializable]
        public enum UserSex { Male = 1, Female }
        [Serializable]
        public class User
        {
            public string Name { set; get; }
            public UserSex Sex { get; set; }
            public uint Age { get; set; }
            public uint Weight { get; set; }
            public uint Height { get; set; }
            public double Calories { set; get; }
        }
        [Serializable]
        public class Day
        {
            public DateTime curDate;
            public ObservableCollection<Product> breakfast;
            public ObservableCollection<Product > dinner;
            public ObservableCollection<Product> supper;
            public double CaloriesPerDay { get; set; }
            public ObservableCollection<Product> this[int first]
            {
                get
                {
                    switch (first)
                    {
                        case 0: return breakfast;
                        case 1: return dinner;
                        case 2: return supper;
                        default: return new ObservableCollection<Product>();
                    }
                }
                set
                {
                    switch (first)
                    {
                        case 1: breakfast = value;
                            break;
                        case 2:
                            dinner = value;
                            break;
                        case 3:
                            supper = value;
                            break;
                    }
                }

            }
            public Day()
            {
                breakfast = new ObservableCollection<Product>();
                dinner = new ObservableCollection<Product>();
                supper = new ObservableCollection<Product>();
                curDate = new DateTime();
                curDate = DateTime.Now;
            }
        }
    }
}