using System.Text.Json;

namespace samsungweb.Extensions
{
    public static class SessionExtensions
    {
        // Hàm chuyển Object (List giỏ hàng) thành chuỗi JSON để lưu vào Session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Hàm dịch chuỗi JSON từ Session trở lại thành Object
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}