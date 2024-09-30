using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Net;
using Telemetry_Simulator.Classes;

namespace Telemetry_Simulator.Services
{

    //utilizing IDisposable for the UDP client
    public class SimulatorService(ILogger<SimulatorService> logger, BitwiseService bitwiseService)
    {
        private CancellationTokenSource source;
        private CancellationToken token;
        private readonly ILogger<SimulatorService> _logger = logger;
        private readonly BitwiseService _bitwiseService = bitwiseService;

        public async Task Start()
        {
            await Task.Run(() =>
            {
                if (source != null && source.IsCancellationRequested)
                {
                    source.Dispose();
                }
                source = new CancellationTokenSource();
                token = source.Token;
                string data = "";
                while (token.CanBeCanceled) {
                    _bitwiseService.ResetParams();
                    List<BitwiseDTO> icd = _bitwiseService.LoadICDData();
                    foreach (BitwiseDTO icdLine in icd)
                    {
                        bool isLocationChanged = _bitwiseService.Encode(icdLine);
                        if (isLocationChanged)
                        {
                            data += _bitwiseService.ConvertToBinary();
                        }

                    }
                    byte[] dataBytes = _bitwiseService.ConvertToBytes(data);
                    _logger.LogInformation($"{dataBytes}");

                    //UDP sending
                    Producer.SendEncodedData(dataBytes);
                }
            });
        }

        public void Stop()
        {
            if (source != null && token.CanBeCanceled)
                source.Cancel();
        }
    }
}
