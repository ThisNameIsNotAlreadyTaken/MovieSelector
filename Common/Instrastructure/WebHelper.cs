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

        private static readonly string DateParam = $"&date={DateTime.Now.ToString("dd.MM.yyyy")}";

        private static bool ParseKinopoiskIdFromQimdbResult(string data, ref string id)
        {
            var potentialIds = QimbdRegex.Matches(data);

            if (potentialIds.Count <= 0) return false;

            var idMatch = NumberRegex.Matches(potentialIds[0].Value);

            if (idMatch.Count <= 0) return false;

            id = idMatch[0].Value;

            return true;
        }

        private static async Task<string> GetKinopoiskIdFromQimbdByNameAsync(Movie movie, int attemptCount, int reconnectTime)
        {
            var attemptCnt = 0;
            var gotResult = false;
            string result = null;

            var parameter = movie.FileNameWithoutExtension;
            var translitParameter = Transliter.TranslitEnToRu(parameter);

            var queryString = ResourceHelper.Resources.QimdbUrl + string.Format(QimbdParamString, parameter);
            var translitQueryString = ResourceHelper.Resources.QimdbUrl + string.Format(QimbdParamString, translitParameter);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < attemptCount)
                {
                    try
                    {
                        var data = await client.GetStringAsync(queryString);
                        gotResult = ParseKinopoiskIdFromQimdbResult(data, ref result);

                        if (!gotResult)
                        {
                            data = await client.GetStringAsync(translitQueryString);
                            gotResult = ParseKinopoiskIdFromQimdbResult(data, ref result);
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

        private static bool ParseKinopoiskIdFromResult(string data, ref string id)
        {
            if (data == null || data == "null") return false;

            var potentialIds = KinopoiskIdByNameRegex.Matches(data);

            if (potentialIds.Count <= 0) return false;

            var idMatch = NumberRegex.Matches(potentialIds[0].Value);

            if (idMatch.Count <= 0) return false;

            id = idMatch[0].Value;

            return true;
        }

        private static async Task<string> GetKinopoiskIdFromUnOfficialApiBaseByNameAsync(Movie movie, int attemptCount, int reconnectTime)
        {
            var attemptCnt = 0;
            var gotResult = false;
            string result = null;

            var parameter = movie.FileNameWithoutExtension.Replace(' ', '_');
            var translitParameter = Transliter.TranslitEnToRu(parameter);

            var queryString = ResourceHelper.Resources.KinopoiskApiUrl +
                              string.Format(KinopoiskSearchFilmByNameParamString, parameter);

            var translitQueryString = ResourceHelper.Resources.KinopoiskApiUrl +
                                      string.Format(KinopoiskSearchFilmByNameParamString, translitParameter);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < attemptCount)
                {
                    try
                    {
                        var data = await client.GetStringAsync(queryString);
                        gotResult = ParseKinopoiskIdFromResult(data, ref result);

                        if (!gotResult)
                        {
                            data = await client.GetStringAsync(translitQueryString);
                            gotResult = ParseKinopoiskIdFromResult(data, ref result);
                        }

                        if (!gotResult)
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
