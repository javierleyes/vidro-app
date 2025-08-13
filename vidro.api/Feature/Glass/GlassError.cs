using vidro.api.Common;

namespace vidro.api.Feature.Glass
{
    public static class GlassError
    {
        public static readonly ErrorCode InvalidPrice = new(MajorErrorCode.Glass, 1, "Price must be greater than 0");

        public static readonly ErrorCode GlassNotFound = new(MajorErrorCode.Glass, 2, "Glass not found");
    }
}
