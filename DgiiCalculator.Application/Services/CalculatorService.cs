using DgiiCalculator.Application.Common;
using DgiiCalculator.Application.DTOs;
using DgiiCalculator.Application.Interfaces;
using DgiiCalculator.Domain.Entities;

namespace DgiiCalculator.Application.Services
{
    public class CalculatorService : ICalculatorService
    {
        // Escala Anual ISR Asalariados (Convertida a Mensual para cálculo base)
        private const decimal Escala1 = 34685.00m;
        private const decimal Escala2 = 52027.42m;
        private const decimal Escala3 = 72260.25m;

        public ServiceResult<CalculationResult> CalculateIsr(CalculationInputDto input)
        {
            
            decimal income = input.IncomeAmount ?? 0;

            //VALIDACIÓN  DE LÓGICA
            if (income <= 0)
            {
                return ServiceResult<CalculationResult>.Fail("El ingreso debe ser mayor a 0. No se permiten valores negativos ni vacíos.");
            }

            // Validar frecuencia correcta
            if (input.Frequency != "M" && input.Frequency != "A")
            {
                return ServiceResult<CalculationResult>.Fail("La frecuencia seleccionada no es válida. Use 'Mensual' o 'Anual'.");
            }

            //NORMALIZACIÓN A BASE MENSUAL
            decimal monthlyBase = 0;
            if (input.Frequency == "A")
            {
                monthlyBase = income / 12; 
            }
            else
            {
                monthlyBase = income;
            }

            //CÁLCULO DEL IMPUESTO (Lógica Core)
            decimal monthlyTax = 0;
            string rateDescription = "Exento (0%)";

            if (monthlyBase <= Escala1)
            {
                monthlyTax = 0;
                rateDescription = "Exento (Ingreso por debajo de RD$ 34,685.00)";
            }
            else if (monthlyBase <= Escala2)
            {
                // 15% del excedente de Escala1
                monthlyTax = (monthlyBase - Escala1) * 0.15m;
                rateDescription = "15% del excedente";
            }
            else if (monthlyBase <= Escala3)
            {
                // Fijo del tramo anterior + 20% del excedente
                decimal fixedTax = 2601.36m;
                monthlyTax = fixedTax + ((monthlyBase - Escala2) * 0.20m);
                rateDescription = "20% del excedente";
            }
            else
            {
                // Fijo tramos anteriores + 25% del excedente
                decimal fixedTax = 6648.00m;
                monthlyTax = fixedTax + ((monthlyBase - Escala3) * 0.25m);
                rateDescription = "25% del excedente (Tasa Máxima)";
            }

            // Redondear a 2 decimales
            monthlyTax = Math.Round(monthlyTax, 2);
            decimal monthlyNet = Math.Round(monthlyBase - monthlyTax, 2);

            //PROYECCIÓN ANUAL
            decimal annualIncome = Math.Round(monthlyBase * 12, 2);
            decimal annualTax = Math.Round(monthlyTax * 12, 2);
            decimal annualNet = Math.Round(monthlyNet * 12, 2);

            //CONSTRUIR RESULTADO
            var result = new CalculationResult
            {
                MonthlyIncome = Math.Round(monthlyBase, 2),
                MonthlyTax = monthlyTax,
                MonthlyNet = monthlyNet,
                AnnualIncome = annualIncome,
                AnnualTax = annualTax,
                AnnualNet = annualNet,
                AppliedRate = rateDescription,
                CalculationDate = DateTime.Now
            };

            return ServiceResult<CalculationResult>.Ok(result);
        }
    }
}