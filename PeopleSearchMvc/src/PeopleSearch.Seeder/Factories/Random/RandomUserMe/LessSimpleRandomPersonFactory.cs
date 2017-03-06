using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using BigCompany.Contracts;
using Newtonsoft.Json;

namespace PeopleSearch.Seeder.Factories.Random.RandomUserMe
{
    public class LessSimpleRandomPersonFactory : IRandomPersonFactory
    {
        private const string ApiUri = "https://randomuser.me/api/?inc=picture,nat,name,dob&?nat=gb,au,ca,gb";
        private const string ApiDateFormat = "yyyy-MM-dd HH:mm:ss";

        public Person Create(int seed)
        {
            var json = GetJsonProfileFromApi();
            dynamic profile = JsonConvert.DeserializeObject(json);

            var firstNameTrimmed = profile.results[0].name.first.ToString().Trim('{', '}');
            var lastNameTrimmed = profile.results[0].name.last.ToString().Trim('{', '}');
            var dobTrimmed = profile.results[0].dob.ToString().Trim('{', '}');
            var imageUrlTrimmed = profile.results[0].picture.large.ToString().Trim('{', '}');
            
            var person = new Person();
            person.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstNameTrimmed);
            person.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastNameTrimmed);
            person.DateOfBirth = DateTime.ParseExact(dobTrimmed, ApiDateFormat, new DateTimeFormatInfo());
            person.ImageBase64 = GetImageAsBase64Url(imageUrlTrimmed);
            person.Interests = StaticData.RandomInterests(seed);

            return person;
        }

        private static string GetJsonProfileFromApi()
        {
            using (var client = new HttpClient())
            {
                var getTask = client.GetStringAsync(ApiUri);
                Task.WaitAll(getTask);
                return getTask.Result;
            }
        }

        private static string GetImageAsBase64Url(string url)
        {
            using (var client = new HttpClient())
            {
                var getTask = client.GetByteArrayAsync(url);
                Task.WaitAll(getTask);
                return "image/jpeg;base64," + Convert.ToBase64String(getTask.Result);
            }
        }
    }
}
