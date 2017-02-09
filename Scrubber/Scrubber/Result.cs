namespace Scrubber
{
    public class Result<T>
    {
        public static Result<T> CreateSuccess(T item)
        {
            return new Result<T>(true, item);
        }

        private Result(bool success, T resultValue, string errorMessage = "")
        {
            ErrorMessage = errorMessage;
            Success = success;
            ResultValue = resultValue;
        }

        public bool Success { get; set; }
        public T ResultValue { get; set; }

        public static Result<T> CreateFail(T item = default(T), string errorMessage = "")
        {
            return new Result<T>(false, item, errorMessage);
        }


        public static Result<T> CreateFail(T item)
        {
            return new Result<T>(false, item, "");
        }
        public static Result<T> CreateFail(string errorMessage)
        {
            return new Result<T>(false, default(T), errorMessage);
        }

        public string ErrorMessage { get; set; }
    }
}