using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Commands;

namespace PFM.Services

{
    public interface IAnalyticsService {
        ICollection<AnalyticsModel> GetAnalytics(string catcode, string startDate, string endDate, string direction);
    }
}