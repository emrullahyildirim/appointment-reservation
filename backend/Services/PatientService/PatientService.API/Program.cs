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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PatientAppointmentContext>();

        //logger.LogInformation("Applying database migrations...");
        dbContext.Database.Migrate();
        //logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        throw ex;
        //var logger = app.Services.GetRequiredService<ILogger<Program>>();
        //logger.LogError(ex, "An error occurred while migrating the database. Make sure PostgreSQL is running and connection string is correct.");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
