using System.Threading.Tasks;

namespace MedicDate.Bussines.Interfaces
{
    public interface ICedulaValidator
    {
        Task<bool> CheckCedulaExistsForCreateAsync(string numCedula);

        Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string resourceId);
    }
}
