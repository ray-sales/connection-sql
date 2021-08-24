using System;
using Microsoft.Data.SqlClient;
using Dapper;
using BaltaDataAcess.Models;

namespace BaltaDataAcess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";

            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSql = @"INSERT INTO 
                                [Category] 
                            VALUES(
                                @Id, 
                                @Title, 
                                @Url, 
                                @Summary, 
                                @Order, 
                                @Description, 
                                @Featured)";
            using (var connection = new SqlConnection(connectionString))
            {
                var rows = connection.Execute(insertSql, new {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                });

                Console.WriteLine($"{rows} linhas inseridas");
                var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
                foreach (var item in categories)
                {
                    Console.WriteLine($"{item.Id} - {item.Title}");
                }
            }
        }
    }
}
