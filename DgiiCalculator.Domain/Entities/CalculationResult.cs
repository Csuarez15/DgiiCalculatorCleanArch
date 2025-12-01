namespace DgiiCalculator.Domain.Entities
{
    public class CalculationResult
    {
        // Datos Mensuales
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyTax { get; set; }
        public decimal MonthlyNet { get; set; }

        // Datos Anuales
        public decimal AnnualIncome { get; set; }
        public decimal AnnualTax { get; set; }
        public decimal AnnualNet { get; set; }

        // Metadatos
        public string AppliedRate { get; set; } = string.Empty;
        public DateTime CalculationDate { get; set; }
    }
}