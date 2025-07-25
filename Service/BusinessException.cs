namespace Service
{
    public class BusinessException : Exception
    {
        /// <summary>
        /// Is saved into the log
        /// </summary>
        public bool IsSaved { get; set; }
        public string Details { get; set; }
        public BusinessException(string message, string details = "", bool isSaved = false) : base(message)
        {
            IsSaved = isSaved;
            Details = details;
        }

        public BusinessException(string message, Exception e, string details = "", bool isSaved = false) : base(message, e)
        {
            IsSaved = isSaved;
            Details = details;
        }
        public override string ToString()
        {
            var message = $"BusinessException: {Message}. Details: {Details}. StackTrace: \n {StackTrace}";
            return message;
        }
    }
}
