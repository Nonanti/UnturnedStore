﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Client.Services;
using Website.Shared.Models;

namespace Website.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public StorageService StorageService { get; set; }

        public IEnumerable<ProductModel> Products { get; set; }

        private IEnumerable<ProductModel> SearchedProducts => Products
            .Where(x => string.IsNullOrEmpty(searchCategory) || x.Category == searchCategory)
            .Where(x => string.IsNullOrEmpty(searchString) 
            || x.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || x.Seller.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));

        private IEnumerable<ProductModel> OrderedProducts {
            get
            {
                switch (orderBy)
                {
                    case EOrderBy.MostDownloads:
                        return SearchedProducts.OrderByDescending(x => x.TotalDownloadsCount);
                    case EOrderBy.BestRated:
                        return SearchedProducts.OrderByDescending(x => x.AverageRating).ThenByDescending(x => x.RatingsCount);
                    case EOrderBy.PriceAsc:
                        return SearchedProducts.OrderBy(x => x.Price);
                    case EOrderBy.PriceDesc:
                        return SearchedProducts.OrderByDescending(x => x.Price);
                    default:
                        return SearchedProducts.OrderByDescending(x => x.CreateDate);
                }
            }
        }

        private string searchString = string.Empty;
        private string searchCategory = string.Empty;

        private void ChangeCategory(string category)
        {
            searchCategory = category;
        }

        private string Active(string category)
        {
            if (searchCategory == category)
                return "active";
            return string.Empty;
        }

        private string GetCategoryIcon()
        {
            if (searchCategory == "Rocket Plugin")
            {
                return "fas fa-rocket";
            } else if (searchCategory == "OpenMod Plugin")
            {
                return "fas fa-plug";
            } else
            {
                return "far fa-folder";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Products = await HttpClient.GetFromJsonAsync<ProductModel[]>("api/products");
        }
            
        private EOrderBy orderBy = EOrderBy.Newest;

        public enum EOrderBy
        {
            Newest,
            MostDownloads,
            BestRated,
            PriceAsc,
            PriceDesc
        }
    }
}
