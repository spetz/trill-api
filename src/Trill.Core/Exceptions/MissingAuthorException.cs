namespace Trill.Core.Exceptions
{
    public class MissingAuthorException : CustomException
    {
        public override string Code { get; } = "missing_author";
        
        public MissingAuthorException() : base("Missing author.")
        {
        }
    }
}