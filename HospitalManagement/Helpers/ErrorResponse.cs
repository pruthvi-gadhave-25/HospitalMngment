namespace HospitalManagement.Helpers
{
    public class ErrorResponse<T>
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public T Data { get; set; } 
    }
}
