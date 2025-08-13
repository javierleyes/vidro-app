using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidro.api.Feature.Glass.Model;
using vidro.api.Persistance;

namespace vidro.api.Feature.Glass
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlassController(VidroContext vidroContext) : ControllerBase
    {
        [HttpGet]
        [Route("/glasses")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GlassReadModel>>> GetAllGlassesAsync(CancellationToken cancellationToken)
        {
            var glasses = await vidroContext.Glasses
                .Where(g => !g.IsDeleted)
                .Select(g => new GlassReadModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Price = g.Price,
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return Ok(glasses);
        }
    }
}
