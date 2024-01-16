using System.Linq;
using System.Net.Mime;
using Logger.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Logger.Web.Controllers
{
    /// <summary>
    /// Контроллер получения данных.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataLogs _dataLogs;
        private readonly ILogger _logger;

        /// <summary>
        /// Конструктор <see cref="DataController"/>
        /// </summary>
        /// <param name="dataLogs"><inheritdoc cref="IDataLogs"/></param>
        /// <param name="logger">Логгер.</param>
        public DataController(IDataLogs dataLogs, ILogger<DataController> logger)
            : this(dataLogs, (ILogger)logger)
        {
        }

        /// <summary>
        /// Конструктор <see cref="DataController"/>
        /// </summary>
        /// <param name="dataLogs"><inheritdoc cref="IDataLogs"/></param>
        /// <param name="logger">Логгер.</param>
        private DataController(IDataLogs dataLogs, ILogger logger)
        {
            _dataLogs = dataLogs;
            _logger = logger;
        }

        /// <summary>
        /// Получить данные.
        /// </summary>
        /// <param name="lastId">Последний загруженный Id.</param>
        /// <returns></returns>
        [HttpGet("get_data")]
        public IActionResult GetData(int? lastId)
        {
            var sendData = _dataLogs.GetData();
            if (lastId != null)
            {
                sendData = sendData.Where(v => v.Id > lastId);
            }

            sendData = sendData.OrderBy(v => v.Id);

            return new JsonResult(
                new
                {
                    Data = sendData
                        .Select(
                            v => new
                            {
                                Id = v.Id,
                                Value = v.Value
                            }
                        )
                        .ToArray()
                }
            );
        }

        /// <summary>
        /// Получить CSV файл.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_file")]
        public IActionResult GetFile()
        {
            var steam = _dataLogs.GetFile();
            return File(steam, MediaTypeNames.Application.Octet, "loggs.csv");
        }
    }
}
