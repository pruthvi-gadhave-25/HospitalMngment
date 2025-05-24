using HospitalManagement.Helpers;
using Microsoft.Extensions.Options;

namespace HospitalManagement.Models
{
    public class LeaveManagment
    {
       
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate{ get; set; }       

        public bool IsApproved  { get; set; }
        public string ApprovedBy { get; set; }

        public LeaveStatus Status { get; set; }
    }
}
