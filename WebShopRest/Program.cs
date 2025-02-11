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

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowAll");

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
