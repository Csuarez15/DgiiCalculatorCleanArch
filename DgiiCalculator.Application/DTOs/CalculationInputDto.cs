using System.ComponentModel.DataAnnotations;

namespace DgiiCalculator.Application.DTOs
{
    public class CalculationInputDto
    {
        [Required(ErrorMessage = "El ingreso es obligatorio.")]
        // rango máximo 999 billones (15 dígitos)
        [Range(0.01, 999999999999999, ErrorMessage = "El valor debe ser mayor a 0 y tener un formato válido.")]
        public decimal? IncomeAmount { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una frecuencia.")]
        public string Frequency { get; set; } = "M";
    }
}