namespace AdventureWorksCore.Api.Application.Models.Common
{
    public class ResultObject
    {
        private ResultObject(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        internal static ResultObject CreateSuccessResult()
        {
            return new ResultObject(success: true);
        }
    }
}
