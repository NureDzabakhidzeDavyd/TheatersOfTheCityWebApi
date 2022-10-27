using FluentValidation;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Validators.ParticipantsRequestsValidators;

public class CreateParticipantRequestValidator : AbstractValidator<CreateParticipantRequest>
{
    public CreateParticipantRequestValidator()
    {
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.PerformanceId).NotEmpty();
        RuleFor(x => x.Role).MaximumLength(30).NotEmpty();
    }
}