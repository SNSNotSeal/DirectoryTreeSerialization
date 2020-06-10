using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DirectoryTreeSerialization
{
    [DataContract]
    class SimpleDirectory
    {
        [DataMember]
        public List<SimpleDirectory> childDirectories;

        [DataMember]
        public string name;

        [DataMember]
        public List<SimpleFile> childFiles;
    }

    [DataContract]
    class SimpleFile
    {
        [DataMember]
        public string name;
    }
}
