using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorontoTrails.APIs.Data;
using TorontoTrails.APIs.Models.Domain;

namespace TorontoTrails.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly TorontoTrailsDbContext dbContext;

        public TrailController(TorontoTrailsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/trail
        [HttpGet]
        public async Task<IActionResult> GetAllTrails()
        {
            var trails = await dbContext.Trails
                .Include(t => t.Region)
                .Include(t => t.Difficulty)
                .ToListAsync();

            return Ok(trails);
        }

        // GET: api/trail/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTrailById(Guid id)
        {
            var trail = await dbContext.Trails
                .Include(t => t.Region)
                .Include(t => t.Difficulty)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trail == null) return NotFound();

            return Ok(trail);
        }

        // POST: api/trail
        [HttpPost]
        public async Task<IActionResult> AddTrail([FromBody] Trails trail)
        {
            trail.Id = Guid.NewGuid();
            await dbContext.Trails.AddAsync(trail);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTrailById), new { id = trail.Id }, trail);
        }

        // POST: api/trail/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTrail(Guid id, [FromBody] Trails updatedTrail)
        {
            var existing = await dbContext.Trails
                                          .Include(t => t.Difficulty)  
                                          .Include(t => t.Region)      
                                          .FirstOrDefaultAsync(t => t.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = updatedTrail.Name ?? existing.Name;
            existing.Description = updatedTrail.Description ?? existing.Description;
            existing.TrailImageURL = updatedTrail.TrailImageURL ?? existing.TrailImageURL;
            existing.LengthInMiles = updatedTrail.LengthInMiles != 0 ? updatedTrail.LengthInMiles : existing.LengthInMiles;

            if (!string.IsNullOrEmpty(updatedTrail.Difficulty?.Name))
            {
                existing.Difficulty.Name = updatedTrail.Difficulty.Name;
            }

            // update the related Region details
            if (!string.IsNullOrEmpty(updatedTrail.Region?.Name))
            {
                existing.Region.Name = updatedTrail.Region.Name;
            }
            if (!string.IsNullOrEmpty(updatedTrail.Region?.Code))
            {
                existing.Region.Code = updatedTrail.Region.Code;
            }
            if (!string.IsNullOrEmpty(updatedTrail.Region?.RegionImageURL))
            {
                existing.Region.RegionImageURL = updatedTrail.Region.RegionImageURL;
            }

            await dbContext.SaveChangesAsync();

            return NoContent();
        }



        // DELETE: api/trail/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTrail(Guid id)
        {
            var trail = await dbContext.Trails.FindAsync(id);
            if (trail == null) return NotFound();

            dbContext.Trails.Remove(trail);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("sorted-by-length")]
        public IActionResult GetTrailsSortedByLength()
        {
            var trails = dbContext.Trails
                                 .OrderBy(t => t.LengthInMiles)  
                                 .Include(t => t.Difficulty)    
                                 .Include(t => t.Region)        
                                 .ToList();

            return Ok(trails);
        }
        // SEARCH: api/trail/search?query={query}
        [HttpGet("search")]
        public async Task<IActionResult> SearchTrails([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var matchedTrails = await dbContext.Trails
                .Include(t => t.Difficulty)
                .Include(t => t.Region)
                .Where(t =>
                    t.Name.ToLower().Contains(query.ToLower()) ||
                    t.Description.ToLower().Contains(query.ToLower()))
                .ToListAsync();

            return Ok(matchedTrails);
        }

        // gET Recom based on trail experience beginner, intermediate, expert: api/trail/search?query={query}
        [HttpGet("recommendation")]
        public async Task<IActionResult> GetRecommendedTrails([FromQuery] string experience)
        {
            if (string.IsNullOrWhiteSpace(experience))
                return BadRequest("Experience level is required.");

            experience = experience.ToLower();

            IQueryable<Trails> query = dbContext.Trails
                .Include(t => t.Difficulty)
                .Include(t => t.Region);

            switch (experience)
            {
                case "beginner":
                    query = query.Where(t =>
                        t.Difficulty.Name == "Easy" &&
                        t.LengthInMiles < 3);
                    break;

                case "intermediate":
                    query = query.Where(t =>
                        t.Difficulty.Name == "Medium" &&
                        t.LengthInMiles >= 3 && t.LengthInMiles <= 6);
                    break;

                case "expert":
                    query = query.Where(t =>
                        t.Difficulty.Name == "Hard" &&
                        t.LengthInMiles > 6);
                    break;

                default:
                    return BadRequest("Invalid experience level. Use 'beginner', 'intermediate', or 'expert'.");
            }

            var recommendedTrails = await query.ToListAsync();

            return Ok(recommendedTrails);
        }



    }
}
