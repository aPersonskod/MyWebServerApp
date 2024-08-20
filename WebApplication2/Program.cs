using WebApplication2.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddSingleton<MyDataContext>();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
var app = builder.Build();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.MapControllers();
app.MapRazorPages();
app.Run();