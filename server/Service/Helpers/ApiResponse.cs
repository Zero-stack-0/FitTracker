namespace Service.Helpers
{
    public class ApiResponse
    {
        public ApiResponse(object? data = null, string? message = null, int statusCodes = 200)
        {
            Data = data;
            Message = message;
            StatusCodes = statusCodes;
        }

        public object? Data { get; set; }
        public string? Message { get; set; }
        public int StatusCodes { get; set; }
    }
}