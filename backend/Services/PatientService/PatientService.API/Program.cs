using Business.Abstract;
using Business.Concrete;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Notification.Mail;
using Core.Utilities.Notification.Mail.SmptMail;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using PatientService.API.BackgroundJobs;
using PatientService.Business.Abstract;
using PatientService.Business.Concrete;
using PatientService.Business.Mapping.Profiles;
using PatientService.DataAccess.Abstract;
using PatientService.DataAccess.Concrete.EntityFramework;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PatientService API",
        Version = "v1",
        Description = "Patient Appointment Service API"
    });
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddDbContext<PatientAppointmentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PatientAppointmentDb"), 
                     npgsqlOptions => npgsqlOptions.MigrationsAssembly("PatientService.DataAccess")));

builder.Services.AddHostedService<SlotGenerationBackgroundService>();


builder.Services.AddScoped<IAppointmentService, AppointmentManager>();
builder.Services.AddScoped<IAppointmentDal, EfAppointmentDal>();

builder.Services.AddScoped<IAppointmentSlotService, AppointmentSlotManager>();
builder.Services.AddScoped<IAppointmentSlotDal, EfAppointmentSlotDal>();

builder.Services.AddScoped<IPatientService, PatientManager>();
builder.Services.AddScoped<IPatientDal, EfPatientDal>();

builder.Services.AddScoped<IDoctorService, DoctorManager>();
builder.Services.AddScoped<IDoctorDal, EfDoctorDal>();

builder.Services.AddScoped<IWaitlistService, WaitlistManager>();
builder.Services.AddScoped<IWaitlistDal, EfWaitlistDal>();
builder.Services.AddSingleton<ILoggerServiceBase, SerilogLogger>();
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

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Nginx üzerinden erişimde Swagger UI gateway path'ini kullanmalı
        // Tarayıcıdaki URL: /swagger/patient/v1/swagger.json
        // Nginx bu isteği /swagger/v1/swagger.json'a rewrite edip patient_service'e yönlendirir
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
        // Geliştirme ortamında, şema yoksa otomatik oluştur (migrations'a bağlı kalmadan)
        db.Database.EnsureCreated();
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
