using DDDPratical.Domain.Commom.Base;
using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Commom.Validations;

namespace DDDPratical.Domain.ValueObjects
{
    public sealed class ReasonCancellation : ValueObject
    {
        public string Code { get; private set; }
        public string Description { get; private set; }
        public ReasonCancellation(string code)
        {
            Guard.AgainstNullOrWhiteSpace(code, nameof(code));
            Guard.Against<DomainException>(_predefinedDescriptions.ContainsKey(code), "Invalid cancellation code");

            Description = _predefinedDescriptions[code];
            Code = code;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
            yield return Code;
        }

        public override string ToString()
        {
            return $"{Description}";
        }

        private static readonly Dictionary<string, string> _predefinedDescriptions = new()
        {
            { "OUT_OF_STOCK", "The item is out of stock." },
            { "CUSTOMER_REQUEST", "Cancellation requested by the customer." },
            { "PAYMENT_ISSUE", "There was an issue with payment." },
            { "FRAUD_SUSPECTED", "Suspicion of fraudulent activity." },
            { "INVALID_ADDRESS", "The provided delivery address is invalid." },
            { "OTHER", "Other Descriptions." }
        };

        public static ReasonCancellation OutOfStock => new("OUT_OF_STOCK");
        public static ReasonCancellation CustomerRequest => new("CUSTOMER_REQUEST");
        public static ReasonCancellation PaymentIssue => new("PAYMENT_ISSUE");
        public static ReasonCancellation FraudSuspected => new("FRAUD_SUSPECTED");
        public static ReasonCancellation InvalidAddress => new("INVALID_ADDRESS");  
        public static ReasonCancellation Other => new("OTHER");

    }
}
