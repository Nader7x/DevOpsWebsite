using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Opsphere.Data;
using Opsphere.Data.Interfaces;
using Opsphere.Helpers;
using Opsphere.Services;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Opsphere API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
builder.Services.AddSingleton<IFileProvider>(physicalProvider);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = 
        options.DefaultChallengeScheme = 
            options.DefaultForbidScheme =       
                options.DefaultScheme = 
                    options.DefaultSignInScheme = 
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"] ))
    };
    options.Events = new JwtBearerEvents()
    {
      OnMessageReceived = context =>
      {
          var accessToken = context.Request.Query["access_token"];
          var path = context.HttpContext.Request.Path;
          if (string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/Notify"))
          {
             context.Token = accessToken; 
          }

          return Task.CompletedTask;
      }
    };
    options.IncludeErrorDetails = true;
    options.SaveToken = true;
});

var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "uploads")),
    RequestPath = "/uploads"
});
app.MapControllers();

app.MapHub<NotificationService>("/Notify");

app.Run();



