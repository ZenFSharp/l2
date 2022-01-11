using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authorization;

namespace api_character.Controllers;
using Data;
using Services;

[Route("api/[controller]")]
[ApiController]
public class RaceController : ControllerBase
{
    private readonly ILogger<RaceController> _logger;    
    private readonly IRaceService _service;

    public RaceController(ILogger<RaceController> logger,
        
        IRaceService raceService)
    {
        _logger = logger;
        _service = raceService;
    }

    [HttpGet("all")]
    [Authorize]
    public Task<IEnumerable<Race>> Get()
    {
        return _service.GetAll();
    }
}
