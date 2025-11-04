namespace mindtechNewsletter.Server.Responses
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ResponseModel<T> Ok(T? data = default, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ResponseModel<T> Fail(string message)
            => new() { Success = false, Message = message, Data = default };
    }
}
