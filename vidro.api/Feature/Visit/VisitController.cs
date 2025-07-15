using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vidro.api.Feature.Visit.Create.Model;
using vidro.api.Feature.Visit.Model;
using vidro.api.Persistance;

namespace vidro.api.Feature.Visit
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private readonly VidroContext _context;
        private readonly IValidator<CreateVisitWriteModel> _createVisitValidator;

        public VisitController(VidroContext context, IValidator<CreateVisitWriteModel> createVisitValidator)
        {
            _context = context;
            _createVisitValidator = createVisitValidator;
        }

        [HttpGet]
        [Route("/visits/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VisitReadModel>> GetVisitByIdAsync(int id, CancellationToken cancellationToken)
        {
            var visit = await _context.Visits
                .Where(v => v.Id == id && !v.IsDeleted)
                .Select(v => new VisitReadModel
                {
                    Id = v.Id,
                    Date = v.Date,
                    Address = v.Address,
                    Name = v.Name,
                    Phone = v.Phone,
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
        public async Task<ActionResult<IEnumerable<VisitReadModel>>> GetAllVisitsAsync(CancellationToken cancellationToken)
        {
            var visits = await _context.Visits
                .Where(v => !v.IsDeleted)
                .Select(v => new VisitReadModel
                {
                    Id = v.Id,
                    Date = v.Date,
                    Address = v.Address,
                    Name = v.Name,
                    Phone = v.Phone,
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
            var validationResult = await _createVisitValidator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
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

            await _context.Visits.AddAsync(visit, cancellationToken).ConfigureAwait(false);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

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
    }
}
