using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace VDB.Architecture.Concern.Helper
{
    public static class CompressionHelpers
    {
        public static IEnumerable<Stream> OpenCompressedFileStreamAsStreams(Stream compressedStream)
        {
            return new ZipArchive(compressedStream)?.Entries?.Select(e => e?.Open());
        }
    }
}
