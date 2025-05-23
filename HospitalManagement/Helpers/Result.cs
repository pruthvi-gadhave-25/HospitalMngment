namespace HospitalManagement.Helpers
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }


        public static Result<T> SuccessResult(T data, string message) =>
        new Result<T> { IsSuccess = true , Message =message , Data =data};


        public static Result<T> ErrorResult( string message) =>
            new Result<T> { IsSuccess = false, Message = message, Data = default };
          

        
    }

}
