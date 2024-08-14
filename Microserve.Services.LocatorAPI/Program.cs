using AutoMapper;
using Microserve.Services.LocatorAPI;
using Microserve.Services.LocatorAPI.Data;
using Microserve.Services.LocatorAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApiLocatorConnections")));

//Register the mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//register the lifetime
builder.Services.AddSingleton(mapper);
//add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option
    =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as follwing: `Bearer Generated-JWT-Token`",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[] {}
        }
    });
});
builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);

builder.AddAppAuthentication();

builder.Services.AddAuthorization();


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
    c.DocumentTitle = "Locator API";
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
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