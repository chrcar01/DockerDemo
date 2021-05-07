using System;
using DockerDemo.OrdersApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DockerDemo.OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Customer>> GetAll()
        {
            var sql = "SELECT [CustomerId], [FirstName], [LastName] FROM [Customer]";
            var connectionString = "server=.,1439;database=OrdersDemo;uid=OrdersApp;pwd=Password@12345;";
            await using var cn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, cn);
            var result = new List<Customer>();
            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }

            return result;
        }

        [HttpGet("{customerId:int}")]
        public async Task<Customer> Get(int customerId)
        {
            var sql = "SELECT [CustomerId], [FirstName], [LastName] FROM [Customer] WHERE [CustomerId]=@CustomerId";
            var connectionString = "server=.,1439;database=OrdersDemo;uid=OrdersApp;pwd=Password@12345;";
            await using var cn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            Customer result = null;
            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            }

            return result;
        }

        [HttpPost]
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var sql = "INSERT [Customer]([FirstName], [LastName])VALUES(@FirstName, @LastName);SELECT SCOPE_IDENTITY() AS 'CustomerId';";
            var connectionString = "server=.,1439;database=OrdersDemo;uid=OrdersApp;pwd=Password@12345;";
            await using var cn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = customer.FirstName;
            cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = customer.LastName;
            await cn.OpenAsync();
            customer.CustomerId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return customer;
        }


        [HttpPut]
        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var sql = "UPDATE [Customer] SET [FirstName]=@FirstName, [LastName]=@LastName WHERE [CustomerId]=@CustomerId;";
            var connectionString = "server=.,1439;database=OrdersDemo;uid=OrdersApp;pwd=Password@12345;";
            await using var cn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customer.CustomerId;
            cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = customer.FirstName;
            cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = customer.LastName;
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return customer;
        }

        [HttpDelete("{customerId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            var sql = "DELETE FROM [Customer] WHERE [CustomerId]=@CustomerId;";
            var connectionString = "server=.,1439;database=OrdersDemo;uid=OrdersApp;pwd=Password@12345;";
            await using var cn = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return NoContent();
        }
    }
}
