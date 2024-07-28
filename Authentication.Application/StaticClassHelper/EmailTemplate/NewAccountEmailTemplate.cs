namespace Authentication.Application.StaticClassHelper.EmailTemplate;

public class NewAccountEmailTemplate
{
    public static string ConfirmEmail(string firstName, string accountNumber, string token, string accountType)
    {
        return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Account Opening Notification</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            background-color: #f9f9f9;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 0.9em;
            color: #555;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p>Dear {firstName},</p>
        <p>An account with account number <strong>{accountNumber}</strong> and account type <strong>{accountType}</strong> has been opened for you.</p>
        <p>Please use this code <strong>{token}</strong> to verify your email. The code will expire by <strong>{DateTime.UtcNow.AddHours(1).AddMinutes(10)}</strong>.</p>
        <div class=""footer"">
            <p>Best regards,</p>
            <p>The People's Bank.</p>
        </div>
    </div>
</body>
</html>
";
    }
}