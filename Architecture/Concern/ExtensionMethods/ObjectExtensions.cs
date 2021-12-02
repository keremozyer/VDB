using Newtonsoft.Json;

namespace VDB.Architecture.Concern.ExtensionMethods
{
    public static class ObjectExtensions
    {
        public static string SerializeAsJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static T DeepCopy<T>(this T obj)
        {
            if (obj == null) return obj;

            return obj.SerializeAsJson().DeserializeJSON<T>();
        }
    }
}
