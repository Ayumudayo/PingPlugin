using Dalamud.Logging;
using PingPlugin.GameAddressDetectors;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Lumina.Excel.GeneratedSheets;

namespace PingPlugin.PingTrackers
{
    public class PsPingPingTracker : PingTracker
    {
        public PsPingPingTracker(PingConfiguration config, GameAddressDetector addressDetector) : base(config, addressDetector, PingTrackerKind.PsPing)
        {
            detector = addressDetector;
        }

        protected override async Task PingLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (SeAddress != null)
                {
                    try
                    {
                        CurrentPort = detector.CurrentPort;
                        IPEndPoint endPoint = new IPEndPoint(SeAddress, CurrentPort);
                        //PluginLog.LogInformation(SeAddress.ToString());

                        var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        sock.Blocking = true;

                        var stopwatch = new Stopwatch();

                        // Measure the Connect call only
                        stopwatch.Start();
                        sock.Connect(endPoint);
                        stopwatch.Stop();

                        double t = stopwatch.Elapsed.TotalMilliseconds;
                        //Console.WriteLine("{0:0.00}ms", t);
                        NextRTTCalculation(Convert.ToUInt64(t));

                        sock.Close();
                    }
                    catch (Exception e)
                    {
                        PluginLog.LogError(e, "Error occurred when executing ping.");
                    }
                }

                //Thread.Sleep(3000);
                await Task.Delay(3000, token);

            }
        }

        private GameAddressDetector detector;
        private int CurrentPort = 54992; // Default Port number
    }
}