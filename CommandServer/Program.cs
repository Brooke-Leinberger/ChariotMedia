using WebApplication1.Controllers;
using static WebApplication1.Controllers.Rpi;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//motors are given 2 bytes of data each

app.MapGet("/", () => "Hello World!");

app.Map("/shutdown", () => SetSpeed(0));

app.Map("/speed/{id}", (float id) => SetSpeed(id));

app.Run();