using System;
using System.Collections.Generic;
using pets.Models;

namespace pets.ViewModels
{
    public class AppointmentsIndexVm
    {
        public int TodayCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int PendingCount { get; set; }
        public int CancelledCount { get; set; }
        public int TotalCount { get; set; }

        public List<AppointmentRowVm> Items { get; set; } = new();
        public List<CalendarEventVm> CalendarEvents { get; set; } = new();
    }

    public class AppointmentRowVm
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = "";
        public string PetName { get; set; } = "";
        public DateTime StartAt { get; set; }   // ✅ لازم عشان Index.cshtml
        public string DoctorName { get; set; } = "";
        public string VisitType { get; set; } = "";
        public string Reason { get; set; } = "";
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }

    public class CalendarEventVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public string Color { get; set; } = "#10b981";
    }
}
