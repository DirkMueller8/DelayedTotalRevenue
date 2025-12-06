namespace DelayedTotalRevenue.Services
{
    using DelayedTotalRevenue.Models;

    /// <summary>
    /// Defines methods for calculating the impact of launch delays on revenue.
    public interface IDelayCalculator
    {
        /// <summary>
        /// Calculates revenue components for both an ideal launch and a delayed launch,
        /// returning per-phase totals and loss metrics.
        /// </summary>
        /// <param name="triangleWeeks">
        /// Number of weeks used for ramp-up and ramp-down (the triangular portion).
        /// Must be greater than 0.
        /// </param>
        /// <param name="maturityWeeks">
        /// Number of weeks the product remains at peak revenue (plateau). Must be 0 or greater.
        /// </param>
        /// <param name="peakRevenue">
        /// Peak revenue value (per week) reached at maturity. Must be 0 or greater.
        /// </param>
        /// <param name="delayWeeks">
        /// Length of the launch delay in weeks. Must be greater than or equal to 0 and typically less than or equal to <paramref name="triangleWeeks"/>.
        /// </param>
        /// <returns>
        /// A <see cref="CalculationResult"/> containing the triangular, plateau and total revenues for the ideal
        /// and delayed scenarios plus absolute and percentage loss metrics.
        /// </returns>
        /// <remarks>
        /// Implementations should validate inputs and throw <see cref="ArgumentOutOfRangeException"/>
        /// for invalid ranges. <see cref="CalculationResult.PercentLoss"/> is expressed as a fraction
        /// of the ideal total (for example, 0.10 == 10%).
        /// </remarks>
        CalculationResult Calculate(double triangleWeeks, double maturityWeeks, double peakRevenue, double delayWeeks);
    }
}