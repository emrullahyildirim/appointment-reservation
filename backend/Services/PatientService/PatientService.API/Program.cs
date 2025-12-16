using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using PatientService.Business.Mapping.Profiles;
using PatientService.DataAccess.Concrete.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddDbContext<PatientAppointmentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PatientAppointmentDb")));



builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IAppointmentDal, EfAppointmentDal>();

builder.Services.AddScoped<IAppointmentSlotService, AppointmentSlotManager>();
builder.Services.AddScoped<IAppointmentSlotDal, EfAppointmentSlotDal>();

builder.Services.AddScoped<IPatientService, PatientManager>();
builder.Services.AddScoped<IPatientDal, EfPatientDal>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseCors("AllowAll");
// Configure the HTTP request pipeline.

    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/patient/v1/swagger.json", "PatientService API V1");
        c.RoutePrefix = "swagger";
    });



if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var db = services.GetRequiredService<PatientAppointmentContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        //var logger = services.GetRequiredService<ILogger<Program>>();
        //logger.LogError(ex, "Database migration failed");
        //throw;
    }
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
