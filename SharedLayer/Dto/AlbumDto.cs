using System;

namespace SharedLayer.Dto
{
    public class AlbumDto
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public string FolderName { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
