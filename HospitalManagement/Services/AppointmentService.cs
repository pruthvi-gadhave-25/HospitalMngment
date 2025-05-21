using HospitalManagement.DTO;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            IPatientService patientService,
            IDoctorService doctorService
            )
        {
            _appointmentRepository = appointmentRepository;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public async  Task<bool> BookAppointment(BookAppointmentDto appointmentDto)
        {
            try
            {                
             
                var patient =  await _patientService.GetPatientByIdAsync(appointmentDto.PatientID);
               
                var doctor = await _doctorService.GetDoctorByIdAsync(appointmentDto.DoctorId);

                if ((patient == null) || (doctor == null))
                    return false;

                var slots = await _doctorService.GetBySlotDoctorIdAsync(appointmentDto.DoctorId);
                var appointTime = appointmentDto.AppointmentTime;
                var appointDay = appointmentDto.AppointmentDate.DayOfWeek;


                var isSlotAvailable = slots.Any(slot =>
                slot.DayofWeek == appointDay &&
                slot.StartTime <= appointTime && 
                appointTime < slot.EndTime
                );


                if (!isSlotAvailable)
                {
                    return false;
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

                await _appointmentRepository.BookAppointment(appointment);
                return true;
                 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> RescheduleAppointmentAsync(RescheduleAppointmentDto dto)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentAsync(dto.AppointmentId);
                if (appointment == null)
                    return false;


                return await _appointmentRepository.RescheduleAppointment(dto.AppointmentId, dto.NewDate, dto.NewTime);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> CancelApppointment(int appointmentId)
        {
            try
            {
                return await _appointmentRepository.CancelAppointment(appointmentId);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> RescheduleApppointment(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            try
            {
                return await _appointmentRepository.RescheduleAppointment(
                    rescheduleAppointmentDto.AppointmentId,
                    rescheduleAppointmentDto.NewDate,
                    rescheduleAppointmentDto.NewTime
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<List<Appointment>> GetAppointmentAsync()
        {
            try
            {
                var res = await _appointmentRepository.GetAllAppointmentAsync();
                return res;
            }
            catch (Exception ex)
            {
                //logger exception
                return new List<Appointment>();
            }
        }

        public Task<List<Appointment>> GetAppointmentByDateAsync(DateTime date)
        {
            try
            {   
              var res=   _appointmentRepository.GetAppointmentByDateAsync(date);
                if (res == null)
                    return null;

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Task<List<Appointment>> GetAppointmentBtDoctorAsync(int doctorId)
        {
            try
            {   
               var res=  _appointmentRepository.GetAppointmentBtDoctorAsync(doctorId);
                if(res == null)
                {
                    return null;
                }
                return  res ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
