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
                    .WithMessage(VisitError.DateIsRequired.ToString())
                    .Must(date => date.Day >= DateTimeOffset.Now.Day)
                    .WithMessage(VisitError.DateCannotBeInThePast.ToString());

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage(VisitError.NameIsRequired.ToString())
                    .MaximumLength(20)
                    .WithMessage(VisitError.NameCannotExceedLength.ToString());

                RuleFor(x => x.Address)
                    .NotEmpty()
                    .WithMessage(VisitError.AddressIsRequired.ToString())
                    .MaximumLength(50)
                    .WithMessage(VisitError.AddressCannotExceedLength.ToString());

                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .WithMessage(VisitError.PhoneIsRequired.ToString());
            }
        }
    }
}
