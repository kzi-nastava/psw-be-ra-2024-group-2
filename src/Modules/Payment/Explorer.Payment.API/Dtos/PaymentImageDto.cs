namespace Explorer.Payment.API.Dtos;

public class PaymentImageDto
{
    public required string Data { get; set; }
    public required DateTime UploadedAt { get; set; }
    public required string MimeType { get; set; }
}