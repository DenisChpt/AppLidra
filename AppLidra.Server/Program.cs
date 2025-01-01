//-----------------------------------------------------------------------
// <copiright file="Program.cs">
//      Copyright (c) 2024 Damache Kamil, Ziani Racim, Chaput Denis. All rights reserved.
// </copyright>
// <author> Damache Kamil, Ziani Racim, Chaput Denis </author>
//-----------------------------------------------------------------------

using System.Text;
using AppLidra.Server.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Ajoutez ces lignes pour servir le client Blazor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Vos services existants...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DataBase
string dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
if (!Directory.Exists(dataFolder))
{
    Directory.CreateDirectory(dataFolder);
}

builder.Services.AddSingleton(new JsonDataStore(Path.Combine(dataFolder, "data.json")));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,  // Modifi� � false pour le test
            ValidateAudience = false, // Modifi� � false pour le test
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
        };
    });

// Cors
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors("AllowAll");

// Commentez temporairement la redirection HTTPS pour tester
// app.UseHttpsRedirection();

// Ajoutez ces lignes pour servir le client Blazor
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Modifiez le mapping des endpoints
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();