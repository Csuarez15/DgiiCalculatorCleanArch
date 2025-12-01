namespace DgiiCalculator.Application.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T> { Success = true, Data = data };
        }

        public static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T> { Success = false, ErrorMessage = message };
        }
    }
}