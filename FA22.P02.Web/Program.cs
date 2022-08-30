using System;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;

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


List<Product> products1 = new List<Product>() {
                new Product(){ Id = 1, Name="Ketchup", Description="Condiment", Price= 4.99},
                new Product(){ Id = 2, Name="Mustard", Description="Condiment", Price= 2.99},
                new Product(){ Id = 3, Name="Mayo", Description="Condiment", Price= 3.99},
                
            };





app.MapGet("/product/get-product", (int i) =>
{
    var id = products1[i];
    return id;


})
    .WithName("GetProduct");



app.MapGet("/products/{get-all}", () =>
{
    
        var Id = products1;
        return Id;
    
})
    .WithName("GetProducts");

app.MapPost("/products/{create-product}", (int i, string name, string description, double price) =>
{
    
    var product =  new Product(){Id = i, Name= name, Description= description, Price= price };
    products1.Add(product);
     var Id = products1;
    return Id;




})
    .WithName("CreateProduct");

app.MapPut("/products/{update-product}", (int i, string name, string description, double price) =>
{

    var product = new Product() { Id = i, Name = name, Description = description, Price = price };
    products1[i] = product;
    var Id = products1;
    return Id;
})
    .WithName("UpdateProduct");

app.MapDelete("/products/{delete-product}", (int i) =>
{

    products1.RemoveAt(i);
    var Id = products1;
    return Id;


})
    .WithName("DeleteProduct");





app.Run();



//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Product {

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }


}