namespace vidro.api.Common
{
    public class ErrorCode(int major, int minor, string message)
    {
        public int Major { get; set; } = major;

        public int Minor { get; set; } = minor;

        public string Message { get; set; } = message;

        public override string ToString()
        {
            return $"{Major + Minor}: {Message}";
        }
    }
}
