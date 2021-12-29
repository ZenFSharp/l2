using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using StackExchange.Redis;

namespace api_account.Base;

public interface IRedisService
{
    ConnectionMultiplexer GetConnection();
}

public class RedisService : IRedisService
{
    private ConnectionMultiplexer? redis;
    public ConnectionMultiplexer GetConnection()
    {
        if (redis == null) redis = ConnectionMultiplexer.Connect("localhost");
        return redis;
    }
}
