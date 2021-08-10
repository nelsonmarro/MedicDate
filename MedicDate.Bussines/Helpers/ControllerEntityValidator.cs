using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Bussines.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Helpers
{
    public static class ControllerEntityValidator
    {
        public static async Task<DataResponse<string>> ValidateAsync
        (
            IRequestEntityValidator requestEntityValidator,
            bool validateCedula = true,
            bool validateRelatedEntityId = true,
            string cedula = null,
            List<string> entityIds = null
        )
        {
            if (validateCedula && !string.IsNullOrEmpty(cedula))
            {
                if (await requestEntityValidator.CheckCedulaExistsAsync(cedula))
                {
                    return new DataResponse<string>()
                    {
                        IsSuccess = false,
                        ActionResult = new BadRequestObjectResult("Ya existe un registro con la cédula que ingresó")
                    };
                }

                return new DataResponse<string>()
                {
                    IsSuccess = true
                };
            }

            if (validateRelatedEntityId && entityIds is not null)
            {
                if (await requestEntityValidator.CheckRelatedEntityIdExistsAsync(entityIds))
                {
                    return new DataResponse<string>()
                    {
                        IsSuccess = false,
                        ActionResult = new BadRequestObjectResult("Error al crear el registro")
                    };
                }

                return new DataResponse<string>()
                {
                    IsSuccess = true
                };
            }

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }
    }
}