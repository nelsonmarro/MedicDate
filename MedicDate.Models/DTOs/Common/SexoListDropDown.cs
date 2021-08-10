using System;
using System.Collections.Generic;
using MedicDate.Utility.Enums;

namespace MedicDate.Models.DTOs.Common
{
    public static class SexoListDropDown
    {
        public static IEnumerable<SexoDropDownItem> SexoList { get; } = new List<SexoDropDownItem>
        {
            new() { Sexo = TipoSexo.Masculino.ToString() },

            new() { Sexo = TipoSexo.Femenino.ToString() }
        };
    }
}
