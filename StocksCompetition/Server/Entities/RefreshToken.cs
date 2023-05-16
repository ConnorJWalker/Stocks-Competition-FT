using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksCompetition.Server.Entities;

public class RefreshToken
{
    [ForeignKey("UserId")]
    public string UserId { get; set; }
    [Key]
    public Guid Token { get; set; }
    public DateTimeOffset ValidTo { get; set; }
    
    public ApplicationUser? User { get; set; }
    
    public RefreshToken() { }
    
    public RefreshToken(string userId)
    {
        UserId = userId;
        Token = Guid.NewGuid();
        ValidTo = DateTimeOffset.Now.AddMinutes(20);
    }
}