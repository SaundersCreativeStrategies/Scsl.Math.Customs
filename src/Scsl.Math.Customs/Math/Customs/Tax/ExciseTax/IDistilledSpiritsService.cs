using Scsl.Models.Requests;
using Scsl.Models.Responses;

namespace Scsl.Math.Customs.Tax.ExciseTax;

public interface IDistilledSpiritsService
{
    DistilledSpiritsResponse ComputeExciseTax(DistilledSpiritsRequest request);
}