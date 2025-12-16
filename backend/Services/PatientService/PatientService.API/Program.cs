using Business.Abstract;
using Business.Concrete;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Notification.Mail;
using Core.Utilities.Notification.Mail.SmptMail;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using PatientService.Business.Abstract;
using PatientService.Business.Concrete;
using PatientService.Business.Mapping.Profiles;
using PatientService.DataAccess.Abstract;
using PatientService.DataAccess.Concrete.EntityFramework;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddDbContext<PatientAppointmentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PatientAppointmentDb")));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    //.WriteTo.File("logs/authservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton<ILoggerServiceBase, SerilogLogger>();




builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IAppointmentDal, EfAppointmentDal>();

builder.Services.AddScoped<IAppointmentSlotService, AppointmentSlotManager>();
builder.Services.AddScoped<IAppointmentSlotDal, EfAppointmentSlotDal>();

builder.Services.AddScoped<IPatientService, PatientManager>();
builder.Services.AddScoped<IPatientDal, EfPatientDal>();

builder.Services.AddScoped<IWaitlistService, WaitlistManager>();
builder.Services.AddScoped<IWaitlistDal, EfWaitlistDal>();


builder.Services.AddScoped<IDoctorDal, EfDoctorDal>();
builder.Services.AddScoped<IDoctorService, DoctorManager>();

builder.Services.AddScoped<IDoctorTitleDal, EfDoctorTitleDal>();
builder.Services.AddScoped<IDoctorTitleService, DoctorTitleManager>();

builder.Services.AddScoped<IMailService, MailSender>();


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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


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
