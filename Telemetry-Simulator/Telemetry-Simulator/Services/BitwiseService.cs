using Telemetry_Simulator.Classes;
using Newtonsoft.Json;

namespace Telemetry_Simulator.Services
{
    public class BitwiseService
    {
        private static int byteData = 0;
        private static int bit = 0;
        public static int location = 0;
        public List<BitwiseDTO> GetData() {
            string jsonFilePath = File.ReadAllText("jsons/FlightBoxDownICD.json");
            List<BitwiseDTO> list =  JsonConvert.DeserializeObject<List<BitwiseDTO>>(jsonFilePath);
            foreach (BitwiseDTO item in list) {
                Console.WriteLine(item.Bit);
            }
            return list;
        }

        public bool Encode(BitwiseDTO data)
        {
            int bytenum = new Random().Next(data.Min, data.Max+1);
            int masknum = Convert.ToInt32(data.Mask, 2);
            bytenum = bytenum & masknum;
            ChangeBit(data.Bit);
            byteData = byteData | bytenum;
            bit += data.Bit;
            if (bit < 8)
                return false;
            bit = 0;
            location++;
            return true;
        }

        public void ChangeBit(int bitCount) {
            bit += bitCount;
        }
    }
}
