namespace Scsl.Math;

public static class TaxCalculator
{
    public static decimal ComputeSpecificTax(int year, decimal baseAmount, decimal increaseRate)
    {
        int yearsSince2024 = year - 2024;

        decimal taxRate = System.Math.Round(
            baseAmount * (decimal)System.Math.Pow((double)(1 + increaseRate), yearsSince2024), 2
        );

        return taxRate;
    }
} 