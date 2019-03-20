namespace OnlineCharter.API.WebService.Models
{
    public class ExecutionResponse<T> 
    {
        public T Result { get; set; }
        public string Error { get; set; }
    }
}
