using DelayedTotalRevenue.Models;
using System;

namespace DelayedTotalRevenue.Services
{
    public class DelayCalculator
    {
        // Compute following the model we discussed (ramp-up slope shifted; decline fixed)
        public CalculationResult Calculate(double triangleWeeks, double maturityWeeks, double peakRevenue, double delayWeeks)
        {
            if (triangleWeeks <= 0) throw new ArgumentOutOfRangeException(nameof(triangleWeeks));
            if (maturityWeeks < 0) throw new ArgumentOutOfRangeException(nameof(maturityWeeks));
            if (peakRevenue < 0) throw new ArgumentOutOfRangeException(nameof(peakRevenue));
            if (delayWeeks < 0 || delayWeeks >= triangleWeeks) throw new ArgumentOutOfRangeException(nameof(delayWeeks));
            if (delayWeeks > maturityWeeks) throw new ArgumentOutOfRangeException(nameof(delayWeeks));


            // Ideal
            double idealTriangleRevenue = triangleWeeks * peakRevenue;        // area of full triangle (ramp-up + ramp-down)
            double idealPlateauRevenue = maturityWeeks * peakRevenue;
            double idealTotalRevenue = idealTriangleRevenue + idealPlateauRevenue;

            double delayedRevenue = peakRevenue * (1- delayWeeks/triangleWeeks); // height of delayed plateau

            // Delayed: compute triangle/plateau per piecewise model
            double delayedTriangleRevenue = delayedRevenue * (triangleWeeks - delayWeeks); // area of both delayed triangle portions
            double delayedPlateauRevenue = maturityWeeks * delayedRevenue;
            double delayedExtraRevenue = delayWeeks * delayedRevenue; // additional rectangle before hitting decline curve
            double delayedTotalRevenue = delayedTriangleRevenue + delayedPlateauRevenue + delayedExtraRevenue;

            double absoluteLoss = idealTotalRevenue - delayedTotalRevenue;
            double percentLoss = idealTotalRevenue > 0 ? (absoluteLoss / idealTotalRevenue) * 100.0 : 0.0;

            return new CalculationResult
            {
                IdealTriangle = idealTriangleRevenue,
                IdealPlateau = idealPlateauRevenue,
                IdealTotal = idealTotalRevenue,
                DelayedPlateau = delayedRevenue,
                DelayedTotal = delayedTotalRevenue,
                AbsoluteLoss = absoluteLoss,
                PercentLoss = percentLoss
            };
        }
    }
}