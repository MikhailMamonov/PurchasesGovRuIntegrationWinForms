using System.Collections.Generic;
using System.Threading.Tasks;

namespace PurchasesGovRuIntegration.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<Dictionary<string,Dictionary<string,string>>> Find(string regNumber);
    }
}
