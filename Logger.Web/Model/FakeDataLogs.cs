using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Logger.Web.Model
{
    /// <summary>
    /// Генератор фейковых данных.
    /// </summary>
    public class FakeDataLogs : IDataLogs
    {
        private static readonly Encoding Encoding_1251 = Encoding.GetEncoding(1251);
        private const int MIN_VALUE = 20;
        private const int MAX_VALUE = 27;

        private readonly ILogger _logger;
        private BlockingCollection<(int Id, int Value)> _data;
        private Timer _timer;
        private Random _random;

        /// <summary>
        /// Конструктор <see cref="FakeDataLogs"/>.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        public FakeDataLogs(ILogger<FakeDataLogs> logger)
            : this((ILogger)logger)
        {
        }

        /// <summary>
        /// Конструктор <see cref="FakeDataLogs"/>.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        private FakeDataLogs(ILogger logger)
        {
            _logger = logger;
            _data = new BlockingCollection<(int Id, int Value)>();
            _timer = new Timer(state => AddDataItem(), null, new TimeSpan(0), TimeSpan.FromSeconds(2));
            _random = new Random();
        }

        /// <summary>
        /// Диструктор.
        /// </summary>
        ~FakeDataLogs()
        {
            _timer.Dispose();
        }

        /// <inheritdoc />
        public IEnumerable<(int Id, int Value)> GetData()
        {
            lock (_data)
            {
                return _data.ToArray();
            }
        }

        /// <inheritdoc /> 
        public Stream GetFile()
        {
            return GetFile(";");
        }

        private Stream GetFile(string splitter)
        {
            var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding_1251, 1024 * 1024, true);
            lock (_data)
            {
                foreach (var value in _data)
                {
                    writer.WriteLine($"{value.Id}{splitter}{value.Value}");
                }
            }

            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private void AddDataItem()
        {
            lock (_data)
            {
                _data.Add((Id: _data.Count, Value: _random.Next(MIN_VALUE, MAX_VALUE)));
            }
        }
    }
}
