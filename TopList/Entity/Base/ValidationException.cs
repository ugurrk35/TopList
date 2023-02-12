using System.ComponentModel.DataAnnotations;

namespace TopList.Entity.Base
{
    public class ValidationException : Exception
    {
        public ValidationException(Type target, IList<ValidationResult> validationResults)
        {
            TargetType = target;
            ValidationResults = validationResults;
        }

        public IList<ValidationResult> ValidationResults { get; }

        public Type TargetType { get; }

        public override string Message
        {
            get
            {
                return string.Concat(TargetType.ToString(), ": ", string.Join(';', ValidationResults.Select(x => $"{x.ErrorMessage}")));
            }
        }
    }
}
