using System.ComponentModel.DataAnnotations;

namespace DgiiCalculator.Application.DTOs
{
    public class CalculationInputDto
    {
        
        [Required(ErrorMessage = "El ingreso es obligatorio.")]
        [Range(0.01, 999999999, ErrorMessage = "El valor debe ser mayor a 0.")]
        public decimal? IncomeAmount { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una frecuencia.")]
        public string Frequency { get; set; } = "M";
    }
}