using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ValidatorAPI.DomainModels
{
    [DataContract]
    public class File
    {
        [DataMember(Name = "lastModified")]
        public long LastModified { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "webkitRelativePath")]
        public string WebkitRelativePath { get; set; }
        [DataMember(Name = "path")]
        public string Path { get; set; } 
    }
}
