using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Helpers
{
    public    class ApiResponse<T>
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }

}
