using FluentValidation;
using TheatersOfTheCity.Api.Validators.ParticipantsRequestsValidators;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Validators.PerformancesRequestsValidators;

public class CreatePerformanceRequestValidator : AbstractValidator<CreatePerformanceRequest>
{
    public CreatePerformanceRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Genre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Language).MaximumLength(30);
        RuleForEach(x => x.Participants).SetValidator(new CreatePerformanceParticipantRequestValidator());
    }
}