using vidro.api.Common;

namespace vidro.api.Feature.Visit
{
    public static class VisitErrors
    {
        public static readonly ErrorCode DateIsRequired = new(MajorErrorCode.Visit, 1, "Date is required");

        public static readonly ErrorCode DateCannotBeInThePast = new(MajorErrorCode.Visit, 2, "Date cannot be in the past");

        public static readonly ErrorCode NameIsRequired = new(MajorErrorCode.Visit, 3, "Name is required");

        public static readonly ErrorCode NameCannotExceedLength = new(MajorErrorCode.Visit, 4, "Name cannot exceed 20 characters");

        public static readonly ErrorCode AddressIsRequired = new(MajorErrorCode.Visit, 5, "Address is required");

        public static readonly ErrorCode AddressCannotExceedLength = new(MajorErrorCode.Visit, 6, "Address cannot exceed 50 characters");

        public static readonly ErrorCode PhoneIsRequired = new(MajorErrorCode.Visit, 7, "Phone is required");

        public static readonly ErrorCode InvalidStatus = new(MajorErrorCode.Visit, 8, "Invalid visit status value");
    }
}
