using System;
using System.Collections.Generic;

namespace CheckBox.Models
{
    public class Album
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string FolderName { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? EditTime { get; set; }

        public List<string> CheckPath { get; set; }
    }
}