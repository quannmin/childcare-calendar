using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.ChildrenRecords
{
    public partial class Create
    {

        [SupplyParameterFromForm(FormName = "ChildrenRecordCreate")]
        private ChildrenRecordCreateViewModel CreateModel { get; set; } = new();

        [Inject]
        private IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private string ErrorMessage = "";
        private async Task HandleCreate()
        {
            try
            {
                var createModel = Mapper.Map<ChildrenRecord>(CreateModel);
                createModel.UserId = 6;
                createModel.CreatedAt = DateTime.Now;
                await ChildrenRecordService.AddChildrenRecordAsync(createModel);
                Navigation.NavigateTo("/AccountDetail", forceLoad: true);

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }
        }
    }
}