using System;
using MedicDate.Utility.Interfaces;

namespace MedicDate.Models.DTOs.Grupo
{
    public class GrupoResponse : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}
