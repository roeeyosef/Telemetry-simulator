using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings;
using Telemetry_Simulator.Classes;
using Telemetry_Simulator.Services;

namespace Telemetry_Simulator.Controllers
{
    public class SimulatorController : Controller
    {
        private CancellationTokenSource source;
        private CancellationToken token;
        private readonly BitwiseService bitwiseService = new BitwiseService();

        [HttpGet("StartSimulator")]
        public async Task Start()
        {
            // starts running a simulation, disposes of the existing cancellation token if exists, and creates a new one
            // while loop for the async task runs as long as the stop command wasn't executed
            if (source != null && source.IsCancellationRequested) {
                source.Dispose();
            }
            source = new CancellationTokenSource();
            token = source.Token;
            while (!token.IsCancellationRequested) {
                List<BitwiseDTO> icd = bitwiseService.GetData();
                foreach (BitwiseDTO icdLine in icd) {
                    //TODO: figure out how to do the UDP transfer to the parse

                }
            }
        }

        [HttpGet("StopSimulator")]
        public void Stop()
        {
            if (source != null)
                source.Cancel();
        }
    }
}
