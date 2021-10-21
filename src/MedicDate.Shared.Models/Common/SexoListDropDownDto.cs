using MedicDate.Utility.Enums;

namespace MedicDate.Shared.Models.Common
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
