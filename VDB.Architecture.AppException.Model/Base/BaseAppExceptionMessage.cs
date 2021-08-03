namespace VDB.Architecture.AppException.Model
{
    public abstract record BaseAppExceptionMessage
    {
        public string Code { get; set; }
        public string Message { get; set; }

        protected BaseAppExceptionMessage(string code)
        {
            Code = code;
        }

        protected BaseAppExceptionMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
