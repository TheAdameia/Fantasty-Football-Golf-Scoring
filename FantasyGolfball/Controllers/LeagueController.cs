using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyGolfball.Data;
using FantasyGolfball.Models;
using FantasyGolfball.Models.DTOs;

namespace FantasyGolfball.Controllers;
[ApiController]
[Route("api/[controller]")]

public class LeagueController : ControllerBase
{
    private FantasyGolfballDbContext _dbContext;

    public LeagueController(FantasyGolfballDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    // [Authorize]
    public IActionResult GetAll()
    {
        return Ok(_dbContext.Leagues);
    }

    [HttpPost]
    // [Authorize]
    public IActionResult Post(League league)
    {
        _dbContext.Leagues.Add(league);
        _dbContext.SaveChanges();
        return Created($"api/leagues/{league.LeagueId}", league);
    }

    // [HttpGet] // needs to also have distinct route
    // // [Authorize]
    // public IActionResult GetByUser(int userId)
    // {
    //     // return Ok(_dbContext.Leagues
    //     // .Where(l => l.))
    //     // expand it to solve it?
    // }

    [HttpGet("{leagueId}")]
    // [Authorize]
    public IActionResult GetByLeagueId(int leagueId)
    {
        return Ok(_dbContext.Leagues
        .SingleOrDefault(l => l.LeagueId == leagueId));
    }

    [HttpGet("not-full-leagues")]
    // [Authorize]
    public IActionResult GetNotFullLeagues()
    {
        var NotFullLeagues = _dbContext.Leagues
            // .Where(l => l.PlayerLimit > l.Participants.Count()) need a new way of doing this
            .Include(l => l.LeagueUsers)
                .ThenInclude(lu => lu.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
            .Select(l => new LeagueSafeExportDTO
            {
                LeagueId = l.LeagueId,
                PlayerLimit = l.PlayerLimit,
                RandomizedDraftOrder = l.RandomizedDraftOrder,
                UsersVetoTrades = l.UsersVetoTrades,
                LeagueName = l.LeagueName,
                RequiredFullToStart = l.RequiredFullToStart,
                LeagueUsers = l.LeagueUsers.Select(lu => new LeagueUserSafeExportDTO
                {
                    LeagueUserId = lu.LeagueUserId,
                    LeagueId = lu.LeagueId,
                    UserId = lu.UserId,
                    UserProfile = new UserProfileSafeExportDTO
                    {
                        Id = lu.UserProfile.Id,
                        UserName = lu.UserProfile.IdentityUser.UserName
                    }
                }).ToList()
            })
            .ToList();
        
        return Ok(NotFullLeagues);
    }
}