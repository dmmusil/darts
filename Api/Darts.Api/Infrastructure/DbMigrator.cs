using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Darts.Api.Infrastructure
{
    public class DbMigrator
    {
        public DbMigrator(IEnumerable<DbContext> contexts)
        {
            Contexts = contexts;
        }
            
        public IEnumerable<DbContext> Contexts { get; }
    }
}
