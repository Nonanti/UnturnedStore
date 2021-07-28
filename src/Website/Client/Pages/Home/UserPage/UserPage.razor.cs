﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Client.Services;
using Website.Shared.Constants;
using Website.Shared.Models;

namespace Website.Client.Pages.Home.UserPage
{
    public partial class UserPage
    {
        [Parameter]
        public int UserId { get; set; }
        
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public UserService UserService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public MUser User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User = await HttpClient.GetFromJsonAsync<MUser>($"api/users/{UserId}/profile");
            if (User != null && RoleConstants.AdminAndSeller.Contains(User.Role))
            {
                User.Products = await HttpClient.GetFromJsonAsync<List<MProduct>>($"api/products/user/{UserId}");
            }
        }
        
        private void GoToProduct(MProduct product)
        {
            NavigationManager.NavigateTo($"/products/{product.Id}");
        }

        private enum EUserTab
        {
            Biography,
            Products
        }

        private EUserTab currentTab = EUserTab.Biography;

        private void ChangeTab(EUserTab tab)
        {
            currentTab = tab;
        }

        private string TabClass(EUserTab tab)
        {
            if (currentTab == tab)
                return "active";
            return string.Empty;
        }
    }
}