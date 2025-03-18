using System.Collections.Generic;

namespace ABB.Web.Modules.Shared.Components.InputTags
{
    public class InputTagsModel
    {
        public string Name { get; set; }
        public string PlaceHolder { get; set; }
        public int ColumnCount { get; set; }
        public List<InputTagSourceItem> Items { get; set; }
    }
    public class InputTagSourceItem
    {
        public string ImageUrl { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsTeam { get; set; }
        public int Member { get; set; }

    }
}