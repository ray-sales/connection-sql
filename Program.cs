using System;
using Microsoft.Data.SqlClient;
using Dapper;
using BaltaDataAcess.Models;
using System.Data;

namespace BaltaDataAcess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";


            using (var connection = new SqlConnection(connectionString))
            {

                // UpdateCategory(connection);
                // CreateManyCategory(connection);
                // ListCategories(connection);
                // CreateCategory(connection);
                // ExecuteProcedure(connection);
                // ExecuteReadProcedure(connection);
                // ExecuteScalar(connection);
                OneToOne(connection);
            }
        }


        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void CreateCategory(SqlConnection connection)
        {
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
            var rows = connection.Execute(insertSql, new
            {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Summary = category.Summary,
                Order = category.Order,
                Description = category.Description,
                Featured = category.Featured
            });
        }
        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("34b774cb-7658-4f67-a545-72834418ea49"),
                title = "Frontend 2021"
            });

            Console.WriteLine($"{rows} registros atualizados");
        }


        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "categoria nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria destinada a serviços do AWS";
            category2.Order = 9;
            category2.Summary = "categoria nova";
            category2.Featured = true;


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
            var rows = connection.Execute(insertSql, new[]{
                new
            {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Summary = category.Summary,
                Order = category.Order,
                Description = category.Description,
                Featured = category.Featured
            },
            new
            {
                Id = category2.Id,
                Title = category2.Title,
                Url = category2.Url,
                Summary = category2.Summary,
                Order = category2.Order,
                Description = category2.Description,
                Featured = category2.Featured
            }}
            );
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "spDeleteStudent";

            var parameters = new { StudentId = "47976efe-982a-4f5c-a593-4a008e0f062c" };

            var affectedRows = connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "spGetCoursesByCategory";
            var parameters = new { CategoryId = "af3407aa-11ae-4621-a2ef-2028b85507c4" };
            var courses = connection.Query(
                procedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            foreach (var item in courses)
            {
                Console.WriteLine(item.Id);
            }

        }

        static void ExecuteScalar(SqlConnection connection)
        {
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
                            OUTPUT inserted.Id
                        VALUES(
                            NEWID(), 
                            @Title, 
                            @Url, 
                            @Summary, 
                            @Order, 
                            @Description, 
                            @Featured)";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                Title = category.Title,
                Url = category.Url,
                Summary = category.Summary,
                Order = category.Order,
                Description = category.Description,
                Featured = category.Featured
            });

            Console.WriteLine($"A categoria inserida foi: {id}");
        }
        static void OneToOne(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    *
                FROM
                    CareerItem
                INNER JOIN Course
                    CareerItem.CourseId = Course.Id
            ";

            var items = connection.Query(sql);

            foreach(var item in items){
                Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
            }
        }

        static void OneToMany(){
            
        }

    }


}
