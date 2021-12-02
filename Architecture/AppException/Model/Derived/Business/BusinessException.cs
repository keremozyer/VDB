namespace VDB.Architecture.AppException.Model.Derived.Business
{
    public class BusinessException : BaseAppException
    {
        public BusinessException(string code)
        {
            this.Messages = new BusinessExceptionMessage[] { new BusinessExceptionMessage(code) };
        }
    }
}
