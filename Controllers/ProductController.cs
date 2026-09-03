using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StockAlarmSystem.Models;
using StockAlarmLibrary;
namespace StockAlarmSystem.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            List<Product> products = new List<Product>();//veri tabanından gelen veriler burada tutulur 

            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);//sql ile bağlantı nesnesi 

            connection.Open();

            string query = "SELECT * FROM Products";

            SqlCommand command = new SqlCommand(query, connection);//çalışmaya hazır hale getiriliyor 

            SqlDataReader reader = command.ExecuteReader();//select sorgusuunu satır satır okuyor 

            while (reader.Read())//veri varsa okumaya devam et 
            {
                Product product = new Product();

                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString();
                product.Stock = Convert.ToInt32(reader["Stock"]);
                product.MinStock = Convert.ToInt32(reader["MinStock"]);
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.IsCritical = AlertHelper.IsCriticalStock(  product.Stock, product.MinStock);
                products.Add(product);
            }

            connection.Close();//kapatıyor ve viewe gönderiyor 

            return View(products);
        }
        public IActionResult AddProduct()//boş ğrğn ekleme ekranı açılıyor 
        {
            return View();
        }

        [HttpPost]//form gönderildiğinde çalış 
        public IActionResult AddProduct(Product product)//ekele için yaptık 
        {
            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string query = "INSERT INTO Products(ProductName, Stock, MinStock, Price, CategoryId, SupplierId) VALUES (@ProductName, @Stock, @MinStock, @Price, 1, 1)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@Stock", product.Stock);
            command.Parameters.AddWithValue("@MinStock", product.MinStock);
            command.Parameters.AddWithValue("@Price", product.Price);

            command.ExecuteNonQuery();//sorgular çalışıyor ınsert update gibi

            connection.Close();

            return RedirectToAction("Index");//güncellenmiş halini görmesi için
        }
        public IActionResult DeleteProduct(int id)//silmek icin 
        {
            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string query = "DELETE FROM Products WHERE ProductId=@id";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToAction("Index");
        }
        public IActionResult UpdateProduct(int id)//güncelecek ürünü veritabanından çekip ekrana getirmek
        {
            Product product = new Product();

            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string query = "SELECT * FROM Products WHERE ProductId=@id";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString();
                product.Stock = Convert.ToInt32(reader["Stock"]);
                product.MinStock = Convert.ToInt32(reader["MinStock"]);
                product.Price = Convert.ToDecimal(reader["Price"]);
            }

            connection.Close();

            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)//değişen ürünü veritabanına kaydetmek 
        {
            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string query = "UPDATE Products SET ProductName=@ProductName, Stock=@Stock, MinStock=@MinStock, Price=@Price WHERE ProductId=@ProductId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProductId", product.ProductId);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@Stock", product.Stock);
            command.Parameters.AddWithValue("@MinStock", product.MinStock);
            command.Parameters.AddWithValue("@Price", product.Price);

            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            List<Product> products = new List<Product>();

            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string query = "SELECT * FROM Products WHERE ProductName LIKE @search";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@search", "%" + searchText + "%");

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();

                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString();
                product.Stock = Convert.ToInt32(reader["Stock"]);
                product.MinStock = Convert.ToInt32(reader["MinStock"]);
                product.Price = Convert.ToDecimal(reader["Price"]);

                products.Add(product);
            }

            connection.Close();

            return View("Index", products);
        }
    }
}