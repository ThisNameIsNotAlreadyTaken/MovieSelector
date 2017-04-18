using System.Runtime.InteropServices;

namespace Common.Instrastructure.Helpers
{
    public static class WebHelper
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int connDescription, int reservedValue);

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