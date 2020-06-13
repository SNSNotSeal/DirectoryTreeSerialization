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

        private string serializedResult = string.Empty;

        private SimpleDirectory deserializedResult;

        public SimpleDirectory rootSimpleDirectory { get; set; }

        public string SerializedResult
        {
            get
            {
                return serializedResult;
            }
        }

        public SimpleDirectory DeserializedResult
        {
            get
            {
                return deserializedResult;
            }
        }

        SimpleSerialization() { }
        public SimpleSerialization(string root)
        {
            this.root = root;
            rootDirectoryInfo = new DirectoryInfo(root);
            serializedResult = Serialization();
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

        private string Serialization()
        {
            DataContractJsonSerializer formatter = new DataContractJsonSerializer(typeof(SimpleDirectory));
            SimpleDirectory rlt = ConstructDirectoryTree(rootDirectoryInfo,rootSimpleDirectory);
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.WriteObject(stream, rlt);
                result = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
            return result;
        }

        public void DeSerialization()
        {
            if (serializedResult == string.Empty)
            {
                throw new NullReferenceException("SerializedResult is empty!");
            }
            DataContractJsonSerializer formatter = new DataContractJsonSerializer(typeof(SimpleDirectory));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(serializedResult)))
            {
                SimpleDirectory outSimpleDirectory = formatter.ReadObject(stream) as SimpleDirectory;
                deserializedResult = outSimpleDirectory;
            }
        }

        public void SerializationToFile(string filePath)
        {
            if (SerializedResult != string.Empty)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(SerializedResult);
                        }
                    }
                    else
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(SerializedResult);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                
            }
            else
            {
                throw new NullReferenceException("SerializedResult is empty!");
            }
        }

        public void DeserializationFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string jsonStr = streamReader.ReadToEnd();
                    if (jsonStr == string.Empty)
                    {
                        throw new Exception($"this {filePath} is empty!");
                    }
                    DeSerialization();
                }
            }
            else
            {
                throw new FileNotFoundException($"this {filePath} is not exist!");
            }
        }
    }
}
