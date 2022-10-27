using FluentValidation;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Validators.ParticipantsRequestsValidators;

public class CreatePerformanceParticipantRequestValidator : AbstractValidator<CreatePerformanceParticipantRequest>
{
    public CreatePerformanceParticipantRequestValidator()
    {
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.Role).MaximumLength(30).NotEmpty();
    }
}