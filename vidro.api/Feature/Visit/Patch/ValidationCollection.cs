using FluentValidation;
using vidro.api.Feature.Visit.Patch.Model;
using vidro.api.Status;

namespace vidro.api.Feature.Visit.Patch
{
    public class ValidationCollection
    {
        public class UpdateVisitRequestValidator : AbstractValidator<UpdateVisitWriteModel>
        {
            public UpdateVisitRequestValidator()
            {
                RuleFor(x => x.Status)
                    .Equal(VisitStatus.Completed)
                    .WithMessage(VisitErrors.InvalidStatus.ToString());
            }
        }
    }
}
