using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Infrastructure.Database;

public class ImageRepository<TContext> : CrudDatabaseRepository<Image, TContext>, IImageRepository
       where TContext : DbContext
    {
        public ImageRepository(TContext dbContext) : base(dbContext)
        {
        }

        public bool Exists(string data)
        {
            return false;
        }

        public Image? GetByData(string data)
        {
            return DbContext.Set<Image>().FirstOrDefault(i => i.Data == data);
        }
    }

