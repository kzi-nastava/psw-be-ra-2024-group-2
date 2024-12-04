using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using FluentResults;

namespace Explorer.Payment.API.Public.Tourist;

public interface IBundleService
{
    Result<BundleDto> CreateBundle(long authorId, BundleDto bundle);
    Result<BundleDto> DeleteBundle(long authorId, long bundleId);
    Result<BundleDto> PublishBundle(long authorId, long bundleId);
    Result<PagedResult<FullBundleDto>> GetAll();
    Result<PagedResult<FullBundleDto>> GetMyBundles(int authorId);
    Result<BundleDto> UpdateBundle(long authorId, long bundleId, BundleDto bundle);
}
