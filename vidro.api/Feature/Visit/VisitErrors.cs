using vidro.api.Common;

namespace vidro.api.Feature.Visit
{
    public class VisitErrors
    {
        public static ErrorCode DateIsRequired = new ErrorCode(MajorErrorCode.Visit, 1, "Date is required");

        public static ErrorCode DateCannotBeInThePast = new ErrorCode(MajorErrorCode.Visit, 2, "Date cannot be in the past");

        public static ErrorCode NameIsRequired = new ErrorCode(MajorErrorCode.Visit, 3, "Name is required");

        public static ErrorCode NameCannotExceedLength = new ErrorCode(MajorErrorCode.Visit, 4, "Name cannot exceed 20 characters");

        public static ErrorCode AddressIsRequired = new ErrorCode(MajorErrorCode.Visit, 5, "Address is required");

        public static ErrorCode AddressCannotExceedLength = new ErrorCode(MajorErrorCode.Visit, 6, "Address cannot exceed 50 characters");

        public static ErrorCode PhoneIsRequired = new ErrorCode(MajorErrorCode.Visit, 7, "Phone is required");
    }
}
