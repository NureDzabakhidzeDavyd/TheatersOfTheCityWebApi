using FluentValidation;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Validators.TheatersRequestsValidators;

public class CreateTheaterRequestValidator : AbstractValidator<CreateTheaterRequest>
{
    public CreateTheaterRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.City).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DirectorId).NotEmpty();
    }
}