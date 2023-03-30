using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsGOStateEmitter.Models.SkinkiDriver
{
    public class UploadFileRequest
    {
        public string Path { get; set; }
        public List<byte[]> BytesFiles { get; set; }
    }
}
