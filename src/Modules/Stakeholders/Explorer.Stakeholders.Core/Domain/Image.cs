using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class Image : Entity
{
    public byte[] Data { get; init; }
    public DateTime UploadedAt { get; init; }
    public MimeType MimeType { get; init; }
    public string GetMimeTypeNormalized => MimeType switch
    {
        MimeType.Jpeg => "image/jpeg",
        MimeType.Png => "image/png",
        MimeType.Gif => "image/gif",
        _ => throw new ArgumentException("MimeType is not valid")
    };

    public Image(byte[] data, DateTime uploadedAt, MimeType mimeType)
    {
        Data = data;
        UploadedAt = uploadedAt;
        MimeType = mimeType;
        Validate();
    }

    public Image(byte[] data, DateTime uploadedAt, string mimeType)
    {
        Data = data;
        UploadedAt = uploadedAt;
        MimeType = GetMimeTypeDenormalized(mimeType);
        Validate();
    }

    private MimeType GetMimeTypeDenormalized(string mimeType) => mimeType switch
    {
        "image/jpeg" => MimeType.Jpeg,
        "image/png" => MimeType.Png,
        "image/gif" => MimeType.Gif,
        _ => throw new ArgumentException("MimeType is not valid")
    };

    private void Validate()
    {
        if (Data.Length == 0) throw new ArgumentException("Data cannot be empty");
        if (UploadedAt == default) throw new ArgumentException("UploadedAt cannot be empty");
        if (!Enum.IsDefined(typeof(MimeType), MimeType)) throw new ArgumentException("MimeType is not valid");
    }
}

public enum MimeType
{
    Jpeg,
    Png,
    Gif
}