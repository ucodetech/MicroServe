using Microserve.Services.AuthAPI.Data;
using Microserve.Services.AuthAPI.Helpers;
using Microserve.Services.AuthAPI.Models;
using Microserve.Services.AuthAPI.Models.JWT;
using Microserve.Services.AuthAPI.Service;
using Microserve.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AuthApiConnections")));
//add jwt option
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//register the services
//auth api service
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UserHelper>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtGenerator>();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    c.DocumentTitle = "Authentication API";
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

//app.UseSession(); // Add this line to enable session

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();

app.Run();
//check for migration automatically and apply them when the application runs
void ApplyMigration()
{
    //create scope
    using (var scope = app.Services.CreateScope())
    {
        //get the application db context service
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //check if there is any pending migrations 
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            //if there is migration then apply it 
            _db.Database.Migrate();
        }
    }

}