using Microsoft.EntityFrameworkCore;

namespace PMCSystem_Backend.Data
{
    public class PmcSpecRuleContext : DbContext
    {
        public DbSet<PmcSpecRuleContext> PmcSpecRules { get; set; }

        
    }
}
