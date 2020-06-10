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
            string root = @"D:\工具集\dependency walker";
            SimpleSerialization simpleSerialization = new SimpleSerialization(root);

            Console.WriteLine(simpleSerialization.Serialization());
        }
    }
}
