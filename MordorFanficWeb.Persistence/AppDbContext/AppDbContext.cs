﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MordorFanficWeb.Models;
using MordorFanficWeb.Models.BaseModels;
using System.Threading.Tasks;

namespace MordorFanficWeb.Persistence.AppDbContext
{
    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        { }

        public DbSet<AccountUser> AccountUsers { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<CompositionTags> CompositionTags { get; set; }
        public DbSet<Tags> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Tags>().HasIndex(t => t.Tag).IsUnique();
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public DbSet<T> DbSet<T>() where T : BaseEntity
        {
            return Set<T>();
        }
    }
}
