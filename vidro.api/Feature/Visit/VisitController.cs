using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidro.api.Feature.Visit.Create.Model;
using vidro.api.Feature.Visit.Model;
using vidro.api.Feature.Visit.Patch.Model;
using vidro.api.Persistance;
using vidro.api.Status;

namespace vidro.api.Feature.Visit
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController(VidroContext vidroContext, IValidator<CreateVisitWriteModel> createVisitValidator, IValidator<UpdateVisitWriteModel> updateVisitValidator) : ControllerBase
    {
        [HttpGet]
        [Route("/visits/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VisitReadModel>> GetVisitByIdAsync(int id, CancellationToken cancellationToken)
        {
            var visit = await vidroContext.Visits
                .Where(v => v.Id == id && !v.IsDeleted)
                .Select(v => new VisitReadModel
                {
                    Id = v.Id,
                    Date = v.Date,
                    Address = v.Address,
                    Name = v.Name,
                    Phone = v.Phone,
                    Status = v.Status,
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (visit == null)
            {
                return NotFound();
            }

            return Ok(visit);
        }

        [HttpGet]
        [Route("/visits")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VisitReadModel>>> GetAllVisitsAsync([FromQuery] VisitStatus? status, CancellationToken cancellationToken)
        {
            var query = vidroContext.Visits.Where(v => !v.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(v => v.Status == status.Value);
            }

            var visits = await query
                .Select(v => new VisitReadModel
                {
                    Id = v.Id,
                    Date = v.Date,
                    Address = v.Address,
                    Name = v.Name,
                    Phone = v.Phone,
                    Status = v.Status,
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return Ok(visits);
        }

        [HttpPost]
        [Route("/visits")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateVisitReadModel>> CreateVisitAsync([FromBody] CreateVisitWriteModel request, CancellationToken cancellationToken)
        {
            var validationResult = await createVisitValidator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request");
            }

            var visit = new Domain.Visit
            {
                Date = request.Date,
                Address = request.Address,
                Name = request.Name,
                Phone = request.Phone
            };

            await vidroContext.Visits.AddAsync(visit, cancellationToken).ConfigureAwait(false);
            await vidroContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var response = new CreateVisitReadModel
            {
                Id = visit.Id,
                Date = visit.Date,
                Address = visit.Address,
                Name = visit.Name,
                Phone = visit.Phone,
            };

            return Created($"/visits/{response.Id}", response);
        }

        [HttpPatch]
        [Route("/visits/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VisitReadModel>> UpdateVisitStatusAsync(int id, [FromBody] UpdateVisitWriteModel request, CancellationToken cancellationToken)
        {
            var validationResult = await updateVisitValidator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request");
            }

            var visit = await vidroContext.Visits
                .Where(v => v.Id == id && !v.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (visit is null)
            {
                return NotFound();
            }

            visit.Status = request.Status;

            await vidroContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var response = new VisitReadModel
            {
                Id = visit.Id,
                Date = visit.Date,
                Address = visit.Address,
                Name = visit.Name,
                Phone = visit.Phone,
                Status = visit.Status,
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("/visits/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVisitAsync(int id, CancellationToken cancellationToken)
        {
            var visit = await vidroContext.Visits
                .Where(v => v.Id == id && !v.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (visit is null)
            {
                return NotFound();
            }

            visit.IsDeleted = true;

            await vidroContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return NoContent();
        }
    }
}
