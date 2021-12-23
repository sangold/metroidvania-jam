using Newtonsoft.Json;

public static class JsonSerializer
{
    public static string Serialize(object data)
    {
        var settings = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.SerializeObject(data, settings);
    }

    public static T Deserialize<T>(object data)
    {
        return JsonConvert.DeserializeObject<T>(data.ToString());
    }
}
