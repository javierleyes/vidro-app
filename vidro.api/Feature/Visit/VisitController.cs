using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using vidro.api.Feature.Visit.Create.Model;
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

        [HttpPost]
        [Route("/visits")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateVisitReadModel>> CreateVisit([FromBody] CreateVisitWriteModel request)
        {
            var validationResult = await _createVisitValidator.ValidateAsync(request);
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

            await _context.Visits.AddAsync(visit).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var response = new CreateVisitReadModel
            {
                Id = visit.Id,
                Date = visit.Date,
                Address = visit.Address,
                Name = visit.Name,
                Phone = visit.Phone,
            };

            return CreatedAtAction(nameof(CreateVisit), new { id = visit.Id }, response);
        }
    }
}
