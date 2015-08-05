namespace Core.Infrastructure.Impl.Crosscutting
{
    using Core.Infrastructure.Crosscutting;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    internal class Serializer : ISerializer
    {
        public string JsonSerializer<T>(T t)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(t.GetType()).WriteObject(ms, t);
                return Encoding.UTF8.GetString(ms.ToArray());
            } 
        }

        public T JsonDeserialize<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }
    }
}
