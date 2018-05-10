using System;
using Microsoft.AspNetCore.Http;

namespace dotnetFun.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }

        public IFormFile File { get; set; }

        public string about { get; set; }

        public DateTime CreatedAt { get; set; }

        public string publicId { get; set; }

        public PhotoForCreationDto() {
            CreatedAt = DateTime.Now;
        }
    }
}