
using Infrastructure.DataAccess;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
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

//Cors
builder.Services.ConfigureCors();

//AppServices
builder.Services.ConfigureAppServices();

//SignalR
builder.Services.AddSignalR();

///                              NOTE : defination of Services that configured above are exist in
///                                          serviceExtension folder in current layer

var app = builder.Build();

/*----------                                 -_-_-_-- HTTP request pipeline --_-_-_-                                         --------- */

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Allow");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotifHub>("/notifhub");
app.MapHub<TaskHub>("/taskhub");

app.Run();
