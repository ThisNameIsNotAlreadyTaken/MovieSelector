using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Models;
using Newtonsoft.Json;

namespace Common.Instrastructure
{
    public static class WebHelper
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int connDescription, int reservedValue);

        private const string QimbdParamString = "?title={0}&format=json";

        private static readonly Regex QimbdRegex = new Regex("kinopoisk_id\\\":[0-9]+,");

        private static readonly Regex NumberRegex = new Regex("[0-9]+");

        private static readonly Regex KinopoiskIdByNameRegex = new Regex("id\\\":\\\"[0-9]+\\\",");

        private const string KinopoiskSearchFilmByNameParamString = "/searchFilms?keyword={0}";

        private const string KinopoiskSearchFilmByIdParamString = "/getFilm?filmID={0}";

        private static readonly string DateParam = string.Format("&date={0}", DateTime.Now.ToString("dd.MM.yyyy"));

        private static async Task<string> GetKinopoiskIdFromQimbdByNameAsync(Movie movie, int attemptCount, int reconnectTime)
        {
            var attemptCnt = 0;
            var gotResult = false;
            string result = null;
            var queryString = ResourceHelper.Resources.QimdbUrl + string.Format(QimbdParamString, movie.FileNameWithoutExtension);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < attemptCount)
                {
                    try
                    {
                        var data = await
                            client.GetStringAsync(queryString);

                        var potentialIds = QimbdRegex.Matches(data);

                        if (potentialIds.Count > 0)
                        {
                            var idMatch = NumberRegex.Matches(potentialIds[0].Value);

                            if (idMatch.Count > 0)
                            {
                                result = idMatch[0].Value;
                                gotResult = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(reconnectTime)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private static async Task<string> GetKinopoiskIdFromUnOfficialApiBaseByNameAsync(Movie movie, int attemptCount, int reconnectTime)
        {
            var attemptCnt = 0;
            var gotResult = false;
            string result = null;

            var queryString = ResourceHelper.Resources.KinopoiskApiUrl +
                              string.Format(KinopoiskSearchFilmByNameParamString,
                                  movie.FileNameWithoutExtension.Replace(' ', '_'));

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < attemptCount)
                {
                    try
                    {
                        var data = await
                            client.GetStringAsync(queryString);

                        if (data != null && data != "null")
                        {
                            var potentialIds = KinopoiskIdByNameRegex.Matches(data);

                            if (potentialIds.Count > 0)
                            {
                                var idMatch = NumberRegex.Matches(potentialIds[0].Value);

                                if (idMatch.Count > 0)
                                {
                                    result = idMatch[0].Value;
                                    gotResult = true;
                                }
                            }
                        }
                        else
                        {
                            queryString += DateParam;
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(reconnectTime)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private static async Task<KinopoiskInfo> GetKinopoiskInfoFromUnOfficialApiBaseByIdAsync(string id, int attemptCount, int reconnectTime)
        {
            var attemptCnt = 0;
            var gotResult = false;
            KinopoiskInfo result = null;

            var queryString = ResourceHelper.Resources.KinopoiskApiUrl + string.Format(KinopoiskSearchFilmByIdParamString, id);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < attemptCount)
                {
                    try
                    {
                        var data = await
                            client.GetStringAsync(queryString);

                        if (data != null && data != "null")
                        {
                            var info = JsonConvert.DeserializeObject<KinopoiskInfo>(data);

                            if (info != null)
                            {
                                result = info;
                                gotResult = true;
                            }
                        }
                        else
                        {
                            queryString += DateParam;
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(reconnectTime)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private static async Task<byte[]> GetImageByInfo(KinopoiskInfo info, int attemptCount, int reconnectTime)
        {
            var attemptCnt = 0;
            var gotResult = false;
            byte[] result = null;

            if (info.PosterUrl == null) return null;

            var queryString = ResourceHelper.Resources.YandexImagesUrl + info.PosterUrl.Replace("iphone90", "iphone360");

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < attemptCount)
                {
                    try
                    {
                        var data = await client.GetByteArrayAsync(queryString);

                        if (data != null && data.Length > 0)
                        {
                            result = data;
                            gotResult = true;
                        }
                        else
                        {
                            queryString += DateParam;
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(reconnectTime)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }
            return result;
        }

        public static async Task<KinopoiskInfo> GetMovieInfo(Movie movie, int attemptsCount = 3, int reconnectTime = 10)
        {
            var kinopoiskId = await GetKinopoiskIdFromUnOfficialApiBaseByNameAsync(movie, attemptsCount, reconnectTime) ??
                              await GetKinopoiskIdFromQimbdByNameAsync(movie, attemptsCount, reconnectTime);

            if (kinopoiskId == null) return null;

            var info = await GetKinopoiskInfoFromUnOfficialApiBaseByIdAsync(kinopoiskId, attemptsCount, reconnectTime);

            if (info == null) return null;

            info.RelatedFileName = movie.FileNameWithoutExtension;
            var image = await GetImageByInfo(info, attemptsCount, reconnectTime);
            info.Poster = image;

            return info;
        }

        public static bool IsInternetConnectionAvailable
        {
            get
            {
                int desc;
                return InternetGetConnectedState(out desc, 0);
            }
        }
    }
}
