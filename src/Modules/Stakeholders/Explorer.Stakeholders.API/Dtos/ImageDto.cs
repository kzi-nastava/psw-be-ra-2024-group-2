namespace Explorer.Stakeholders.API.Dtos;

public class ImageDto
{
    public required string Data { get; set; }
    public required DateTime UploadedAt { get; set; }
    public required string MimeType { get; set; }
}