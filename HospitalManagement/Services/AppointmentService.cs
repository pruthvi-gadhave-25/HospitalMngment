using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Models.Mails;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly ILeaveRepository _leaveRspository;
        private readonly IEmailService _emailService;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            IPatientService patientService,
            IDoctorService doctorService,
            ILeaveRepository leaveRepository,
            IEmailService emailService
            )
        {
            _appointmentRepository = appointmentRepository;
            _patientService = patientService;
            _doctorService = doctorService;
            _leaveRspository = leaveRepository;
            _emailService = emailService;
        }

        public async  Task<Result<bool>> BookAppointment(BookAppointmentDto appointmentDto)
        {
            var patient =  await _patientService.GetPatientByIdAsync(appointmentDto.PatientID);
               
            var doctor = await _doctorService.GetDoctorByIdAsync(appointmentDto.DoctorId);

            if ((patient == null) || (doctor == null))
                return Result<bool>.ErrorResult("patient or doctor is invalid");

            var isDoctorOnLeave = await _leaveRspository.IsDoctorOnLeaveAsync(appointmentDto.DoctorId , appointmentDto.AppointmentDate);
            if (isDoctorOnLeave)
            {
                return Result<bool>.ErrorResult("docotr is on leave");
            }
        
            var slots = await _doctorService.GetBySlotDoctorIdAsync(appointmentDto.DoctorId);
            var appointTime = appointmentDto.AppointmentTime;
            var appointDay = appointmentDto.AppointmentDate.DayOfWeek;


            var isSlotAvailable = slots.Data.Any(slot =>
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
            var appointment = await _appointmentRepository.GetAppointmentAsync(dto.AppointmentId);
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
                var res = await _appointmentRepository.GetAllAppointmentAsync();

            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate =s.AppointmentDate ,
                AppointmentTime = s.AppointmentTime ,
                DoctorName = s.Doctor?.Name ?? "N/a",
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
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a"
            }).ToList();

                

                return  Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto, "appointments");            
        }

        public async  Task<Result<List<GetAppointmentsDto>>> GetAppointmentBtDoctorAsync(int doctorId)
        { 
               var res= await _appointmentRepository.GetAppointmentBtDoctorAsync(doctorId);
                if(res == null)                
                return Result<List<GetAppointmentsDto>>.ErrorResult("appointments");

            var appointmentDto = res.Select(s => new GetAppointmentsDto
            {
                Diagnoasis = s.Diagnoasis,
                Medications = s.Medications,
                Treatement = s.Treatement,
                AppointmentDate = s.AppointmentDate,
                AppointmentTime = s.AppointmentTime,
                DoctorName = s.Doctor?.Name ?? "N/a",
                DepartmentName = s.Doctor?.Department?.Name ?? "N/a"
            }).ToList();


           return  Result<List<GetAppointmentsDto>>.SuccessResult(appointmentDto,"appointments");
        }
    }
}
