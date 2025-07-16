using FluentValidation;
using vidro.api.Feature.Visit.Create.Model;

namespace vidro.api.Feature.Visit.Create
{
    public class ValidationCollection
    {
        public class CreateVisitRequestValidator : AbstractValidator<CreateVisitWriteModel>
        {
            public CreateVisitRequestValidator()
            {
                RuleFor(x => x.Date)
                    .NotEmpty()
                    .WithMessage(VisitErrors.DateIsRequired.ToString())
                    .Must(date => date.Day >= DateTimeOffset.Now.Day)
                    .WithMessage(VisitErrors.DateCannotBeInThePast.ToString());

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage(VisitErrors.NameIsRequired.ToString())
                    .MaximumLength(20)
                    .WithMessage(VisitErrors.NameCannotExceedLength.ToString());

                RuleFor(x => x.Address)
                    .NotEmpty()
                    .WithMessage(VisitErrors.AddressIsRequired.ToString())
                    .MaximumLength(50)
                    .WithMessage(VisitErrors.AddressCannotExceedLength.ToString());

                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .WithMessage(VisitErrors.PhoneIsRequired.ToString());
            }
        }
    }
}
