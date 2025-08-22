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
                When(x => x.PriceTransparent.HasValue, () =>
                {
                    RuleFor(x => x.PriceTransparent)
                        .GreaterThan(0)
                        .WithMessage(GlassError.InvalidPrice.ToString());
                });

                When(x => x.PriceColor.HasValue, () =>
                {
                    RuleFor(x => x.PriceColor)
                        .GreaterThan(0)
                        .WithMessage(GlassError.InvalidPrice.ToString());
                });
            }
        }
    }
}