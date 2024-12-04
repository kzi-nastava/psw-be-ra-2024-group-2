﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Mappers;
using Explorer.Encounters.Core.UseCases;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Encounters.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Infrastructure
{
    public static class EncountersStartup
    {

        public static IServiceCollection ConfigureEncountersModule(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EncountersProfile).Assembly);
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IEncounterService, EncounterService>();
            services.AddScoped<IUserLevelService, UserLevelService>();
        }

        private static void SetupCore(IServiceCollection services)
        {
            services.AddScoped<IEncounterRepository, EncounterRepository>();
            services.AddScoped<IUserLevelRepository, UserLevelRepository>();


            services.AddDbContext<EncountersContext>(opt =>
                opt.UseNpgsql(DbConnectionStringBuilder.Build("encounters"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "encounters")));
        }
    }
}
