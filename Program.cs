using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Data;
using ProjectManager.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProjectContext>(options =>
    options.UseMySql(config.GetConnectionString("ProjectManager"),
            new MySqlServerVersion(new Version(10, 4, 22))
            ));

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	var Key = Convert.FromBase64String(config["JwtSettings:Key"]);
	o.SaveToken = true;

	o.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = config["JwtSettings:Issuer"],
		ValidAudience = config["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Key)
	};
});

builder.Services.AddSingleton<ICryptoService, CryptoService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
