using Application.Services.EmployeeService;
using Domain.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

//                                                                  Services Configuration

//Controllers
builder.Services.ConfigureMvc();

//DbContext
builder.Services.ConfigureDbContext(builder);

//InternalServices
builder.Services.ConfigureEmployeeService();

//Swagger
builder.Services.ConfigureSwagger();





var app = builder.Build();
//                                                                  HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
