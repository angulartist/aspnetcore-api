using System;

namespace dotnetFun.API.Dtos
{
    public class PhotosForDetailsDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string About { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool isMain { get; set; }
    }
}