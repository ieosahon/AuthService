namespace Authentication.Application.DataValidation;

public class LoginRequestValidation : AbstractValidator<LoginDto>
{
    public LoginRequestValidation()
    {
        RuleFor(x => x.Password)
            .MinimumLength(10).WithMessage("Password must be at least 10 characters long.")
            .Must(password => Regex.IsMatch(password, @"[\W_]")).WithMessage("Password must contain at least one special character.")
            .Must(password => Regex.IsMatch(password, @"[A-Z]")).WithMessage("Password must contain at least one uppercase letter.")
            .Must(password => Regex.IsMatch(password, @"[a-z]")).WithMessage("Password must contain at least one lowercase letter.")
            .Must(password => Regex.IsMatch(password, @"[0-9]")).WithMessage("Password must contain at least one number.");

        RuleFor(x => x.AccountNumber)
            .Matches(@"^\d{10}$").WithMessage("Account number must be 10 digits.");
    }
    
}