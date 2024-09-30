using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Tracing;
using System.Net;
using System.Net.Sockets;
using System.Text.Encodings;
using Telemetry_Simulator.Classes;
using Telemetry_Simulator.Services;

namespace Telemetry_Simulator.Controllers
{
    public class SimulatorController(SimulatorService simulatorService) : Controller
    {
        private readonly SimulatorService _simulatorService = simulatorService;

        [HttpGet("StartSimulator")]
        public async Task Start()
        {
            await _simulatorService.Start();
        }

        [HttpGet("StopSimulator")]
        public void Stop()
        {
            _simulatorService.Stop();
        }
    }
}
