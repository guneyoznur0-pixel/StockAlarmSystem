using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace StockAlarmSystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            string connectionString = "Server=DESKTOP-LPUNACN\\SQLEXPRESS;Database=StockAlarmDB;Trusted_Connection=True;TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            // Toplam ürün
            string totalProductQuery = "SELECT COUNT(*) FROM Products";
            SqlCommand totalProductCommand = new SqlCommand(totalProductQuery, connection);
            int totalProducts = (int)totalProductCommand.ExecuteScalar();

            // Kritik stok
            string criticalQuery = "SELECT COUNT(*) FROM Products WHERE Stock <= MinStock";
            SqlCommand criticalCommand = new SqlCommand(criticalQuery, connection);
            int criticalStock = (int)criticalCommand.ExecuteScalar();

            // Toplam satış
            string salesQuery = "SELECT COUNT(*) FROM Sales";
            SqlCommand salesCommand = new SqlCommand(salesQuery, connection);
            int totalSales = (int)salesCommand.ExecuteScalar();
            string supplierQuery = "SELECT COUNT(*) FROM Suppliers";
            SqlCommand supplierCommand = new SqlCommand(supplierQuery, connection);
            int totalSuppliers = (int)supplierCommand.ExecuteScalar();

            string customerQuery = "SELECT COUNT(*) FROM Customers";
            SqlCommand customerCommand = new SqlCommand(customerQuery, connection);
            int totalCustomers = (int)customerCommand.ExecuteScalar();

            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.TotalCustomers = totalCustomers;
            connection.Close();

            ViewBag.TotalProducts = totalProducts;
            ViewBag.CriticalStock = criticalStock;
            ViewBag.TotalSales = totalSales;

            return View();
        }
    }
}