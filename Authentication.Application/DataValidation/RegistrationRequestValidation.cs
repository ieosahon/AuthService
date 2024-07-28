namespace Authentication.Application.DataValidation;

public class RegistrationRequestValidation : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidation()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.FirstName)
            .Matches(@"^[a-zA-Z\-]+$").WithMessage("First name must not contain emojis or special characters.");

        RuleFor(x => x.LastName)
            .Matches(@"^[a-zA-Z\-]+$").WithMessage("Last name must not contain emojis or special characters.");
        
        RuleFor(x => x.Title)
            .Matches(@"^[a-zA-Z\-]+$").WithMessage("Title must not contain emojis or special characters.");

        RuleFor(x => x.MiddleName)
            .Matches(@"^[a-zA-Z\-]*$").WithMessage("Middle name must not contain emojis or special characters.")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));

        RuleFor(x => x.NIN)
            .Matches(@"^\d{11}$").WithMessage("NIN must be 11 digits.")
            .When(x =>!string.IsNullOrWhiteSpace(x.NIN));

        RuleFor(x => x.BVN)
            .Matches(@"^\d{11}$").WithMessage("BVN must be 11 digits.")
            .When(x => x.HasBVN);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+234\d{10}$").WithMessage("Phone number must start with +234 and be 13 digits.")
            .When(x => x.PhoneNumber.StartsWith("+"))
            .Matches(@"^0\d{10}$").WithMessage("Phone number must start with 0 and be 11 digits.")
            .When(x => !x.PhoneNumber.StartsWith("+"));

        RuleFor(x => x.Email)
            .Must(Helper.IsValidEmail).WithMessage("Email is not valid.");

        RuleFor(x => x.DOB)
            .Must(Helper.BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old.");

        RuleFor(x => x.Password)
            .MinimumLength(10).WithMessage("Password must be at least 10 characters long.")
            .Must(password => Regex.IsMatch(password, @"[\W_]")).WithMessage("Password must contain at least one special character.")
            .Must(password => Regex.IsMatch(password, @"[A-Z]")).WithMessage("Password must contain at least one uppercase letter.")
            .Must(password => Regex.IsMatch(password, @"[a-z]")).WithMessage("Password must contain at least one lowercase letter.")
            .Must(password => Regex.IsMatch(password, @"[0-9]")).WithMessage("Password must contain at least one number.");


        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.");
        
        RuleFor(x => x.LandMark)
            .NotEmpty().WithMessage("Address is required.");
        
        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Address is required.");
        
        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Account type is required.")
            .Must(accountType => Enum.IsDefined(typeof(AccountTypeEnum), accountType))
            .WithMessage("Invalid account type. Must be 'Savings' or 'Current'.");

        RuleFor(x => x.LGAId)
            .NotEmpty().WithMessage("LGAId must be provided.");
    }
}