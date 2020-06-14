using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTreeSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            //  string root = @"D:\工具集\dependency walker";
            string root = @"G:\OS images";
            SimpleSerialization simpleSerialization = new SimpleSerialization(root);

            //Console.WriteLine(simpleSerialization.SerializedResult);

            simpleSerialization.DeSerialization();

            simpleSerialization.DeserializedResult.print();

            simpleSerialization.SerializationToFile(@"H:\test.txt");
            Console.WriteLine("Write to file successfully");

            Console.WriteLine();

            simpleSerialization.DeserializationFromFile(@"H:\test.txt");

            Console.WriteLine("Deserialization from file:");

            simpleSerialization.DeserializedResult.print();

        }
    }
}
