using System;

namespace ABB.Application.Common.Helpers
{
    public static class MoneyHelper
    {
        public static string ConvertToReportFormat(decimal? value)
        {
            return value == null ? "0" : value.Value.ToString("#,##0;(#,##0)");
        }
        
        public static decimal ConvertToDecimalFormat(string value)
        {
            return Convert.ToDecimal(value.Replace("(", "").Replace(")", ""));
        }
    }
}