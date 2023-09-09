using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Domain.Contexts;
using Chat.Domain.Contracts.Repositories;
using Chat.Domain.Entities;
using Chat.Hubs;
using Chat.Repositories;
using Chat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Extensions;
using SharedLibrary.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var connectionString = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository, EfRepository>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddScoped<ChatDbContext>();



builder.Services.Configure<appSettingClass>(builder.Configuration.GetSection("Setting"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{



    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };

    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (string.IsNullOrEmpty(accessToken) == false)
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };


});

builder.Services.AddSignalR();

builder.Services.AddAuthorization();

//builder.Services.AddMvcCore().AddRazorViewEngine(); 
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddProgressiveWebApp();


var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
    app.UseSwaggerAuthorized();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.MapHub<ChatHub>("/chatHub");
app.MapHub<ChatHub>("/LanguageHub");
app.MapHub<ChatHub>("/CampaignHub");
app.MapHub<ChatHub>("/CustomerStateChangeHub");
app.MapHub<ChatHub>("/CustomerNotificationHub");

app.UseRouting();


app.MapDefaultControllerRoute();
app.MapRazorPages();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Auth}/{action=Index}/{id?}");

//app.MapGet("/security/getMessage",
//    () => "Hello World!").RequireAuthorization();


//app.MapPost("/security/createToken",
//    [AllowAnonymous] (Users user) =>
//    {
//        if (user.UserName == "admin" && user.Password == "123456")
//        {
//            var issuer = builder.Configuration["Jwt:Issuer"];
//            var audience = builder.Configuration["Jwt:Audience"];
//            var key = Encoding.ASCII.GetBytes
//                (builder.Configuration["Jwt:Key"]);
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                    new Claim("Id", Guid.NewGuid().ToString()),
//                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
//                    new Claim(JwtRegisteredClaimNames.Jti,
//                        Guid.NewGuid().ToString())
//                }),
//                Expires = DateTime.UtcNow.AddMinutes(5),
//                Issuer = issuer,
//                Audience = audience,
//                SigningCredentials = new SigningCredentials
//                (new SymmetricSecurityKey(key),
//                    SecurityAlgorithms.HmacSha512Signature)
//            };
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var jwtToken = tokenHandler.WriteToken(token);
//            var stringToken = tokenHandler.WriteToken(token);
//            return Results.Ok(stringToken);
//        }
//        return Results.Unauthorized();
//    });



app.Run();


