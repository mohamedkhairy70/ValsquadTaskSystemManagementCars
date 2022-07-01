using ValsquadTaskSystemManagementCars.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

///////////////////////////////////////    Builder     //////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ManagementCarsDBContext>(cfg =>
                                            cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection")));


builder.Services.AddCors(cfg => cfg.AddPolicy("AllowWebApp", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SystemManagementCars", Version = "v1" });
});




///////////////////////////////////////    App Builder     //////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SystemManagementCars v1"));
    app.UseCors("AllowWebApp");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
