using DgiiCalculator.Application.Common;
using DgiiCalculator.Application.DTOs;
using DgiiCalculator.Application.Interfaces;
using DgiiCalculator.Domain.Entities;
using Microsoft.Extensions.Options;

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

            // VALIDACIONES
            if (income <= 0)
            {
                return ServiceResult<CalculationResult>.Fail("El ingreso debe ser mayor a 0.");
            }

            //PROYECCIÓN DE INGRESOS (Base Anual)
            decimal annualIncomeBase;

            if (input.Frequency == "M")
            {
                annualIncomeBase = income * 12;
            }
            else
            {
                annualIncomeBase = income;
            }

            // CÁLCULO DEL IMPUESTO ANUAL (LIQUIDACIÓN TEÓRICA)
            decimal rawAnnualTax = 0;
            string rateDescription = "Exento";

            if (annualIncomeBase <= _settings.Scale1Limit)
            {
                rawAnnualTax = 0;
                rateDescription = "Exento";
            }
            else if (annualIncomeBase <= _settings.Scale2Limit)
            {
                rawAnnualTax = (annualIncomeBase - _settings.Scale1Limit) * _settings.Rate1;
                rateDescription = "15% del excedente";
            }
            else if (annualIncomeBase <= _settings.Scale3Limit)
            {
                rawAnnualTax = _settings.BaseTax2 + ((annualIncomeBase - _settings.Scale2Limit) * _settings.Rate2);
                rateDescription = "20% del excedente";
            }
            else
            {
                rawAnnualTax = _settings.BaseTax3 + ((annualIncomeBase - _settings.Scale3Limit) * _settings.Rate3);
                rateDescription = "25% del excedente (Tasa Máxima)";
            }

            //CÁLCULO DE RETENCIÓN REAL 

            //Dividimos entre 12 para saber la cuota mensual bruta
            decimal rawMonthlyTax = rawAnnualTax / 12;

            //REDONDEAMOS la cuota mensual a 2 decimales AHORA.
            // Esto es crucial. En la vida real, la nómina te descuenta centavos exactos.
            decimal finalMonthlyTax = Math.Round(rawMonthlyTax, 2, MidpointRounding.AwayFromZero);

            //Recalculamos el Anual "Proyectado" basado en lo que realmente se va a retener.
            // Esto garantiza que la columna Anual sea siempre exactamente 12 veces la Mensual.
            decimal finalAnnualTax = finalMonthlyTax * 12;

            //CÁLCULO DE SUELDOS NETOS
            decimal monthlyIncomeBase = input.Frequency == "M" ? income : income / 12;
            decimal finalMonthlyNet = monthlyIncomeBase - finalMonthlyTax;
            decimal finalAnnualNet = annualIncomeBase - finalAnnualTax;

            var result = new CalculationResult
            {
                // Ingresos
                MonthlyIncome = Math.Round(monthlyIncomeBase, 2),
                AnnualIncome = Math.Round(annualIncomeBase, 2),

                // Impuestos (Usamos los valores ya normalizados)
                MonthlyTax = finalMonthlyTax,
                AnnualTax = finalAnnualTax,

                // Netos
                MonthlyNet = Math.Round(finalMonthlyNet, 2),
                AnnualNet = Math.Round(finalAnnualNet, 2),

                AppliedRate = rateDescription,
                CalculationDate = DateTime.Now
            };

            return ServiceResult<CalculationResult>.Ok(result);
        }
    }
}