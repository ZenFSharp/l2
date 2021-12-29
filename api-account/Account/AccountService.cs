using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace api_account.Account;
using Data;

public interface IAccountService
{
    Task<int> IsValid(string name, string password);
    Task<bool> Signup(string name, string password);
    Task<bool> IsExistUserName(string name);
}

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly L2AccountContext _context;
    public AccountService(ILogger<AccountService> logger, L2AccountContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<int> IsValid(string name, string password)
    {
        var _user = await _context.Account!.FirstOrDefaultAsync(u => u.Name == name).ConfigureAwait(false);
        string hashed = GetHash(name, password);
        return hashed == _user!.Hash ? _user.Id : -1;
    }

    public async Task<bool> Signup(string name, string password)
    {
        var user = new Account
        {
            Name = name,
            CreatedOn = DateTime.Now.ToUniversalTime(),
            Hash = GetHash(name, password)
        };
        await _context.Account!.AddAsync(user).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }

    public async Task<bool> IsExistUserName(string name)
    {
        var _user = await _context.Account!.FirstOrDefaultAsync(u => u.Name == name).ConfigureAwait(false);
        return _user != null;
    }

    private string GetHash(string name, string password)
    {
        var salt_str = name.First().ToString() + name.First().ToString() + "==";
        byte[] salt = Convert.FromBase64String(salt_str);
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }
}
