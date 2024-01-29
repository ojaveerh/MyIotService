using Database.Context;
using Database.Data.Models;
using DatabaseService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly("ServiceApi")));
builder.Services.AddTransient<IDevicesService, DevicesService>();
builder.Services.AddTransient<IUserAccountsService, UserAccountsService>();
builder.Services.AddControllers();
//Add swagger documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(conf =>
{

    conf.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT with bearer. Example:'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    conf.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme="oauth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

MigrateDatabase(app);
SeedDatabase(app);

app.Run();

static void MigrateDatabase(IApplicationBuilder app)
{
    var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

    if (serviceScope != null)
    {
        var databaseContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
        databaseContext.Database.Migrate();
    }
}

static void SeedDatabase(IApplicationBuilder app)
{
    var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

    if (serviceScope != null)
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();

        if (!context.UserAccounts.Any())
        {
            context.UserAccounts.Add(new UserAccount { UserName= "test", Password = "test1", Role="Admin" });
        }
        context.SaveChanges();

        if (!context.Devices.Any())
        {
            context.Devices.Add(new Device { UserAccount = null, DeviceNr = 123, Name = "Factory device", Description = "Suitable for factories" });
        }
        context.SaveChanges();

        var deviceId = context.Devices.Where(x => x.DeviceNr == 123).Select(x => x.Id).FirstOrDefault();

        if (!context.DeviceDatas.Any())
        {
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1000, Name = "Inside temperature", MinRange = -150, MaxRange = 150, Value = 22 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1010, Name = "Has outside temperature sensor", MinRange = 0, MaxRange = 1, Value = 1 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1011, Name = "Outside temperature", MinRange = -100, MaxRange = 100, Value = 32 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 1020, Name = "Water temperature", MinRange = -100, MaxRange = 100, Value = 50 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 2000, Name = "OperationTimeInSec", MinRange = 0, MaxRange = int.MaxValue, Value = 0 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 2010, Name = "WorkingHour", MinRange = 0, MaxRange = int.MaxValue, Value = 0 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 4040, Name = "IsOperational", MinRange = 0, MaxRange = 1, Value = 1 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 5050, Name = "SilentMode", MinRange = 0, MaxRange = 1, Value = 1 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 8000, Name = "Machine is broken", MinRange = 0, MaxRange = 1, Value = 0 });
            context.DeviceDatas.Add(new DeviceData { DeviceId = deviceId, DataId = 9000, Name = "SerialNumber", MinRange = 0, MaxRange = 999999, Value = 12345 });
        }
        context.SaveChanges();
    }
}