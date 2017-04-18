using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Instrastructure.Helpers;
using Common.Models;
using Common.Models.InfoModels.TheMovieDb;

namespace Common.Instrastructure.SearchEngines.Engines
{
    public class TheMovieDbEngine : EngineBase
    {
        private static TheMovieDbConfiguration Configuration => ResourceHelper.Resources.TheMovieDbConfiguration;

        private static readonly Random Rnd = new Random();

        public override async Task<CommonInfoModel> GetMovieInfo(MovieModel movie)
        {
            var info = await GetCommonMovieInfoAsync(movie.FileNameWithoutExtension);

            if (info == null) return null;

            var poster = GetImageAsync(info.PosterUrl);
            var crew = GetCrewAsync(info);

            info.Poster = await poster;
            info.Participants = await crew;

            return info.ToCommonInfoModel();
        }

        private async Task<TheMovieDbInfoModel> GetCommonMovieInfoAsync(string query)
        {
            var attemptCnt = 0;
            var gotResult = false;
            TheMovieDbInfoModel result = null;

            var queryString = string.Format(Configuration.SearchUrlTemplate, Configuration.ApiKey, query);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < AttemptsCount)
                {
                    try
                    {
                        var data = await client.GetStringAsync(queryString);

                        if (data != null)
                        {
                            var results = JsonHelper.GetFirstInstance<List<TheMovieDbInfoModel>>("results", data);

                            if (results.Any())
                            {
                                result = results.First();
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

            var queryString = string.Format(Configuration.ImageUrlTemplate, posterUrl);

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

        private async Task<List<TheMovieDbParticipantInfo>> GetCrewAsync(TheMovieDbInfoModel info)
        {
            var attemptCnt = 0;
            var gotResult = false;
            dynamic result = null;

            var queryString = string.Format(Configuration.CreditsUrlTemplate, info.Id, Configuration.ApiKey);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < AttemptsCount)
                {
                    try
                    {
                        var data = await client.GetStringAsync(queryString);

                        if (data != null)
                        {
                            var results = JsonHelper.GetFirstInstance<IEnumerable<TheMovieDbParticipantInfo>>("cast", data)
                                    .Union(JsonHelper.GetFirstInstance<IEnumerable<TheMovieDbParticipantInfo>>("crew", data)).ToList();

                            if (results.Any())
                            {
                                result = results;
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
    }
}