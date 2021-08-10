namespace MedicDate.Client.Interceptors.IInterceptors
{
    public interface IInterceptor
    {
        public void RegisterEvent();
        public void DisposeEvent();
    }
}
