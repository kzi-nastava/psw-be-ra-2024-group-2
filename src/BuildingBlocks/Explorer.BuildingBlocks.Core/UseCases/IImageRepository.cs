using Explorer.BuildingBlocks.Core.Domain;

public interface IImageRepository
{
    bool Exists(string data);
    Image? GetByData(string data);
    Image Create(Image image);
    Image Get(int id);
}