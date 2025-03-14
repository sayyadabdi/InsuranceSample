using System.Net;

namespace InsuranceSample.Exceptions
{
    public class ValidationException : BaseException
    {

        public ValidationException(string message, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
            : base(message, statusCode) { }
    }
}
