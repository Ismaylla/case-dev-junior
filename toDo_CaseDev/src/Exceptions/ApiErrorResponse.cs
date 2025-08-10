namespace TodoApi.Exceptions
{
    public class ApiErrorResponse
    {
        public string Status { get; set; }
        public IEnumerable<string> Mensagens { get; set; }

        public ApiErrorResponse(string status, string message)
        {
            Status = status;
            Mensagens = new[] { message };
        }

        public ApiErrorResponse(string status, IEnumerable<string> messages)
        {
            Status = status;
            Mensagens = messages;
        }
    }
}