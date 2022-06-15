﻿using System;

namespace SharedLayer.Dto
{
    public class AlbumDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailName { get; set; }

        public string FolderName { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
