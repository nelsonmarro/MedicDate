using MedicDate.Bussines.ApplicationServices.IApplicationServices;

namespace MedicDate.Bussines.ApplicationServices
{
    public class ISuccessDataResultBuilder<T> : ISuccessResultBuilder<T>
    {
        public T BuildSuccessResult(T successObj)
        {
            return successObj;
        }
    }
}
