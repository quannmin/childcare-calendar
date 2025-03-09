using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Web;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
	public partial class Index
	{
		[Inject]
		private ISpecialityService SpecialityService { get; set; } = default!;
		[Inject]
		private IMapper Mapper { get; set; } = default!;
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;
        private List<SpecialityViewModel> Specialities = new();
		[SupplyParameterFromForm]
		private SpecialitySearchViewModel SearchData { get; set; } = new();

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
            await LoadSpecialities();
		}

		private async Task LoadSpecialities()
		{
            var (specialities, totalCount) = await SpecialityService.GetPagedSpecialitiesAsync(CurrentPage, PageSize, SearchData.Keyword);
            Specialities = Mapper.Map<List<SpecialityViewModel>>(specialities);
            TotalItems = totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
            StateHasChanged();
		}

        private async Task HandleSearch()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/specialities?page={CurrentPage}&search={SearchData.Keyword}", forceLoad: false);
            await LoadSpecialities();
        }
        private async Task ChangePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages && newPage != CurrentPage)
            {

                CurrentPage = newPage;
                await LoadSpecialities();
                Navigation.NavigateTo($"/specialities?page={CurrentPage}", forceLoad: false);


            }
        }

        private async void ConfirmDelete(int id)
		{
			idToDelete = id;
			await JS.InvokeVoidAsync("showDeleteModal");
		}

		private async Task DeleteSpeciality()
		{
			if (idToDelete.HasValue)
			{
				await SpecialityService.DeleteSpecialityAsync(idToDelete.Value);
				idToDelete = null;
				await LoadSpecialities();
			}
			await JS.InvokeVoidAsync("hideDeleteModal");
		}
	}
}