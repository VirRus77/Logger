using System.Collections.Generic;
using System.IO;

namespace Logger.Web.Model
{
    /// <summary>
    /// Данные.
    /// </summary>
    public interface IDataLogs
    {
        /// <summary>
        /// Получить перечисление данных.
        /// </summary>
        /// <returns></returns>
        IEnumerable<(int Id, int Value)> GetData();

        /// <summary>
        /// Получить поток для файла.
        /// </summary>
        /// <returns>Поток записи в файл.</returns>
        Stream GetFile();
    }
}
