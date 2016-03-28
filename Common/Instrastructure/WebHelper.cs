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

        private static async Task<string> GetKinopoiskIdFromQimbdByNameAsync(Movie movie)
        {
            var attemptCnt = 0;
            var gotResult = false;
            string result = null;
            var queryString = ResourceHelper.Resources.QimdbUrl + string.Format(QimbdParamString, movie.FileNameWithoutExtension);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < 3)
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
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private static async Task<string> GetKinopoiskIdFromUnOfficialApiBaseByNameAsync(Movie movie)
        {
            var attemptCnt = 0;
            var gotResult = false;
            string result = null;

            var queryString = ResourceHelper.Resources.KinopoiskApiUrl +
                              string.Format(KinopoiskSearchFilmByNameParamString,
                                  movie.FileNameWithoutExtension.Replace(' ', '_'));

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < 3)
                {
                    try
                    {
                        var data = await
                            client.GetStringAsync(queryString);

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
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private static async Task<KinopoiskInfo> GetKinopoiskInfoFromUnOfficialApiBaseByIdAsync(string id)
        {
            var attemptCnt = 0;
            var gotResult = false;
            KinopoiskInfo result = null;

            var queryString = ResourceHelper.Resources.KinopoiskApiUrl + string.Format(KinopoiskSearchFilmByIdParamString, id);

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < 3)
                {
                    try
                    {
                        var data = await
                            client.GetStringAsync(queryString);

                        var info = JsonConvert.DeserializeObject<KinopoiskInfo>(data);

                        if (info != null)
                        {
                            result = info;
                            gotResult = true;
                        }
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }

            return result;
        }

        private static async Task<byte[]> GetImageByInfo(KinopoiskInfo info)
        {
            var attemptCnt = 0;
            var gotResult = false;
            byte[] result = null;

            var queryString = ResourceHelper.Resources.YandexImagesUrl + info.PosterUrl;

            using (var client = new HttpClient())
            {
                while (!gotResult && attemptCnt < 3)
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
                        await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                    }

                    attemptCnt++;
                }
            }
            return result;
        }

        public static async Task<KinopoiskInfo> GetMovieInfo(Movie movie)
        {
            var kinopoiskId = await GetKinopoiskIdFromUnOfficialApiBaseByNameAsync(movie) ??
                              await GetKinopoiskIdFromQimbdByNameAsync(movie);

            if (kinopoiskId == null) return null;

            var info = await GetKinopoiskInfoFromUnOfficialApiBaseByIdAsync(kinopoiskId);

            if (info == null) return null;

            info.RelatedFileName = movie.FileNameWithoutExtension;
            var image = await GetImageByInfo(info);
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
