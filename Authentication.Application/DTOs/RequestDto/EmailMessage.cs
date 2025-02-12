﻿namespace Authentication.Application.DTOs.RequestDto;

public class EmailMessage
{
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> Attachments { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public string Purpose { get; set; }
}