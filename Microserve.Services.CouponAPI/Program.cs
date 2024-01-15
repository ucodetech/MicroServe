using AutoMapper;
using Microserve.Services.CouponAPI;
using Microserve.Services.CouponAPI.Data;
using Microserve.Services.CouponAPI.Models.DTOs;
using Microserve.Web.Services;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApiConnections")));

//Register the mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//register the lifetime
builder.Services.AddSingleton(mapper);
//add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
ApplyMigration();
app.Run();

//check for migration automatically and apply them when the application runs
void ApplyMigration()
{
    //create scope
    using (var scope = app.Services.CreateScope())
    {
        //get the application db context service
        var _db =scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //check if there is any pending migrations 
        if(_db.Database.GetPendingMigrations().Count() > 0)
        {
            //if there is migration then apply it 
            _db.Database.Migrate();
        }
    }

}