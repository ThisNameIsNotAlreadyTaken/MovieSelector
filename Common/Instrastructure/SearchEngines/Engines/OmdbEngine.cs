using System;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Instrastructure.Helpers;
using Common.Models;
using Common.Models.InfoModels.OmdbApi;
using Newtonsoft.Json;

namespace Common.Instrastructure.SearchEngines.Engines
{
    public class OmdbEngine : EngineBase
    {
        private static OmdbApiConfiguration Configuration => ResourceHelper.Resources.OmdbApiConfiguration;

        private static readonly Random Rnd = new Random();

        public override async Task<CommonInfoModel> GetMovieInfo(MovieModel movie)
        {
            var info = await GetCommonMovieInfoAsync(movie.FileNameWithoutExtension);

            if (info == null) return null;

            info.PosterImage = await GetImageAsync(info.PosterUrl);

            return info.ToCommonInfoModel();
        }

        private async Task<OmdbApiInfo> GetCommonMovieInfoAsync(string query)
        {
            var attemptCnt = 0;
            var gotResult = false;
            OmdbApiInfo result = null;

            var queryString = string.Format(Configuration.SearchUrlTemplate, query);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < AttemptsCount)
                {
                    try
                    {
                        var data = await client.GetStringAsync(queryString);

                        if (data != null)
                        {
                            var info = JsonConvert.DeserializeObject<OmdbApiInfo>(data);

                            if (info?.Id != null)
                            {
                                result = info;
                                gotResult = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Rnd.Next(0, ReconnectTime))).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private async Task<byte[]> GetImageAsync(string posterUrl)
        {
            var attemptCnt = 0;
            var gotResult = false;
            byte[] result = null;

            if (posterUrl == null) return null;

            var queryString = posterUrl;

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < AttemptsCount)
                {
                    try
                    {
                        var data = await client.GetByteArrayAsync(queryString);

                        if (data != null && data.Length > 0)
                        {
                            result = data;
                            gotResult = true;
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Rnd.Next(0, ReconnectTime))).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }
            return result;
        }
    }
}