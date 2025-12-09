using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DelayedTotalRevenue.Services;

namespace DelayedTotalRevenue.Tests
{
    [TestClass]
    public class RecallCalculatorTests
    {
        [TestMethod]
        public void CalculateRecallLoss_ValidInputs_ReturnsCorrectLoss()
        {
            var calculator = new DelayCalculator();
            double triangleWeeks = 4;
            double maturityWeeks = 10;
            double peakRevenue = 100;
            double recallWeeks = 2;

            var result = calculator.CalculateRecallLoss(triangleWeeks, maturityWeeks, peakRevenue, recallWeeks);

            // idealTotal = 4*100 + 10*100 = 1400
            // recallLoss = 2 * 100 = 200
            // adjustedTotal = 1400 - 200 = 1200
            // percentLoss = (200 / 1400) * 100 ? 14.29%
            Assert.AreEqual(1400.0, result.IdealTotal, 1e-9);
            Assert.AreEqual(2.0, result.RecallWeeks, 1e-9);
            Assert.AreEqual(200.0, result.RecallLoss, 1e-9);
            Assert.AreEqual(1200.0, result.AdjustedTotal, 1e-9);
            Assert.AreEqual(14.285714285714286, result.PercentLoss, 1e-6);
        }

        [TestMethod]
        public void CalculateRecallLoss_ZeroRecallWeeks_ReturnsNoLoss()
        {
            var calculator = new DelayCalculator();
            var result = calculator.CalculateRecallLoss(4, 10, 100, 0);

            Assert.AreEqual(0.0, result.RecallLoss, 1e-9);
            Assert.AreEqual(result.IdealTotal, result.AdjustedTotal, 1e-9);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculateRecallLoss_RecallExceedsMaturity_Throws()
        {
            var calculator = new DelayCalculator();
            calculator.CalculateRecallLoss(4, 10, 100, 15); // 15 > 10
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculateRecallLoss_NegativeRecallWeeks_Throws()
        {
            var calculator = new DelayCalculator();
            calculator.CalculateRecallLoss(4, 10, 100, -1);
        }
    }
}