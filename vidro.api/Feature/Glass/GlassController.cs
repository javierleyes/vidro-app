using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidro.api.Feature.Glass.Model;
using vidro.api.Feature.Glass.Patch.Model;
using vidro.api.Persistance;

namespace vidro.api.Feature.Glass
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlassController(VidroContext vidroContext, IValidator<UpdateGlassPriceWriteModel> updateGlassPriceValidator) : ControllerBase
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
                    Order = g.Order,
                    Name = g.Name,
                    PriceTransparent = g.PriceTransparent,
                    PriceColor = g.PriceColor,
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return Ok(glasses);
        }

        [HttpPatch]
        [Route("/glasses/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GlassReadModel>> UpdateGlassPriceAsync(Guid id, [FromBody] UpdateGlassPriceWriteModel request, CancellationToken cancellationToken)
        {
            var validationResult = await updateGlassPriceValidator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request");
            }

            var glass = await vidroContext.Glasses
                .Where(g => g.Id == id && !g.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (glass is null)
            {
                return NotFound(GlassError.GlassNotFound.ToString());
            }

            glass.PriceTransparent = request.PriceTransparent;
            glass.PriceColor = request.PriceColor;
            glass.ModifyDate = DateTime.UtcNow;

            await vidroContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var response = new GlassReadModel
            {
                Id = glass.Id,
                Order = glass.Order,
                Name = glass.Name,
                PriceTransparent = glass.PriceTransparent,
                PriceColor = glass.PriceColor,
            };

            return Ok(response);
        }
    }
}
