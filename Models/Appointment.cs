using System.ComponentModel.DataAnnotations;

namespace pets.Models
{
    public enum AppointmentStatus
    {
        Pending = 0,
        Confirmed = 1,
        Completed = 2,
        Cancelled = 3
    }

    public class Appointment
    {
        public int Id { get; set; }

        public int? OwnerId { get; set; }
        public int? PetId { get; set; }

        // الدكتور = User (Identity)
        public string? DoctorUserId { get; set; }

        [Required]
        public DateTime StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        [Required, MaxLength(200)]
        public string Reason { get; set; } = "";

        [MaxLength(2000)]
        public string? Notes { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [MaxLength(200)]
        public string OwnerName { get; set; } = "";

        [MaxLength(200)]
        public string PetName { get; set; } = "";

        [MaxLength(200)]
        public string DoctorName { get; set; } = "";
    }
}
