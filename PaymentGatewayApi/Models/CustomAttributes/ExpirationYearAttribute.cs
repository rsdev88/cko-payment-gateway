using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApi.Models.CustomAttributes
{
    public class ExpirationYearAttribute : RangeAttribute
    {
        public ExpirationYearAttribute()
            : base(typeof(string), DateTime.Now.ToString("yy"), DateTime.Now.AddYears(15).ToString("yy")) { }
    }
}
