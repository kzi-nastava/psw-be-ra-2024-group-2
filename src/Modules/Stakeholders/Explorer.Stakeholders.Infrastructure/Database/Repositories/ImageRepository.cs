using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class ImageRepository : CrudDatabaseRepository<Image, StakeholdersContext>, IImageRepository
{
    public ImageRepository(StakeholdersContext dbContext) : base(dbContext)
    {
    }

    public bool Exists(string data)
    {
        return DbContext.Images.Any(i => i.Data == data);
    }

    public Image? GetByData(string data)
    {
        return DbContext.Images.FirstOrDefault(i => i.Data == data);
    }
    public Image Get(int id)
    {
        return base.Get((long)id);
    }
}
