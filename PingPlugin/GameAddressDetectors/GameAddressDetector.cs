using System.Net;

namespace PingPlugin.GameAddressDetectors
{
    public abstract class GameAddressDetector
    {
        protected IPAddress Address { get; set; }

        public abstract IPAddress GetAddress(bool verbose = false);

        public int CurrentPort = 54992;
    }
}