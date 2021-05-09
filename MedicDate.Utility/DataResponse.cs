namespace MedicDate.Utility
{
    public class DataResponse<T>
    {
        public T Data { get; set; }
        public bool Sussces { get; set; } = true;
        public string Message { get; set; }
    }
}