using Scsl.Models.Requests;
using Scsl.Models.Responses;

namespace Scsl.Math.Customs.Tax.ExciseTax;

// ref: Section 141, Republic ct No. 11467, Effective Jan 1, 2020 until Dec 31, 2020
// Compute: Excise Tax = Ad Valorm Tax + spacific tax
// Book Ref: Handbook on Custom Valuation and Practical Computation of Duties and Taxes 
// Edition: 2022
// ISBN: 971-14-3445-4

/// <summary>
/// Computes the excise tax for distilled spirits based on the provided request parameters.
/// </summary>
/// <param name="request">The details of the distilled spirits, including volume, alcohol content, and pricing.</param>
/// <returns>A <see cref="DistilledSpiritsResponse"/> containing the calculated excise tax.</returns>
/// <remarks>
/// This method performs excise tax calculations based on the provided input data.
/// The accuracy of the calculations depends on the correctness of the input values.
/// </remarks>
/// <exception cref="ArgumentException">
/// Thrown if the input values are invalid, such as negative numbers.
/// </exception>
/// <para>
/// <b>Disclaimer:</b>  
/// This code is provided "as is" without warranty of any kind, either express or implied,  
/// including but not limited to the warranties of merchantability, fitness for a particular purpose,  
/// or non-infringement. The author is not liable for any damages, losses, or incorrect calculations  
/// resulting from the use of this code.
/// </para>
public class DistilledSpiritsService : IDistilledSpiritsService
{
    public DistilledSpiritsResponse ComputeExciseTax(DistilledSpiritsRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Request object cannot be null.");
        }

        if (request.NumberOfCases <= 0)
        {
            throw new ArgumentException("Number of cases must be greater than zero.", nameof(request.NumberOfCases));
        }

        if (request.NumberBottlesPerCase <= 0)
        {
            throw new ArgumentException("Number of bottles per case must be greater than zero.", nameof(request.NumberBottlesPerCase));
        }
        
        if (request.NetRetailPrice <= 0)
        {
            throw new ArgumentException("Net retail price cannot be negative.", nameof(request.NetRetailPrice));
        }

        if (request.AdValoremTaxRate < 0)
        {
            throw new ArgumentException("Ad Valorem Tax rate cannot be negative.", nameof(request.AdValoremTaxRate));
        }
        
        int totalBottles = request.NumberOfCases * request.NumberBottlesPerCase;
        decimal liter = (decimal)request.BottleSize / 1000m;
        decimal proof = (decimal)request.AlcoholByVolume / 100m;
        decimal specificTaxRate = ComputeSpecificTax(DateTime.UtcNow.Year);
        
        // Compute the net retail price per liter
        decimal nrpPerLiter = System.Math.Round(request.NetRetailPrice / liter, 2);
        
        // Compute the gross liters
        decimal grossLiters = System.Math.Round(totalBottles * liter, 2);
        
        // Computer the proof liters
        decimal proofLiters = System.Math.Round(grossLiters * proof, 2);
        
        // Compute the Ad Valorem Tax
        decimal adValormTax = System.Math.Round((nrpPerLiter * ((decimal)request.AdValoremTaxRate / 100m)) + specificTaxRate, 4);
        
        // Compute the Excise Tax 
        decimal exciseTax = proofLiters * adValormTax;
        
        return new (System.Math.Round(exciseTax, 2));
    }

    private decimal ComputeSpecificTax(int year)
    {
        const decimal baseAmount = 66.00m;
        const decimal increaseRate = 0.06m;
        int yearsSince2024 = year - 2024;
        
        decimal taxRate = System.Math.Round(baseAmount * (decimal)System.Math.Pow((double)(1 + increaseRate), yearsSince2024), 2);
        return taxRate;
    }
}