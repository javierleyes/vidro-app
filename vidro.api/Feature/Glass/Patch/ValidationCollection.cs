using FluentValidation;
using vidro.api.Feature.Glass.Patch.Model;

namespace vidro.api.Feature.Glass.Patch
{
    public class ValidationCollection
    {
        public class UpdateGlassPriceRequestValidator : AbstractValidator<UpdateGlassPriceWriteModel>
        {
            public UpdateGlassPriceRequestValidator()
            {
                RuleFor(x => x.Price)
                    .GreaterThan(0)
                    .WithMessage(GlassError.InvalidPrice.ToString());
            }
        }
    }
}