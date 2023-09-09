using Microsoft.Extensions.Configuration;
using SharedLibrary.Extensions;
using SharedLibrary.Helpers;
using SharedLibrary.MessagingLibraries;
using EmailService.Class;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<appSettingClass>(builder.Configuration.GetSection("urls"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton<IMailService, MailService >();

//builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidationFilterAttribute>();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAuthorized();
    app.UseSwagger();
	app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
