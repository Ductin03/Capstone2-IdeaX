using CloudinaryDotNet;
using IdeaX.ChatHub;
using IdeaX.Entities;
using IdeaX.Model.RequestModels;
using IdeaX.Repository;
using IdeaX.Seeder;
using IdeaX.Services;
using IdeaX.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdeaXDbContext>(options => options.UseNpgsql("Host=localhost;Database=IdeaX;Username=postgres;Password=123456"));

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EmailSettingModel>(builder.Configuration.GetSection("Emailsettings"));

builder.Services.AddSignalR();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<MatchingService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.AddScoped<IIdeaService, IdeaService>();
builder.Services.AddScoped<IIdeaRepository, IdeaRepository>();

builder.Services.AddScoped<EmailCacheService>();

builder.Services.AddScoped<ChatHubService>();

builder.Services.AddScoped<IAuth, AuthSevice>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Name = "Bearer",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
    }
});


});
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = false,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"])),
//            ClockSkew = TimeSpan.Zero,
//            NameClaimType = ClaimTypes.NameIdentifier
//        };
//    });



var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");

var cloudinaryAccount = new Account(
    cloudinaryConfig["CloudName"],
    cloudinaryConfig["ApiKey"],
    cloudinaryConfig["ApiSecret"]
);

builder.Services.AddSingleton(new Cloudinary(cloudinaryAccount));
builder.Services.AddSingleton<CloudinaryService>();


// Cấu hình JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})

.AddCookie("Cookies")
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        ClockSkew = TimeSpan.Zero
    };
})
// Thêm xác thực Google OAuth
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.SaveTokens = true; // Quan trọng: Lưu lại tokens để dùng cho refresh token
    googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
    googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
    googleOptions.Scope.Add("openid");
    googleOptions.AccessType = "offline"; // để lấy refresh_token
    googleOptions.Events.OnCreatingTicket = context =>
    {
        // Bạn có thể truy cập và lưu các token tại đây
        //var accessToken = context.TokenResponse.AccessToken;
        //var refreshToken = context.TokenResponse.RefreshToken;

        // Lưu vào context để xử lý sau trong controller
        //context.HttpContext.Items["GoogleAccessToken"] = accessToken;
        //context.HttpContext.Items["GoogleRefreshToken"] = refreshToken;
        context.HttpContext.Items["GoogleAccessToken"] = context.AccessToken;
        context.HttpContext.Items["GoogleRefreshToken"] = context.RefreshToken;

        return Task.CompletedTask;
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IdeaXDbContext>();
    DbInitializer.Seed(db);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHubService>("/userchathub");
app.MapHub<AIChatHub>("/aichathub");

app.MapControllers();

app.Run();
