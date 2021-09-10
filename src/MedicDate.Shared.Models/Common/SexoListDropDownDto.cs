using MedicDate.Utility.Enums;
using System.Collections.Generic;

namespace MedicDate.API.DTOs.Common
{
    public static class SexoListDropDownDto
    {
        public static IEnumerable<SexoDropDownItemDto> SexoList { get; } = new List<SexoDropDownItemDto>
        {
            new() { Sexo = TipoSexo.Masculino.ToString() },

            new() { Sexo = TipoSexo.Femenino.ToString() }
        };
    }
}
