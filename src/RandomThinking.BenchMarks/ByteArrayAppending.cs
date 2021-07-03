using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RandomThinking.BenchMarks
{
    [SimpleJob(RuntimeMoniker.Net48, baseline: true)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    public class ByteArrayAppending
    {
        private byte[] _buffer;

        [Params(64, 128, 4096)]
        public int BufferSize { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _buffer = new byte[BufferSize];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(_buffer);
            }
        }

        [Benchmark(Baseline = true)]
        public byte[] ListByte_Add_DynamicSize()
        {
            var dst = new List<byte>();

            foreach (var b in _buffer)
            {
                dst.Add(b);
            }

            return dst.ToArray();
        }

        [Benchmark()]
        public byte[] ListByte_Add_PreAllocatedSize()
        {
            var dst = new List<byte>(8096);

            foreach (var b in _buffer)
            {
                dst.Add(b);
            }

            return dst.ToArray();
        }

        [Benchmark]
        public byte[] MemoryStream_Write_DynamicSize()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                foreach (var b in _buffer)
                {
                    ms.WriteByte(b);
                }

                return ms.ToArray();
            }
        }

        [Benchmark]
        public byte[] MemoryStream_Write_PreAllocatedSize()
        {
            using (var ms = new System.IO.MemoryStream(8096))
            {
                foreach (var b in _buffer)
                {
                    ms.WriteByte(b);
                }

                return ms.ToArray();
            }
        }
    }
}
