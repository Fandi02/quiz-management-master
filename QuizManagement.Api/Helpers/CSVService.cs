using CsvHelper;
using System.Globalization;
using System.Text;

namespace QuizManagement.Api.Helpers
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
        public MemoryStream WriteCSV<T>(List<T> records);
    }

    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>();
            return records;
        }

        public MemoryStream WriteCSV<T>(List<T> records)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, true))
            {
                csvWriter.WriteRecords(records);
            }

            streamWriter.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
