using CheckBox.Constants;
using System;
using System.Collections.Generic;

namespace CheckBox.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FolderName { get; set; }

        public string Description { get; set; }

        public string ThumbnailName { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? EditTime { get; set; }

        public List<Images> Images { get; set; }

        public List<string> ImagesToRemove { get; set; }

        public string ThumbnailFullPath => AppConstants.UserDirectory + $"{FolderName}/{ThumbnailName}";
    }
}