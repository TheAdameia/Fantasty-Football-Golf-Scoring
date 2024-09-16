using Microsoft.AspNetCore.Identity;

namespace FantasyGolfball.Models;

public class Status
{
    public int StatusId { get; set; }
    public string StatusType { get; set; }
    public bool ViableToPlay { get; set; }
}