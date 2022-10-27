using FluentValidation;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Validators.ParticipantsRequestsValidators;

public class UpdateParticipantRequestValidator : AbstractValidator<UpdateParticipantRequest>
{
    public UpdateParticipantRequestValidator()
    {
        RuleFor(x => x.PerformanceId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.Role).NotEmpty().MaximumLength(30);

    }
}