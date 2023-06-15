// Tools -> NuGet package manager to change packages in project.

using HelloWorld;
using Microsoft.EntityFrameworkCore;

// Creates a builder which allows us to build an app
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductDatabase>(options =>
    options.UseInMemoryDatabase("ProductDatabase")
);

// Good practice to also add exception filter
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// reference to our application
var app = builder.Build();

// Change default page in Properties/launchSettings.json
// Mapping
app.MapGet("/", () => "Welcome to the website" );

// For non-async, just use ToLlist()
//app.MapGet("/products", (ProductDatabase db) =>
//db.Products.ToList()
//    );
app.MapGet("/products", async (ProductDatabase db) =>
        await db.Products.ToListAsync()
    );

// (Reference to the item we are posting, the database we are posting to)=>{}
app.MapPost("/products", (Product product, ProductDatabase db) => {
    db.Products.Add(product); // take a product and add it to the databases' set (db.Products)
    db.SaveChanges(); // save the new states of the database
    return Results.Created($"/products/{product.ID}", product);
});

// (value type, Reference to the item we are posting, the database we are posting to)=>{}
app.MapPut("/products/{id}", (int id, Product updatedProduct, ProductDatabase db) => {
    // Find the ID
    var product = db.Products.Find(id);

    if(product is null)
    {
        return Results.NotFound(); // Asp.Net core function
    }
        if (updatedProduct.Name != null)
    {
        product.Name = updatedProduct.Name;
    }
    if (updatedProduct.Description != null)
    {
        product.Description = updatedProduct.Description;
    }

    db.SaveChanges();

    return Results.NoContent(); // NoContent to keep simple
    
    
});

app.MapDelete("/products/{id}", (int id, ProductDatabase db) => { 
    if(db.Products.Find(id) is Product product)
    {
        db.Products.Remove(product);

        db.SaveChanges();

        return Results.Ok(product);
    }
    else {
        return Results.NotFound();
    }
});
/*
    Send requests in terminal (curl)
        View->Terminal
        POST
            curl -d '{"id":1, "name":"Hello Coding","description": "Testing out .Net!"}' -H "Content-Type: application/json" -X POST https://localhost:7232/products
        PUT
            curl -d '{"name":"Edited Coding", "description":"Making sure the PUT worked."}' -H "Content-Type: application/jason" -X PUT https://localhost:7232/products/1
        DELETE
            curl -X DELETE https://localhost:7232/products/1
 */

// Run the app
app.Run();