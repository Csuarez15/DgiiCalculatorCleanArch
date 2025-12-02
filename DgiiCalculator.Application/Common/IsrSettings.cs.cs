namespace DgiiCalculator.Application.Common
{
    public class IsrSettings
    {
        public decimal Scale1Limit { get; set; } // 416,220.00
        public decimal Scale2Limit { get; set; } // 624,329.00
        public decimal Scale3Limit { get; set; } // 867,123.00

        public decimal Rate1 { get; set; } // 0.15
        public decimal Rate2 { get; set; } // 0.20
        public decimal Rate3 { get; set; } // 0.25

        public decimal BaseTax2 { get; set; } // 31,216.00
        public decimal BaseTax3 { get; set; } // 79,776.00
    }
}