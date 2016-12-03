using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace VisualGrading.Helpers
{
    public static class JSONSerialization
    {

        public static T DeserializeJSON<T>(string fileLocation)
        {
            string JSONstring = string.Empty;
            try
            {
                JSONstring = File.ReadAllText(fileLocation);
            }
            catch
            {

            }
            return JsonConvert.DeserializeObject<T>(JSONstring);
        }

        public static async Task<T> DeserializeJSONAsync<T>(string fileLocation)
        {
            var result = await Task.Factory.StartNew(
                () =>

                {
                    //TODO: this is exact same as the nonAsync method. see if we can jsut call the method without a 'void' error
                    string JSONstring = string.Empty;
                    try
                    {
                        JSONstring = File.ReadAllText(fileLocation);
                    }
                    catch
                    {

                    }
                    return JsonConvert.DeserializeObject<T>(JSONstring);
                }

            );

            return result;
        }

        public static void SerializeJSON(string fileLocation, object objectToSerialize)
        {
            string serializedObject = JsonConvert.SerializeObject(objectToSerialize);
            File.WriteAllText(fileLocation, serializedObject);
        }

        public static async Task SerializeJSONAsync(string fileLocation, object objectToSerialize)
        {
            await Task.Factory.StartNew(
            () =>
                {
                    SerializeJSON( fileLocation, objectToSerialize);
                }
            );
        }
    }
}
