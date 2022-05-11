using System.Collections.Generic;

namespace CheckBox.Models
{
    public class Album
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public IEnumerable<string> Checks { get; set; }
    }
}