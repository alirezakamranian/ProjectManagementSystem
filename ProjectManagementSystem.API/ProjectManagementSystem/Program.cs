
using Infrastructure.DataAccess;
using ProjectManagementSystem.ServiceExtension;
using Serilog;

//-_-_-_-- EntryPoint --_-_-_-//

var builder = WebApplication.CreateBuilder(args);

/*----------                                 -_-_-_-- Services Configuration --_-_-_-                                        ---------*/

//Controllers
builder.Services.ConfigureMvc();

//Logger 
builder.Host.ConfigureLogging();

//DbContext
builder.Services.ConfigureDbContext(builder);

//Swagger
builder.Services.ConfigureSwagger();

//Auth
builder.Services.ConfigureAuth(builder);

//AppServices
builder.Services.ConfigureAppServices();


var app = builder.Build();

/*----------                                 -_-_-_-- HTTP request pipeline --_-_-_-                                         --------- */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
