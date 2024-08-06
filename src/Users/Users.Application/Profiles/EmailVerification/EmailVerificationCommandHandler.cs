//using ErrorOr;
//using MediatR;
//using User.Domain.Contracts;
//using User.Domain.Errors;
//using User.Domain.Time;

//namespace User.Application.Profiles.EmailVerification;

//public sealed class EmailVerificationCommandHandler :
//    IRequestHandler<EmailVerificationCommand, ErrorOr<RegistrationAggregate>>
//{
//    private readonly IUserAggregateRepository _registrationRepository;
//    private readonly ISystemTimeProvider _systemTimeProvider;

//    public EmailVerificationCommandHandler(
//        IUserAggregateRepository registrationRepository,
//        ISystemTimeProvider systemTimeProvider)
//    {
//        _registrationRepository = registrationRepository;
//        _systemTimeProvider = systemTimeProvider;
//    }

//    public async Task<ErrorOr<RegistrationAggregate>> Handle(EmailVerificationCommand command, CancellationToken cancellationToken)
//    {
//        var registrationResult = await _registrationRepository.GetByIdAsync(command.RegistrationId);

//        if (registrationResult is null)
//            return RegistrationAggregateErrors.NotFound(command.RegistrationId);

//        var emailVerificationResult = registrationResult.VerifyEmail(command.Email, _systemTimeProvider);

//        if (emailVerificationResult.IsError)
//            return emailVerificationResult.Errors;

//        await _registrationRepository.UpdateAsync(emailVerificationResult.Value);

//        return emailVerificationResult;
//    }
//}