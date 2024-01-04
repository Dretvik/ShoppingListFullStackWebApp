using System.Data.SqlClient;
using Dapper;
using ShoppingListWithBackend;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();
const string connStr = @"Data Source=(localdb)\local;Initial Catalog=ShoppingList;Integrated Security=True";

app.MapGet("/shoppingListItems", async () =>
    {
        var conn = new SqlConnection(connStr);
        const string sql = "SELECT Id, Text FROM ShoppingListItems";
        var shoppingListItems = await conn.QueryAsync<ShoppingListItem>(sql);
        return shoppingListItems;
    })
    .WithName("GetShoppingListItems")
    .WithOpenApi();

app.MapPost("/shoppingListItems", async (ShoppingListItem newItem) =>
    {
        try
        {
            newItem.Id = Guid.NewGuid();

            using (var conn = new SqlConnection(connStr))
            {
                const string sql = "INSERT INTO ShoppingListItems (Id, Text) VALUES (@Id, @Text);";
                await conn.ExecuteAsync(sql, newItem);
            }

            return newItem;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating shopping list item: {ex.Message}");
            throw;  
        }
    })
    .WithName("CreateShoppingListItem")
    .WithOpenApi();

app.MapDelete("/shoppingListItems/{id}", async (Guid id) =>
    {
        var conn = new SqlConnection(connStr);
        const string sql = "DELETE FROM ShoppingListItems WHERE Id = @Id";
        await conn.ExecuteAsync(sql, new { Id = id });
    })
    .WithName("DeleteShoppingListItem")
    .WithOpenApi();

app.UseStaticFiles();
app.Run();
