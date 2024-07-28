namespace Authentication.Application.StaticClassHelper;

public static class Helper
{
    // A single Random instance to be shared across methods
    private static readonly Random GlobalRandom = new();

    // Lock object to ensure thread safety
    private static readonly object GlobalLock = new();

    /// <summary>
    /// Generates a random 11-digit number as a string.
    /// </summary>
    /// <returns>A random 11-digit number.</returns>
    public static string GenerateRandom11DigitNumber()
    {
        var digits = new char[11];
        lock (GlobalLock)
        {
            for (var i = 0; i < 11; i++)
            {
                digits[i] = (char)('0' + GlobalRandom.Next(0, 10));
            }
        }

        return new string(digits);
    }

    /// <summary>
    /// Generates a random 10-digit number as a string, with the first two digits the same.
    /// </summary>
    /// <returns>A random 10-digit number with the first two digits the same.</returns>
    public static string GenerateRandom10DigitNumber()
    {
        const int firstTwoDigits = 33;
        int remainingDigits;
        // Lock to ensure thread safety for Random instance
        lock (GlobalLock)
        {
            // Generate 8-digit number where the last digit is fixed to ensure a total of 10 digits
            remainingDigits = GlobalRandom.Next(10000000, 100000000); // [10000000, 100000000)
        }

        // Combine the fixed two-digit prefix with the 8-digit suffix to form a 10-digit number
        return firstTwoDigits + remainingDigits.ToString();
    }

    public static string HashString(string input)
    {
        using var sha512 = SHA512.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha512.ComputeHash(inputBytes);
        var hashedString = ConvertToHexString(hashBytes);
        return hashedString;
    }

    /// <summary>
    /// Converts a byte array to a hexadecimal string.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <returns>A hexadecimal string representation of the byte array.</returns>
    private static string ConvertToHexString(IEnumerable<byte> bytes)
    {
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public static string GenerateJwtToken(string email, string firstName, string lastName, IConfiguration config)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.Jti, DateTime.Now.Ticks.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(config["Jwt:ExpireMinutes"]));

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Issuer"],
            claims,
            expires: expires,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static bool BeAtLeast18YearsOld(DateTime dob)
    {
        var today = DateTime.Today;
        var age = today.Year - dob.Year;

        if (dob.Date > today.AddYears(-age)) age--;

        return age >= 18;
    }

    public static int Age(DateTime dob)
    {
        var today = DateTime.Today;
        var age = today.Year - dob.Year;
        // Check if the birthday has not occurred yet this year
        if (dob.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                RegexOptions.None, TimeSpan.FromMilliseconds(200));
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static string DomainMapper(Match match)
    {
        var idn = new IdnMapping();
        var domainName = match.Groups[2].Value;
        domainName = idn.GetAscii(domainName);
        return match.Groups[1].Value + domainName;
    }
}