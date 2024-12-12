using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public RequiredAttribute(string errorMessage = "This field is required.")
        {
            ErrorMessage = errorMessage;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PriceRangeAttribute : Attribute
    {
        public decimal Min { get; }
        public decimal Max { get; }

        public PriceRangeAttribute(decimal min, decimal max)
        {
            Min = min;
            Max = max;
        }
    }
}
