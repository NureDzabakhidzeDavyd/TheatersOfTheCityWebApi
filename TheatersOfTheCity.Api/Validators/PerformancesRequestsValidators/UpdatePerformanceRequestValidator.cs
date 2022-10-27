using FluentValidation;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Validators.PerformancesRequestsValidators;

public class UpdatePerformanceRequestValidator : AbstractValidator<UpdatePerformanceRequest>
{
    public UpdatePerformanceRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Genre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Language).MaximumLength(30);
    }
}