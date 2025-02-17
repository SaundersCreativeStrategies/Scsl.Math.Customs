using Scsl.Enums;
using Scsl.Math.Customs.Tax.ExciseTax;
using Scsl.Models.Requests;

namespace Scsl.Math.Customs.UnitTest;

[TestClass]
public sealed class DistilledSpiritsTaxTests
{
    private DistilledSpiritsService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _service = new DistilledSpiritsService();
    }
    
    [TestMethod]
    public void ComputeExciseTax_ShouldReturnExpectedResult()
    {
        // Arrange
        var request = new DistilledSpiritsRequest(
            220,
            BottleSize: BottleSize.BottleSize750Ml,
            AlcoholByVolume: AlcoholByVolume.FortyPercent,
            200,
            12,
            22m
        );
        var expectedExciseTax = ComputeExpectedExciseTax(request);
        
        // Act
        var response = _service.ComputeExciseTax(request);
        
        // Assert
        Assert.AreEqual(expectedExciseTax, response.ExciseTax, 0.01m, "Excise tax computation is incorrect");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ComputeExciseTax_ShouldThrowException_WhenRequestIsNull()
    {
        _service.ComputeExciseTax(null!);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ComputeExciseTax_ShouldThrowException_WhenNumberOfCasesIsZero()
    {
        var request = new DistilledSpiritsRequest(
            220,
            BottleSize: BottleSize.BottleSize750Ml,
            AlcoholByVolume: AlcoholByVolume.FortyPercent,
            0,
            12,
            22m);
        
        _service.ComputeExciseTax(request);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ComputeExciseTax_ShouldThrowException_WhenNegativeBottlesPerCase()
    {
        var request = new DistilledSpiritsRequest(
            220,
            BottleSize: BottleSize.BottleSize750Ml,
            AlcoholByVolume: AlcoholByVolume.FortyPercent,
            -1,
            12,
            22m);
        
        _service.ComputeExciseTax(request);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ComputeExciseTax_ShouldThrowException_WhenNegativeNetRetailPrice()
    {
        var request = new DistilledSpiritsRequest(
            -500,
            BottleSize: BottleSize.BottleSize750Ml,
            AlcoholByVolume: AlcoholByVolume.FortyPercent,
            200,
            12,
            22m);
        
        _service.ComputeExciseTax(request);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ComputeExciseTax_ShouldThrowException_WhenNegativeAdValoremTaxRate()
    {
        var request = new DistilledSpiritsRequest(
            220,
            BottleSize: BottleSize.BottleSize750Ml,
            AlcoholByVolume: AlcoholByVolume.FortyPercent,
            200,
            12,
            -5m);
        
        _service.ComputeExciseTax(request);
    }
    
    private decimal ComputeExpectedExciseTax(DistilledSpiritsRequest request)
    {
        int totalBottles = request.NumberOfCases * request.NumberBottlesPerCase;
        decimal liter = (decimal)request.BottleSize / 1000m;
        decimal proof = (decimal)request.AlcoholByVolume / 100m;
        decimal specificTaxRate = TaxCalculator.ComputeSpecificTax(DateTime.UtcNow.Year, 66.00m,0.06m);
        decimal nrpPerLiter = System.Math.Round(request.NetRetailPrice / liter, 2);
        decimal grossLiters = System.Math.Round(totalBottles * liter, 2);
        decimal proofLiters = System.Math.Round(grossLiters * proof, 2);
        decimal adValoremTax = System.Math.Round((nrpPerLiter * ((decimal)request.AdValoremTaxRate / 100m)) + specificTaxRate, 4);
        decimal exciseTax = proofLiters * adValoremTax;
        return System.Math.Round(exciseTax, 2);
    }
}
