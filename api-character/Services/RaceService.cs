using Microsoft.EntityFrameworkCore;

namespace api_character.Services;
using Data;

public interface IRaceService
{
    Task<IEnumerable<Race>> GetAll();
}

public class RaceService : IRaceService
{
    private readonly L2CharacterContext _context;
    private readonly ILogger<RaceService> _logger;
    public RaceService(ILogger<RaceService> logger,
        L2CharacterContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<IEnumerable<Race>> GetAll()
    {
        return await _context.Race!.ToListAsync().ConfigureAwait(false);
    }
}