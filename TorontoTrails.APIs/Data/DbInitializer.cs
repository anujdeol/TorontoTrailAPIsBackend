using TorontoTrails.APIs.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace TorontoTrails.APIs.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(TorontoTrailsDbContext context)
        {
            // Apply any pending migrations (this ensures the DB schema is up to date)
            await context.Database.MigrateAsync();

            // Seed Difficulties
            if (!await context.Difficulties.AnyAsync())
            {
                var difficulties = new List<Difficulty>
                {
                    new Difficulty { Id = Guid.NewGuid(), Name = "Easy" },
                    new Difficulty { Id = Guid.NewGuid(), Name = "Medium" },
                    new Difficulty { Id = Guid.NewGuid(), Name = "Hard" }
                };

                await context.Difficulties.AddRangeAsync(difficulties);
            }

            // Seed Regions
            if (!await context.Regions.AnyAsync())
            {
                var regions = new List<Region>
                {
                    new Region
                    {
                        Id = Guid.NewGuid(),
                        Code = "TO",
                        Name = "Toronto",
                        RegionImageURL = "https://example.com/toronto.jpg"
                    },
                    new Region
                    {
                        Id = Guid.NewGuid(),
                        Code = "GTA",
                        Name = "Greater Toronto Area",
                        RegionImageURL = "https://example.com/gta.jpg"
                    },
                    new Region
                    {
                        Id = Guid.NewGuid(),
                        Code = "ONT",
                        Name = "Ontario",
                        RegionImageURL = "https://example.com/ontario.jpg"
                    }
                };

                await context.Regions.AddRangeAsync(regions);
            }

            await context.SaveChangesAsync();

            // Seed Trails
            if (!await context.Trails.AnyAsync())
            {
                // Fetch difficulties
                var easy = await context.Difficulties.FirstOrDefaultAsync(d => d.Name == "Easy");
                var medium = await context.Difficulties.FirstOrDefaultAsync(d => d.Name == "Medium");
                var hard = await context.Difficulties.FirstOrDefaultAsync(d => d.Name == "Hard");

                // Fetch regions
                var toronto = await context.Regions.FirstOrDefaultAsync(r => r.Name == "Toronto");
                var gta = await context.Regions.FirstOrDefaultAsync(r => r.Name == "Greater Toronto Area");
                var ontario = await context.Regions.FirstOrDefaultAsync(r => r.Name == "Ontario");

                if (easy != null && medium != null && hard != null &&
                    toronto != null && gta != null && ontario != null)
                {
                    var trails = new List<Trails>
                    {
                        new Trails
                        {
                            Id = Guid.NewGuid(),
                            Name = "High Park Trail",
                            Description = "Scenic walk through Toronto's largest park.",
                            TrailImageURL = "https://example.com/highpark.jpg",
                            LengthInMiles = 2.5,
                            DifficultyId = easy.Id,
                            RegionId = toronto.Id
                        },
                        new Trails
                        {
                            Id = Guid.NewGuid(),
                            Name = "Don Valley Trails",
                            Description = "Winding trails through Don Valley's lush forest.",
                            TrailImageURL = "https://example.com/donvalley.jpg",
                            LengthInMiles = 4.2,
                            DifficultyId = medium.Id,
                            RegionId = toronto.Id
                        },
                        new Trails
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crothers Woods",
                            Description = "Popular MTB trail with diverse terrain and forest.",
                            TrailImageURL = "https://example.com/crothers.jpg",
                            LengthInMiles = 3.1,
                            DifficultyId = medium.Id,
                            RegionId = gta.Id
                        },
                        new Trails
                        {
                            Id = Guid.NewGuid(),
                            Name = "Bruce Trail - Hamilton Section",
                            Description = "Part of Canada's longest trail with scenic escarpment views.",
                            TrailImageURL = "https://example.com/bruce-hamilton.jpg",
                            LengthInMiles = 6.5,
                            DifficultyId = hard.Id,
                            RegionId = ontario.Id
                        },
                        new Trails
                        {
                            Id = Guid.NewGuid(),
                            Name = "Scarborough Bluffs Trail",
                            Description = "Beautiful views of Lake Ontario from towering cliffs.",
                            TrailImageURL = "https://example.com/bluffs.jpg",
                            LengthInMiles = 1.8,
                            DifficultyId = easy.Id,
                            RegionId = gta.Id
                        }
                    };

                    await context.Trails.AddRangeAsync(trails);
                    await context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Unable to seed Trails. Missing Difficulty or Region.");
                }
            }
        }
    }
}
