using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PeopleSearch.Seeder.Publishers.File
{
    public class FilePublisher : IPublisher
    {
        private readonly FileStream _destinationFileStream;

        public FilePublisher(string destinationFile)
        {
            var file = new FileInfo(destinationFile);
            if (file.Exists)
            {
                file.Delete();
            }

            _destinationFileStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        }

        public async Task Publish<T>(T person)
        {
            var jsonPerson = JsonConvert.SerializeObject(person);
            byte[] encodedText = Encoding.Unicode.GetBytes(jsonPerson);

            await _destinationFileStream.WriteAsync(encodedText, 0, encodedText.Length);
        }

        public async Task Publish<T>(T person, CancellationToken cancellationToken)
        {
            var jsonPerson = JsonConvert.SerializeObject(person);
            byte[] encodedText = Encoding.Unicode.GetBytes(jsonPerson);

            await _destinationFileStream.WriteAsync(encodedText, 0, encodedText.Length, cancellationToken);
        }
    }
}