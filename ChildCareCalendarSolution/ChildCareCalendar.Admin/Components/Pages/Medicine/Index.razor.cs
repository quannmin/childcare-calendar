using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Medicine;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq.Expressions;
using ChildCareCalendar.Domain.Entities;
using System.Web;

namespace ChildCareCalendar.Admin.Components.Pages.Medicine
{
    public partial class Index
    {
        [Inject]
        private IMedicineService MedicineService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;
        private List<MedicineViewModel> Medicines = new();
        [SupplyParameterFromForm]
        private MedicineSearchViewModel SearchData { get; set; } = new();

        [Inject]
        private IJSRuntime JS { get; set; }

        private int? idToDelete;
        private int CurrentPage = 1;
        private int PageSize = 10;
        private int TotalPages = 1;
        private int TotalItems = 0;

        protected override async Task OnInitializedAsync()
        {
            // Đọc tham số truy vấn từ URL
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);
            var page = queryParameters["page"];

            if (int.TryParse(page, out int pageNumber) && pageNumber > 0)
            {
                CurrentPage = pageNumber;
            }
            else
            {
                CurrentPage = 1;
            }

            await LoadMedicines();
        }



        private async Task LoadMedicines()
        {
            Console.WriteLine($"Loading data for page: {CurrentPage}");
            var (medicines, totalCount) = await MedicineService.GetPagedMedicinesAsync(CurrentPage, PageSize, SearchData.Keyword);
            Medicines = Mapper.Map<List<MedicineViewModel>>(medicines);
            TotalItems = totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        }


        private async Task HandleSearch()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/medicines?page={CurrentPage}/search={SearchData.Keyword}", forceLoad: false);
            await LoadMedicines();
        }

        private async Task ChangePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages && newPage != CurrentPage)
            {
                
                CurrentPage = newPage;
                await LoadMedicines();
                Navigation.NavigateTo($"/medicines?page={CurrentPage}", forceLoad: false);              
               
                
            }
        }

        private async void ConfirmDelete(int id)
        {
            idToDelete = id;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        private async Task DeleteMedicine()
        {
            if (idToDelete.HasValue)
            {
                await MedicineService.DeleteMedicineAsync(idToDelete.Value);
                idToDelete = null;
                await LoadMedicines();
            }
            await JS.InvokeVoidAsync("hideDeleteModal");
        }
    }
}