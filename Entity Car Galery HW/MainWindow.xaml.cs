using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace Entity_Car_Galery_HW
{

    public class Car
    {
        public int Id { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int StNumber { get; set; }

        public override string ToString() => $"{Id} {Mark} {Model} {Year} {StNumber}";
    }


    public class CarDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public CarDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Database=CarGalery;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True");
        }
    }


    public partial class MainWindow : Window
    {
        CarDbContext CarContext = new CarDbContext();
        public ObservableCollection<Car> cars { get; set; }



        public MainWindow()
        {
            InitializeComponent();
            cars = new ObservableCollection<Car>();
            CarContext.Cars.ToList().ForEach(c => cars.Add(c));
            DataContext = this;
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbMark.Text == string.Empty && TbModel.Text == string.Empty && TbYear.Text == string.Empty && TbStNumber.Text == string.Empty)
            {
                BtnDelete.IsEnabled = false;
                BtnUpdate.IsEnabled = false;
            }
            if (Cars.SelectedItem is not null &&
                ((TbMark.Text != cars[Cars.SelectedIndex].Mark && TbMark.Text != string.Empty) ||
                (TbModel.Text != cars[Cars.SelectedIndex].Model && TbModel.Text != string.Empty) ||
                (TbYear.Text != cars[Cars.SelectedIndex].Year.ToString() && TbYear.Text != string.Empty) ||
                (TbStNumber.Text != cars[Cars.SelectedIndex].StNumber.ToString() && TbStNumber.Text != string.Empty)))
                BtnUpdate.IsEnabled = true;
            else
                BtnUpdate.IsEnabled = false;
        }

        private void Cars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cars.SelectedItem is not null)
            {
                BtnDelete.IsEnabled = true;

                TbMark.Text = cars[Cars.SelectedIndex].Mark;
                TbModel.Text = cars[Cars.SelectedIndex].Model;
                TbYear.Text = cars[Cars.SelectedIndex].Year.ToString();
                TbStNumber.Text = cars[Cars.SelectedIndex].StNumber.ToString();
            }
        }

        private void TbClear()
        {
            TbMark.Clear();
            TbModel.Clear();
            TbYear.Clear();
            TbStNumber.Clear();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (TbMark.Text is not null && TbModel.Text is not null)
            {
                using (CarDbContext database = new())
                {

                    Car car = new()
                    {
                        Model = TbModel.Text,
                        Mark = TbMark.Text,
                        Year = Convert.ToInt32(TbYear.Text),
                        StNumber = Convert.ToInt32(TbStNumber.Text)
                    };
                    database.Cars.Add(car);

                    database.SaveChanges();

                    cars.Clear();

                    CarContext.Cars.ToList().ForEach(c => cars.Add(c));

                    TbClear();

                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Car car = null!;
            using (var database = new CarDbContext())
            {

                car = database.Cars.FirstOrDefault(c => c.Id == cars[Cars.SelectedIndex].Id)!;
                if (car is not null)
                {
                    car.Mark = TbMark.Text;
                    car.Model = TbModel.Text;
                    car.Year = Convert.ToInt32(TbYear.Text);
                    car.StNumber = Convert.ToInt32(TbStNumber.Text);

                    database.Cars.Update(car);

                    cars.Clear();

                    database.Cars.ToList().ForEach(c => cars.Add(c));

                    database.SaveChanges();
                }

                TbClear();

            }
        }



        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (CarDbContext database = new())
            {
                Car car = database.Cars.FirstOrDefault(c => c.Id == cars[Cars.SelectedIndex].Id)!;

                database.Remove(car);
                database.SaveChanges();

                TbClear();

                Cars.SelectedItem = null;

                cars.Clear();

                database.Cars.ToList().ForEach(c => cars.Add(c));
            }
        }
    }
}
