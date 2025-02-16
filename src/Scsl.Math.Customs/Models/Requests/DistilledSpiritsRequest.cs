using Scsl.Enums;

namespace Scsl.Models.Requests;

public record DistilledSpiritsRequest(
    decimal NetRetailPrice,
    BottleSize BottleSize,
    AlcoholByVolume AlcoholByVolume,
    int NumberOfCases,
    int NumberBottlesPerCase,
    decimal AdValoremTaxRate);