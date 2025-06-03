using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Interface;
using HospitalManagement.Models;
using HospitalManagement.Models.Mails;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class AppointmentService :  IAppointmentService
    {
       
        private readonly AppointmentRepository _appointmentRepository;
        private readonly DoctorRepository _doctorRepository;
        private readonly PatientRepository _patientRepository;
        private readonly LeaveManagementRepository _leaveRepo;
        private readonly DepartmentRepository _departmentRepos;
        private readonly IEmailService _emailService;

        public AppointmentService(
            AppointmentRepository appointmentRepository,
            DoctorRepository doctorRepository,
            PatientRepository patientRepository,
            LeaveManagementRepository leaveManagementRepository,
            IEmailService emailService,
            DepartmentRepository departmentRepository
            )
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _leaveRepo = leaveManagementRepository;
            _emailService = emailService;
            _departmentRepos = departmentRepository;
        }

        public async  Task<Result<bool>> BookAppointment(BookAppointmentDto appointmentDto)
        {
            var patient =  await _patientRepository.GetById(appointmentDto.PatientID);
            //var patient1 = await _patientRpos.GetById(appointmentDto.PatientID);

            var doctor = await _doctorRepository.GetById(appointmentDto.DoctorId);


            if ((patient == null) || (doctor == null))
                return Result<bool>.ErrorResult("patient or doctor is invalid");

            var isDoctorOnLeave = await _leaveRepo.IsDoctorOnLeaveAsync(appointmentDto.DoctorId , appointmentDto.AppointmentDate);
            if (isDoctorOnLeave)
            {
                return Result<bool>.ErrorResult("docotr is on leave");
            }
        
            var slots = await _doctorRepository.GetBySlotDoctorIdAsync(appointmentDto.DoctorId);
            var appointTime = appointmentDto.AppointmentTime;
            var appointDay = appointmentDto.AppointmentDate.DayOfWeek;

            var isSlotAvailable = slots.Any(slot =>
            slot.DayofWeek == appointDay &&
            slot.StartTime <= appointTime && 
            appointTime < slot.EndTime
            );


            if (!isSlotAvailable)
            {
                return  Result<bool>.ErrorResult("slot is not available");
            }

            var appointment = new Appointment
            {
                PatientID = appointmentDto.PatientID,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                AppointmentTime = appointmentDto.AppointmentTime,
                Diagnoasis = appointmentDto.Diagnoasis,
                Medications = appointmentDto.Medications,
                Treatement = appointmentDto.Treatement,
                Status = AppointmentStatus.Pending

            };

               var res =  await _appointmentRepository.BookAppointment(appointment);
            if(res)
            { 

                var emailData = new MailRequest()
                {
                    Subject = "Appointment Details",
                    ToEmail = appointment.Patient.Email ,
                    Body = $"Dear Patient\n" +
                    $"Date : {appointment.AppointmentDate} \n" +
                    $"Time : {appointment.AppointmentTime}\n" +
                    $"Doctor: {appointment.Doctor.Name}\n\n" +
                    $"Thanks"
                    
                };
             var resData =   await  _emailService.EmailSendServices(emailData);
                return Result<bool>.ErrorResult(resData.Message);
            }
            return Result < bool>.SuccessResult(res, "appointment booked succfully");            
        }

        public async Task<Result<bool>> RescheduleAppointmentAsync(RescheduleAppointmentDto dto)
        {
            var appointment = await _appointmentRepository.GetById(dto.AppointmentId);
            if (appointment == null)
                return Result<bool>.ErrorResult("appointmentId  is invlaid");


            var res =  await _appointmentRepository.RescheduleAppointment(dto.AppointmentId, dto.NewDate, dto.NewTime);
            return Result<bool>.SuccessResult(res, "rescheduled appoint succefully");
        }

        public async Task<Result<bool>> CancelApppointment(int appointmentId)
        {
                var res =  await _appointmentRepository.CancelAppointment(appointmentId);
                if (res == false)
                    return Result<bool>.ErrorResult("failed to cancel appintment");

                return Result<bool>.SuccessResult(res, "appointment cancelled succefully"); 
            
        }

        public async Task<Result<bool>> RescheduleApppointment(RescheduleAppointmentDto rescedule)
        {
                var res =  await _appointmentRepository.RescheduleAppointment(
                    rescedule.AppointmentId,
                    rescedule.NewDate,
                    rescedule.NewTime
                    );

                if (res == false)
                    return Result<bool>.ErrorResult("failed to reschedule appintment");

                return Result<bool>.SuccessResult(res, "appointment rescheduled succefully");            
        }


        public async Task<Result<List<GetAppointmentsDto>>> GetAppointmentAsync()
        {
                var res = await _appointmentRepository.GetAll();

            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate =s.AppointmentDate ,
                AppointmentTime = s.AppointmentTime ,
                DoctorName = s.Doctor?.Name ?? "N/a",
                Status = s.Status,
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a"
            }).ToList() ;

                if (res == null)
                    return Result<List<GetAppointmentsDto>>.ErrorResult("appointments");
                
                return Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto,"appointments");           
        }

        public  async Task<Result<List<GetAppointmentsDto>>> GetAppointmentByDateAsync(DateTime date)
        {   
              var res=  await _appointmentRepository.GetAppointmentByDateAsync(date);
            if (res == null)
                return Result<List<GetAppointmentsDto>>.ErrorResult("date is invalid");
            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate = s.AppointmentDate,
                AppointmentTime = s.AppointmentTime,
                DoctorName = s.Doctor?.Name ?? "N/a",
                Status = s.Status,
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a"
            }).ToList();

            return  Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto, "appointments");            
        }

        public async  Task<Result<List<GetAppointmentsDto>>> GetAppointmentBtDoctorAsync(int doctorId)
        { 
               var res= await _appointmentRepository.GetAppointmentBtDoctorAsync(doctorId);
                if(res == null)                
                return Result<List<GetAppointmentsDto>>.ErrorResult("error occured or appointments not found");

            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate = s.AppointmentDate,
                AppointmentTime = s.AppointmentTime,
                DoctorName = s.Doctor?.Name ?? "N/a",
                Status = s.Status,
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a"
            }).ToList();


           return  Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto,"appointments");
        }

        public async Task<Result<List<GetAppointmentsDto>>> GetDailyAppointmentByDepartmentAsync(int departmentId, DateTime date, int pageIndex, int pageSize)
        {
           var department = await  _departmentRepos.GetById(departmentId);
            if (department == null)
                return Result<List<GetAppointmentsDto>>.ErrorResult("deprtmentId is  not valid ");

            var res = await  _appointmentRepository.GetDailyAppointmetsByDepartment(departmentId, date, pageIndex, pageSize); 
            if(res == null)
            {
                return Result<List<GetAppointmentsDto>>.ErrorResult("no appointments found");
            }
            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate = s.AppointmentDate,
                AppointmentTime = s.AppointmentTime,
                DoctorName = s.Doctor?.Name ?? "N/a",
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a",
                Status = s.Status,
            }).ToList();
            return Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto, "Data found ");
        }

        public async Task<Result<List<GetAppointmentsDto>>> GetDailyAppointmentByDocotorAsync(int doctorId, DateTime date, int pageIndex, int pageSize)
        {
            var doctor = await _doctorRepository.GetById(doctorId);
            if (doctor == null)
                return Result<List<GetAppointmentsDto>>.ErrorResult("docotr id is not valid ");

            var res = await _appointmentRepository.GetDailyAppointmetsByDoctor(doctorId, date, pageIndex, pageSize);
            if (res == null)
            {
                return Result<List<GetAppointmentsDto>>.ErrorResult("no appointments found");
            }
            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate = s.AppointmentDate,
                AppointmentTime = s.AppointmentTime,
                DoctorName = s.Doctor?.Name ?? "N/a",
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a",
                Status = s.Status
            }).ToList();
            return Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto, "Data found ");
        }

        public async Task<Result<bool>> UpdateAppointmentStatus(int appointmentId)
        {
            var appointment = await  _appointmentRepository.GetById(appointmentId);
            if (appointment == null)
            {
                return Result<bool>.ErrorResult("Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                return Result<bool>.ErrorResult("Appointment is already marked as completed.");
            }

            appointment.Status = AppointmentStatus.Completed;
           var updated =  await _appointmentRepository.UpdateAppointentAsync(appointmentId);

            return updated ?
                Result<bool>.SuccessResult(updated, "updated  appointment as completed ") :
                Result<bool>.ErrorResult("Failed to update appointment ");

        }

        public async Task<Result<VisitsAddPatientDto>> GetVisitCountPatientAysnc(int patientId)
        {   
            //if(patientId  )
            var appointments =  await _appointmentRepository.GetPatientVisitCountAsync(patientId);
            if(appointments ==null)
            {
                return Result<VisitsAddPatientDto>.ErrorResult("no appointment found");
            }
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            int todaysVisit = appointments.Count(a => a.AppointmentDate.Date == today);
            int weeksVisits = appointments.Count(a => a.AppointmentDate.Date >= startOfWeek);
            int monthVisits = appointments.Count(a => a.AppointmentDate.Date >= startOfMonth);

            var visits = new VisitsAddPatientDto
            {
                TodaysVisits = todaysVisit,
                ThisWeekVisits = weeksVisits,
                ThisMontsVisits = monthVisits,
            };
            return Result<VisitsAddPatientDto>.SuccessResult(visits, "visits of patient found");
        }
    }
}
