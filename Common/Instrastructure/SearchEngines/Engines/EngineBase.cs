using System.Threading.Tasks;
using Common.Instrastructure.Helpers;
using Common.Models;

namespace Common.Instrastructure.SearchEngines.Engines
{
    public abstract class EngineBase
    {
        protected int AttemptsCount { get; set; }
        protected int ReconnectTime { get; set; }

        protected EngineBase()
        {
            var configuration = ResourceHelper.Resources.RequestConfiguration;

            AttemptsCount = configuration.AttemptsCount > 0 ? configuration.AttemptsCount : 1;
            ReconnectTime = configuration.ReconnectTime;
        }

        public abstract Task<CommonInfoModel> GetMovieInfo(MovieModel movie);
    }
}