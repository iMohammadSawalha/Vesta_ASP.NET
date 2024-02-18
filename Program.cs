using Microsoft.EntityFrameworkCore;
using Vesta.Data;
using Vesta.Interfaces;
using Vesta.Repositories;
using Vesta.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration["Vesta:ConnectionString"];
var AccessTokenSecret = builder.Configuration["Vesta:JWT_AccessToken_SecretKey"];
var RefreshTokenSecret = builder.Configuration["Vesta:JWT_RefreshToken_SecretKey"];
var PasswordHashSalt = builder.Configuration["Vesta:PasswordHashSalt"];
var SenderHostAddress = builder.Configuration["Vesta:SenderHostAddress"];
var SenderHostPort = builder.Configuration["Vesta:SenderHostPort"];
var SenderUserEmail = builder.Configuration["Vesta:SenderUserEmail"];
var SenderUserPassword = builder.Configuration["Vesta:SenderUserPassword"];


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(ConnectionString);
});
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserTokenRepository,UserTokenRepository>();
builder.Services.AddSingleton<IEmailService, EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    Environment.SetEnvironmentVariable("Vesta:JWT_AccessToken_SecretKey", AccessTokenSecret);
    Environment.SetEnvironmentVariable("Vesta:JWT_RefreshToken_SecretKey", RefreshTokenSecret);
    Environment.SetEnvironmentVariable("Vesta:PasswordHashSalt", PasswordHashSalt);
    Environment.SetEnvironmentVariable("Vesta:SenderHostAddress", SenderHostAddress);
    Environment.SetEnvironmentVariable("Vesta:SenderHostPort", SenderHostPort);
    Environment.SetEnvironmentVariable("Vesta:SenderUserEmail", SenderUserEmail);
    Environment.SetEnvironmentVariable("Vesta:SenderUserPassword", SenderUserPassword);
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
