using DynamicObjectAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DynamicObjectAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<DynamicObject> DynamicObjects { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var valueComparer = new ValueComparer<Dictionary<string, object>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            );

            modelBuilder.Entity<DynamicObject>()
                .Property(d => d.Fields)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v), 
                    v => JsonConvert.DeserializeObject<Dictionary<string, object>>(v), 
                    new ValueComparer<Dictionary<string, object>>(
                        (d1, d2) => d1.SequenceEqual(d2),
                        d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        d => d.ToDictionary(entry => entry.Key, entry => entry.Value)
                    )
                )
                .Metadata.SetValueComparer(valueComparer);
        }
    }
}
