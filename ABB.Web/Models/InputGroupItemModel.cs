﻿using System.Collections.Generic;
using Kendo.Mvc.UI;

namespace ABB.Web.Models
{
    public class InputGroupItemModel : IInputGroupItem
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
        public string CssClass { get; set; }
        public bool? Enabled { get; set; }
        public bool? Encoded { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
    }
}