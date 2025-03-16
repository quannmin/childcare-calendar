using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Appointment
{
    public partial class Create
    {
        public List<AppUser> Parents = new();
        public List<ChildrenRecord> ChildrenRecords = new();
        public List<AppUser> Doctors = new();
        public List<ChildCareCalendar.Domain.Entities.Service> Services = new();

        [SupplyParameterFromForm(FormName = "AppointmentCreate")]
        private AppointmentCreateViewModel CreateModel { get; set; } = new();

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        private IServiceService ServiceService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;


        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private string ErrorMessage = "";




        protected override async Task OnInitializedAsync()
        {
            Parents = (await UserService.FindUsersAsync(u => u.Role.Equals("PhuHuynh"))).ToList();
            Doctors = (await UserService.FindUsersAsync(u => u.Role.Equals("BacSi"))).ToList();
        }

        private async Task HandleCreate()
        {
            ErrorMessage = "";
            var newSpeciality = Mapper.Map<ChildCareCalendar.Domain.Entities.Appointment>(CreateModel);
            await AppointmentService.AddAppointmentAsync(newSpeciality);
            Navigation.NavigateTo("/appointments");
        }

        private async Task LoadChildrenRecords(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int parentId))
            { 
                try
                {
                    ChildrenRecords = (await ChildrenRecordService.FindUsersAsync(cr => cr.UserId.Equals(parentId))).ToList();
                    StateHasChanged(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tải ChildrenRecords: {ex.Message}");
                }
            }
        }
    }
}