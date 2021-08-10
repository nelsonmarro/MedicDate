using System.Threading.Tasks;

namespace MedicDate.Bussines.Interfaces
{
    public interface ICedulaValidator
    {
        Task<bool> CheckCedulaExistsAsync(string numCedula);

        Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string resourceId);
    }
}
