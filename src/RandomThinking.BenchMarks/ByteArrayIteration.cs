using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RandomThinking.BenchMarks
{
    [SimpleJob(RuntimeMoniker.Net48, baseline: true)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //[SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    public class ByteArrayIteration
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
        public void ForEach_Iteration()
        {
            var ms = new System.IO.MemoryStream();

            foreach (var b in _buffer)
            {
                ms.WriteByte(b);
            }
        }

        [Benchmark]
        public void For_Iteration()
        {
            var ms = new System.IO.MemoryStream();

            for (var i = 0; i < _buffer.Length; i++)
            {
                ms.WriteByte(_buffer[i]);
            }
        }

        [Benchmark]
        public void Stream_Read()
        {
            var ms = new System.IO.MemoryStream();

            int read = 0;
            using (var sr = new System.IO.MemoryStream(_buffer))
            {
                while (read != -1)
                {
                    read = sr.ReadByte();
                    if (read == -1)
                    {
                        break;
                    }

                    ms.WriteByte((byte)read);
                }
            }
        }
    }
}
