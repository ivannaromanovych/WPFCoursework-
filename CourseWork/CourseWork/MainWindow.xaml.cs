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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            }
        }
        User user = new User();
        string nameUserFile = "User_File.dat";
        private void butSaveClick(object sender, RoutedEventArgs e)
        {
            string errorString = "You didn't fill field";
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
            if (String.IsNullOrWhiteSpace(tboxWeight.Text))
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
                MessageBox.Show(errorString + "\"Age\"");
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
                BinaryFormatter BF = new BinaryFormatter();
                using (FileStream FS = new FileStream(nameUserFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    BF.Serialize(FS, user);
                    FS.Seek(0, SeekOrigin.Begin);
                }
            }
        }
        private void butEditClick(object sender, RoutedEventArgs e)
        {
            rbSexFemale.IsEnabled = true;
            rbSexMale.IsEnabled = true;
            tboxWeight.IsEnabled = true;
            tboxHeight.IsEnabled = true;
            tboxAge.IsEnabled = true;
            tboxName.IsEnabled = true;
        }

        private void TboxAge_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    [Serializable]
    public enum ProductCategory { Proteinous = 1, Adipose, Carbohydrate, Mineral }
    [Serializable]
    public class Product
    {
        public string Name { set; get; }
        public ProductCategory Category { set; get; }
        public double Calories
        {
            set
            {
                if (value < 0)
                    Calories = 0;
                else
                    Calories = value;
            }
            get
            {
                return Calories;
            }
        }
        public override string ToString()
        {
            return $"{Name} {Category.ToString()} {Calories} cal in 100 gr";
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
    public enum TakeFood { Breakfast = 1, Dinned, Supper }
    [Serializable]
    public class Day
    {
        ObservableCollection<Product> breakfast;
        ObservableCollection<Product> dinner;
        ObservableCollection<Product> supper;
    }
}
