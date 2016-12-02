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

        public static async Task<T> DeserializeJSONAsync<T>(string fileLocation)
        {
            var result = await Task.Factory.StartNew(
                () =>
                
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
                
            );
            return result;
        }

        public static async Task SerializeJSONAsync(string fileLocation, object objectToSerialize)
        {
            await Task.Factory.StartNew(
            () =>
                {
                    string serializedObject = JsonConvert.SerializeObject(objectToSerialize);
                    File.WriteAllText(fileLocation, serializedObject);
                }
            );
        }
    }
}
