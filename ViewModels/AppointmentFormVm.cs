using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using pets.Models;

namespace pets.ViewModels
{
    public class AppointmentFormVm
    {
        public int? Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int PetId { get; set; }

        // الدكتور هو اليوسر (Identity)
        public string? DoctorUserId { get; set; }

        [Required]
        public DateTime StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        [Required, MaxLength(200)]
        public string Reason { get; set; } = "";

        [MaxLength(2000)]
        public string? Notes { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // dropdowns
        public List<SelectListItem> Owners { get; set; } = new();
        public List<SelectListItem> Pets { get; set; } = new();
        public List<SelectListItem> Doctors { get; set; } = new();
    }
}
