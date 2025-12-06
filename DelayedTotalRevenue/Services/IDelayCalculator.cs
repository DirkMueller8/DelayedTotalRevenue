using DelayedTotalRevenue.Models;

namespace DelayedTotalRevenue.Services
{
    public interface IDelayCalculator
    {
        CalculationResult Calculate(double triangleWeeks, double maturityWeeks, double peakRevenue, double delayWeeks);
    }
}