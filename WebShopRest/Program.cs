using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using WebShopSem4;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ProductRepository>(new ProductRepository());
//builder.Services.AddSingleton<OrderHistoryRepository>(new OrderHistoryRepository());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                              policy =>
                              {
                                  policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                              });
});

var app = builder.Build();
app.Use(async (context, next) =>
{
    var allowedMethods = new HashSet<string> { "GET", "POST" };
    if (!allowedMethods.Contains(context.Request.Method))
    {
        context.Response.StatusCode = 405;
        await context.Response.WriteAsync("Method Not Allowed");
        return;
    }
    await next();
});

// Configure MIME type whitelisting
var provider = new FileExtensionContentTypeProvider();
provider.Mappings.Clear(); // Clear all existing mappings

// Add allowed MIME types
provider.Mappings[".txt"] = "text/plain";
provider.Mappings[".jpg"] = "image/jpeg";
provider.Mappings[".png"] = "image/png";
provider.Mappings[".pdf"] = "application/pdf";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});


app.UseHsts(); // Enforce HSTS for 1 year
               

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI();


app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();  

app.UseAuthorization();

app.MapControllers(); // If using MVC/Web API

app.Run();
