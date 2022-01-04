using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authorization;

namespace api_account.Account;
using Data;
using Base;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly L2AccountContext _context;
    private readonly IAccountService _accountService;
    private readonly IRedisService _redis;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public AccountController(ILogger<AccountController> logger,
        L2AccountContext context,
        IAccountService accountService,
        IRedisService redis,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
        _redis = redis;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    [HttpGet]
    [Authorize]
    public async Task<string> Get()
    {
        return await Task.FromResult("hello");
    }

    [HttpPost("signup")]
    public async Task<ActionResult<bool>> CreateUser(AccountModel model)
    {
        var ip = this.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(ip)) ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        var db = _redis.GetConnection().GetDatabase();
        (await db.StringGetAsync($"account:new:{ip}")).TryParse(out int count);
        if (count > 1)
        {
            await db.KeyExpireAsync($"account:new:{ip}", new TimeSpan(0, 15, 0));
            return StatusCode(412, $"注册次数过多,15分钟后再试");
        }

        var exist = await _accountService.IsExistUserName(model.Name!).ConfigureAwait(false);
        if (exist) return StatusCode(412, $"已存在用户名为{model.Name}的账户");
        await _accountService.Signup(model.Name!, model.Password!).ConfigureAwait(false);
        await db.StringIncrementAsync($"account:new:{ip}");
        await db.KeyExpireAsync($"account:new:{ip}", new TimeSpan(0, 15, 0));
        return true;
    }
    [HttpPost("signin")]
    public async Task<ActionResult<string>> Login(AccountModel model)
    {
        var ip = this.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(ip)) ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        var db = _redis.GetConnection().GetDatabase();
        (await db.StringGetAsync($"account:wrong:{ip}")).TryParse(out int count);
        if (count > 4)
        {
            await db.KeyExpireAsync($"account:wrong:{ip}", new TimeSpan(0, 15, 0));
            return StatusCode(412, $"尝试次数过多,15分钟后再试");
        }

        var exist = await _accountService.IsExistUserName(model.Name!).ConfigureAwait(false);
        if (!exist)
        {
            await db.StringIncrementAsync($"account:wrong:{ip}");
            await db.KeyExpireAsync($"account:wrong:{ip}", new TimeSpan(0, 15, 0));
            return StatusCode(412, $"账号或密码错误");
        }
        var valid = await _accountService.IsValid(model.Name!, model.Password!).ConfigureAwait(false);
        if (valid == -1)
        {

            await db.StringIncrementAsync($"account:wrong:{ip}");
            await db.KeyExpireAsync($"account:wrong:{ip}", new TimeSpan(0, 15, 0));
            return StatusCode(412, $"账号或密码错误");
        }

        var claims = new[] { new Claim(ClaimTypes.Name!, model.Name!), new Claim("user_id", valid.ToString()) };
        _logger.LogInformation(_configuration["JWTSetting:SecretKey"]);
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWTSetting:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(_configuration["JWTSetting:Issuer"], _configuration["JWTSetting:Audience"], claims, expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWTSetting:AccessExpiration"])), signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return Ok(token);
    }
}
