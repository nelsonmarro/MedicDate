using System.Threading.Tasks;

namespace MedicDate.DataAccess.Interfaces
{
    public interface ICedulaValidator
    {
        Task<bool> CheckCedulaExistsForCreateAsync(string numCedula);

        Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string resourceId);
    }
}
