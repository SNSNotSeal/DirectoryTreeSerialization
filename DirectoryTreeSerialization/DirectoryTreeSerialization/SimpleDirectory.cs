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

        public string convertToStr(int count)
        {
            StringBuilder rlt = new StringBuilder(name);
            string tabs = printDoubleSpaces();
            rlt.Append($"\n{tabs}|");

            if (childDirectories != null)
            {
                count++;
                foreach (SimpleDirectory sd in childDirectories)
                {
                    rlt.Append("\n" + tabs);
                    rlt.Append($"|-");
                    rlt.Append(sd.convertToStr(count));
                }
            }
            if(childFiles != null)
            {
                foreach(SimpleFile sf in childFiles)
                {
                    rlt.Append("\n" + tabs);
                    rlt.Append($"|-{sf.name}");
                }
            }

            return rlt.ToString();

            string printDoubleSpaces()
            {
                StringBuilder _tabs = new StringBuilder();
                for(int i = 0; i < count; i++)
                {
                    _tabs.Append("  ");
                }
                return _tabs.ToString();
            }
        }
        
        public void print()
        {
            Console.WriteLine(convertToStr(0));
        }
    }

    [DataContract]
    class SimpleFile
    {
        [DataMember]
        public string name;
    }
}
