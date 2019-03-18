namespace OnlineCharter.API.WebService.Models
{
    public class ExecutionResponse<T> where T : class
    {
        public T Result { get; set; }
        public string Error { get; set; }
    }
}
