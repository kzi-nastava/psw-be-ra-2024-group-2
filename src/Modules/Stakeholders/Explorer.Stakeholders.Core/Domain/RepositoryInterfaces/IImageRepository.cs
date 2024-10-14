using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;

public interface IImageRepository
{
    bool Exists(string data);
    Image? GetByData(string data);
    Image Create(Image image);
}