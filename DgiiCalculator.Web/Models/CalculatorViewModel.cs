using DgiiCalculator.Application.DTOs;
using DgiiCalculator.Domain.Entities;

namespace DgiiCalculator.Web.Models
{
    public class CalculatorViewModel
    {
        // Los datos que el usuario llena
        public CalculationInputDto Input { get; set; } = new CalculationInputDto();

        // El resultado que mostramos (puede ser nulo si no se ha calculado nada)
        public CalculationResult? Result { get; set; }

        // Para mostrar errores globales si ocurren
        public string? ErrorMessage { get; set; }

        // Propiedad para saber si mostramos la tabla de resultados
        public bool ShowResults => Result != null;
    }
}