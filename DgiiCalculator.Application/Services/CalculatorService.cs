using DgiiCalculator.Application.Common;
using DgiiCalculator.Application.DTOs;
using DgiiCalculator.Application.Interfaces;
using DgiiCalculator.Domain.Entities;
using Microsoft.Extensions.Options; // Necesario para IOptions

namespace DgiiCalculator.Application.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IsrSettings _settings;

        // Inyectamos la configuración desde appsettings.json
        public CalculatorService(IOptions<IsrSettings> settings)
        {
            _settings = settings.Value;
        }

        public ServiceResult<CalculationResult> CalculateIsr(CalculationInputDto input)
        {
            decimal income = input.IncomeAmount ?? 0;

            // 1. VALIDACIONES
            if (income <= 0)
            {
                return ServiceResult<CalculationResult>.Fail("El ingreso debe ser mayor a 0.");
            }

            // 2. PROYECCIÓN ANUAL (Toda la lógica de DGII se basa en la escala anual)
            decimal annualIncome;

            if (input.Frequency == "M")
            {
                // Si ingresó mensual, lo anualizamos multiplicando por 12
                annualIncome = income * 12;
            }
            else
            {
                // Si ingresó anual, se queda igual
                annualIncome = income;
            }

            // CÁLCULO DEL IMPUESTO ANUAL (Usando valores de configuración)
            decimal annualTax = 0;
            string rateDescription = "Exento";

            if (annualIncome <= _settings.Scale1Limit)
            {
                annualTax = 0;
                rateDescription = "Exento (Renta anual inferior al mínimo)";
            }
            else if (annualIncome <= _settings.Scale2Limit)
            {
                // 15% del excedente de la Escala 1
                annualTax = (annualIncome - _settings.Scale1Limit) * _settings.Rate1;
                rateDescription = "15% del excedente";
            }
            else if (annualIncome <= _settings.Scale3Limit)
            {
                // Base fija + 20% del excedente de la Escala 2
                annualTax = _settings.BaseTax2 + ((annualIncome - _settings.Scale2Limit) * _settings.Rate2);
                rateDescription = "20% del excedente";
            }
            else
            {
                // Base fija + 25% del excedente de la Escala 3
                annualTax = _settings.BaseTax3 + ((annualIncome - _settings.Scale3Limit) * _settings.Rate3);
                rateDescription = "25% del excedente (Tasa Máxima)";
            }

            // CALCULAR VALORES MENSUALES (Dividiendo el anual entre 12)
            // La DGII retiene mensualmente la doceava parte del impuesto anual estimado.
            decimal monthlyTax = annualTax / 12;

            // Calculamos el ingreso mensual base para mostrar en la tabla
            decimal monthlyIncomeBase = input.Frequency == "M" ? income : income / 12;

            decimal monthlyNet = monthlyIncomeBase - monthlyTax;
            decimal annualNet = annualIncome - annualTax;

            var result = new CalculationResult
            {
                // Redondeamos a 2 decimales para la visualización final
                MonthlyIncome = Math.Round(monthlyIncomeBase, 2),
                MonthlyTax = Math.Round(monthlyTax, 2),
                MonthlyNet = Math.Round(monthlyNet, 2),

                AnnualIncome = Math.Round(annualIncome, 2),
                AnnualTax = Math.Round(annualTax, 2),
                AnnualNet = Math.Round(annualNet, 2),

                AppliedRate = rateDescription,
                CalculationDate = DateTime.Now
            };

            return ServiceResult<CalculationResult>.Ok(result);
        }
    }
}