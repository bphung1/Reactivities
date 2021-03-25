using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reactivities.Persistence
{
    public class ReactivitiesDbContext : DbContext
    {
        public ReactivitiesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
    }
}