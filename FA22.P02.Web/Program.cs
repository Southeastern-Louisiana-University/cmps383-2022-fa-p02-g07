using Microsoft.AspNetCore.WebUtilities;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


List<Product> products = new List<Product>() {
                new Product(){ Id = 1, Name="Ketchup", Description="Condiment", Price= 4.99},
                new Product(){ Id = 2, Name="Mustard", Description="Condiment", Price= 2.99},
                new Product(){ Id = 3, Name="Mayo", Description="Condiment", Price= 3.99},
                
            };



var response = new HttpResponseMessage(HttpStatusCode.OK);



app.MapGet("/api/products", () =>
{
   
        return Results.Ok(products);
    
})
    .WithName("GetProducts");

app.MapGet("/api/products/{id}", (int id) =>
{

    var result = products.FirstOrDefault(x => x.Id == id);

    if (result == null)
    {
        return Results.NotFound("Product does not exist");
    }


    return Results.Ok(result);


})
    .WithName("GetProduct");

app.MapPost("/api/products", (Product foo) =>
{

    if (foo.Id == 0 || foo.Id < 0)
    {
        return Results.BadRequest("Id cannot be 0 or less than 0");
    }

   
    

    if (string.IsNullOrEmpty(foo.Name) ||
        foo.Name.Length > 120 ||
        foo.Price <= 0 ||
        string.IsNullOrEmpty(foo.Description))
    {
        return Results.BadRequest();
    }





    products.Add(foo);
    return Results.Created("This is your created product:", foo);





})
    .Produces(400)
    .Produces(201, typeof(Product));
    

app.MapPut("/api/products/{id}", (int id, Product foo) =>
{
    var result = products.FirstOrDefault(x => x.Id == id);

    if (products.FirstOrDefault(x => x.Id == id) == null)
    {
        return Results.NotFound("Product does not exist");
    }


    if (foo.Name == null || foo.Name == "")
    {
        return Results.BadRequest("No Name");
    }

    if (foo.Name.Length > 120 || foo.Name.Length == 0)
    {
        return Results.BadRequest("Invalid Product name");
    }

    if (foo.Description == null || foo.Name == "")
    {
        return Results.BadRequest("No Description");
    }


    if (foo.Price <= 0)
    {
        return Results.BadRequest("Invalid Price");
    }

    
    foreach(var products in products)
    {
        if(foo.Id == products.Id)
        {
            
            products.Name = foo.Name;
            products.Description = foo.Description;
            products.Price = foo.Price;
        }
    }
    
    
    return Results.Ok(foo);
})
    .WithName("UpdateProduct");

app.MapDelete("/api/products/{id}", (int id) =>
{
    
    var current = products.FirstOrDefault(x => x.Id == id);

    if (current == null)
    {
        return Results.NotFound();
    }

    products.Remove(current);

    return Results.Ok(products);


})
    .Produces(400)
    .Produces(404)
    .Produces(200, typeof(Product))
    .WithName("DeleteProduct");
    





app.Run();

public  class Product {

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }


}



//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }