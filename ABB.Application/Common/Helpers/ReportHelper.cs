using System;

namespace ABB.Application.Common.Helpers
{
    public static class ReportHelper
    {
        public static string ConvertToReportFormat(decimal? value, bool percentage = false)
        {
            return value == null ? "0" : percentage ? value.Value.ToString("#,##0.0000;(#,##0.0000)") : value.Value.ToString("#,##0.00;(#,##0.00)");
        }
        
        public static decimal ConvertToDecimalFormat(string value)
        {
            return Convert.ToDecimal(value.Replace("(", "-").Replace(")", ""));
        }

        public static string ConvertDateTime(DateTime? value, string format)
        {
            
            return value == null ? string.Empty : value.Value.ToString(format);
        }
    }
}