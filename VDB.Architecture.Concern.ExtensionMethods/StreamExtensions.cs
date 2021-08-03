using System.IO;
using System.Threading.Tasks;

namespace VDB.Architecture.Concern.ExtensionMethods
{
    public static class StreamExtensions
    {
        public static async Task<string> ReadAsStringAsync(this Stream stream)
        {
            using StreamReader reader = new(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
