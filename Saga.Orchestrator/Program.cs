var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("Order", c =>
                                        c.BaseAddress = new Uri("https://localhost:5011"));
builder.Services.AddHttpClient("Inventory", c =>
                                        c.BaseAddress = new Uri("https://localhost:5021"));
builder.Services.AddHttpClient("Notifier", c =>
                                        c.BaseAddress = new Uri("https://localhost:5031"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
