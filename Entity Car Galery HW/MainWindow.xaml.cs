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



    class Cars
    {
        public int Id { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? StNumber { get; set; }
    }

    class CarsContext : DbContext
    {
        public DbSet<Cars> car { get; set; }

        public CarsContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Database=CarGalery;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True");
        }
    }

    public partial class MainWindow : Window
    {

        CarsContext CarContext = new CarsContext();
        public ObservableCollection<Cars> Cars { get; set; }



        public MainWindow()
        {
            InitializeComponent();
            Cars = new ObservableCollection<Cars>();
            CarsContext.Cars.ToList().ForEach(c => Cars.Add(c));
            DataContext = this;
        }

        private void TbClear()
        {
            CarMake.Clear();
            CarModel.Clear();
            CarYear.Clear();
            CarStNumber.Clear();
        }

        private void Cars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CarMake_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CarModel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CarYear_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CarStNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Add_Button(object sender, RoutedEventArgs e)
        {
            using(var db = new CarsContext())
            {
                List<Cars> cars = new()
                {
                    new Cars
                    {

                        Make = CarMake.Text,
                        Model = CarModel.Text,
                        Year = CarYear.Text,
                        StNumber = CarStNumber.Text
                    }
                    
                };
                db.AddRange(cars);
                db.SaveChanges();
            }

            if (CarMake.Text is not null && CarModel.Text is not null)
            {
                using (CarsContext database = new())
                {


                    Cars car = new()
                    {
                        Model = CarModel.Text,
                        Mark = CarMake.Text,
                        Year = CarYear,
                        StNumber = CarStNumber
                    };
                    database.Cars.Add(car);

                    database.SaveChanges();

                    Cars.Clear();

                    CarsContext.Cars.ToList().ForEach(c => cars.Add(c));

                    TbClear();


                }
            }

        }

        private void Update_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Button(object sender, RoutedEventArgs e)
        {

        }
    }
}
