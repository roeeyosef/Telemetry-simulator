using Telemetry_Simulator.Classes;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;
using Microsoft.Extensions.Logging;

namespace Telemetry_Simulator.Services
{
    public class BitwiseService(ILogger<BitwiseService> logger)
    {
        private static int byteData = 0;
        private static int bit = 0;
        public static int location = 0;
        private readonly ILogger<BitwiseService> _logger = logger;

        public string ReadJSONFromRandomICDFile() {
            string down = "FlightBoxDownICD";
            string up = "FlightBoxUpICD";
            int FlightBoxDownICD = 1;
            int FlightBoxUpICD = 2;
            int filenum = new Random().Next(FlightBoxDownICD, FlightBoxUpICD+1);
            return (filenum == FlightBoxDownICD) ? File.ReadAllText($"jsons/{down}.json") : File.ReadAllText($"jsons/{up}.json");
        }

        public List<BitwiseDTO> LoadICDData() {
            string jsonFilePath = ReadJSONFromRandomICDFile();
            List<BitwiseDTO> dataList = JsonConvert.DeserializeObject<List<BitwiseDTO>>(jsonFilePath);
            foreach (BitwiseDTO item in dataList) {
                _logger.LogInformation($"{item}");
            }
            return dataList;
        }

        public bool Encode(BitwiseDTO dataRow)
        {
            int bytenum = new Random().Next(dataRow.Min, dataRow.Max+1);
            int masknum = (dataRow.Mask != null && dataRow.Mask != "") ? Convert.ToInt32(dataRow.Mask, 2) : 255; // 255 = 11111111
            bytenum = bytenum & masknum;
            byteData = byteData | bytenum;
            bit += dataRow.Bit;
            _logger.LogInformation($"byteData: {byteData}");
            if (bit < 8)
                return false;
            bit = 0;
            location++;
            return true;
        }

        public string ConvertToBinary() {
            byte binary = (byte)byteData;
            string strbinary = Convert.ToString(binary, 2).PadLeft(8, '0');
            byteData = 0; // resets for the next location
            _logger.LogInformation($"binary representation: {strbinary}");
            return strbinary;
        }

        public byte[] ConvertToBytes(string byteString) { 
            int byteCount = byteString.Length / 8;
            byte[] bytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++) {
                string byteSubString = byteString.Substring(8 * i, 8);
                bytes[i] = Convert.ToByte(byteSubString, 2);
            }
            return bytes;
        }

        public void ResetParams() {
            location = 0;
            bit = 0;
            byteData = 0;
        }
    }
}