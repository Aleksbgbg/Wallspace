namespace Wallspace.Models.Database
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class WallspaceDbContext : IdentityDbContext<WallspaceUser>
    {
        public WallspaceDbContext(DbContextOptions<WallspaceDbContext> options) : base(options)
        {
        }
    }
}