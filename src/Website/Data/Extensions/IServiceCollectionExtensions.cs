﻿using Microsoft.Extensions.DependencyInjection;
using Website.Data.Repositories;

namespace Website.Data.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<UsersRepository>();
            services.AddScoped<ImagesRepository>();
            services.AddScoped<ProductsRepository>();
            services.AddScoped<BranchesRepository>();
            services.AddScoped<VersionsRepository>();
            services.AddScoped<SellersRepository>();
            services.AddScoped<OrdersRepository>();
            services.AddScoped<MessagesRepository>();
            services.AddScoped<AdminRepository>();
        }
    }
}
