namespace Authentication.Application.Contracts.ExternalServicesContract;

public interface ISendEmail
{
    Task<Result<EmailResponse>>  SendEmailAsync(EmailMessage request);
}