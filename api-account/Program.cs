using api_account.Data;
using api_account.Account;
using api_account.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,//是否验证签名,不验证的画可以篡改数据，不安全
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:SecretKey"])),
        ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
        ValidIssuer = builder.Configuration["JWTSetting:Issuer"],//发行人
        ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
        ValidAudience = builder.Configuration["JWTSetting:Audience"],//订阅人
        ValidateLifetime = true,//是否验证过期时间，过期了就拒绝访问
        ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
        RequireExpirationTime = true,
    };
});

builder.Services.AddDbContext<L2AccountContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionString"]));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRedisService, RedisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
