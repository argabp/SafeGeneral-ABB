using System;
using System.Collections.Generic;

namespace ABB.Application.Common.Helpers
{
    public static class ReportHelper
    {
        public static bool StartsWithSpace(string s)
            => !string.IsNullOrEmpty(s) && s.StartsWith(" ");
        
        public static string FormatIf(bool condition, decimal value, bool percentage = false)
            => condition && value != 0
                ? ConvertToReportFormat(value)
                : string.Empty;
        
        
        public static string ConvertToReportFormat(decimal? value, bool percentage = false)
        {
            if (value == null)
            {
                return percentage ? "0.0000" : "0.00";
            }

            return percentage
                ? value.Value.ToString("#,##0.0000;(#,##0.0000)")
                : value.Value.ToString("#,##0.00;(#,##0.00)");
        }
        public static string ConvertToReportFormat(string mataUang, decimal? value)
        {
            return value == null ? "0" : mataUang.ToLower().Contains("rp") ? value.Value.ToString("#,##0.00;(#,##0.00)") : value.Value.ToString("#,##0.0000;(#,##0.0000)");
        }
        
        public static decimal ConvertToDecimalFormat(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? 0 : Convert.ToDecimal(value.Replace("(", "-").Replace(")", ""));
        }

        public static string ConvertDateTime(DateTime? value, string format)
        {
            
            return value == null ? string.Empty : value.Value.ToString(format);
        }
        
        public static string BuildSection(decimal? triggerValue, string template, Dictionary<string, object> values)
        {
            // Only render if triggerValue is > 0
            if (triggerValue is null || triggerValue <= 0)
                return string.Empty;

            string result = template;

            // Replace placeholders dynamically
            foreach (var kvp in values)
            {
                result = result.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString() ?? string.Empty);
            }

            return result;
        }
    }
}