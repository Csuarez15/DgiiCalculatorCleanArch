using DgiiCalculator.Application.Common;
using DgiiCalculator.Application.DTOs;
using DgiiCalculator.Domain.Entities;

namespace DgiiCalculator.Application.Interfaces
{
    public interface ICalculatorService
    {
        ServiceResult<CalculationResult> CalculateIsr(CalculationInputDto input);
    }
}