using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;

namespace DirectoryTreeSerialization
{
    class SimpleSerialization
    {
        private string root;

        private DirectoryInfo rootDirectoryInfo;

        public SimpleDirectory rootSimpleDirectory { get; set; }

        public string SerializedResult { set; get; }

        public SimpleDirectory DeserializedResult { set; get; }

        SimpleSerialization() { }
        public SimpleSerialization(string root)
        {
            this.root = root;
            rootDirectoryInfo = new DirectoryInfo(root);
        }

        public SimpleDirectory ConstructDirectoryTree(DirectoryInfo directoryInfo,SimpleDirectory simpleDirectory)
        {
            simpleDirectory = new SimpleDirectory() { name = directoryInfo.Name, childDirectories = new List<SimpleDirectory>(), childFiles = new List<SimpleFile>() };
            if (directoryInfo.GetDirectories().Length == 0)
            {
                simpleDirectory.childDirectories = null;
                //    return;
            }
            else
            {
                foreach (DirectoryInfo d in directoryInfo.GetDirectories())
                {
                    SimpleDirectory child = new SimpleDirectory() { name = d.Name };

                    simpleDirectory.childDirectories.Add(ConstructDirectoryTree(d, child));
                }
            }

            if (directoryInfo.GetFiles().Length != 0)
            {
                foreach (FileInfo f in directoryInfo.GetFiles())
                {
                    SimpleFile simpleFile = new SimpleFile() { name = f.Name };
                    simpleDirectory.childFiles.Add(simpleFile);
                }
            }
            else
            {
                simpleDirectory.childFiles = null;
            }
            return simpleDirectory;
        }

        public string Serialization()
        {
            DataContractJsonSerializer formatter = new DataContractJsonSerializer(typeof(SimpleDirectory));
            SimpleDirectory rlt = ConstructDirectoryTree(rootDirectoryInfo,rootSimpleDirectory);
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.WriteObject(stream, rlt);
                result = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
            SerializedResult = result;
            return result;
        }

        public SimpleDirectory DeSerialization()
        {
            DataContractJsonSerializer formatter = new DataContractJsonSerializer(typeof(SimpleDirectory));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(SerializedResult)))
            {
                SimpleDirectory outSimpleDirectory = formatter.ReadObject(stream) as SimpleDirectory;
                DeserializedResult = outSimpleDirectory;
                return outSimpleDirectory;
            }
        }
    }
}
