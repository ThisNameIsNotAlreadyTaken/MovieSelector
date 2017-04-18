using System;
using Common.Instrastructure.SearchEngines.Engines;

namespace Common.Instrastructure.SearchEngines
{
    public static class EngineFactory
    {
        private static readonly Lazy<TheMovieDbEngine> TheMovieDbEngine = new Lazy<TheMovieDbEngine>();

        private static readonly Lazy<OmdbEngine> OmdbEngine = new Lazy<OmdbEngine>();

        public static EngineBase GetEngine(EngineType engineType)
        {
            switch (engineType)
            {
                case EngineType.TheMovieDb:
                    return TheMovieDbEngine.Value;
                case EngineType.OmdbApi:
                    return OmdbEngine.Value;
                default:
                    return TheMovieDbEngine.Value;
            }
        }
    }
}