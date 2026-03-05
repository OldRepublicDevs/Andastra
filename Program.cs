
using System;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.IO;

class Program {
    static void Main() {
        using var fs = new FileStream(@"src\BioWare\bin\Debug\net48\BioWare.dll", FileMode.Open, FileAccess.Read);
        using var pe = new PEReader(fs);
        var metadata = pe.GetMetadataReader();
        foreach (var handle in metadata.AssemblyReferences) {
            var reference = metadata.GetAssemblyReference(handle);
            Console.WriteLine(metadata.GetString(reference.Name));
        }
    }
}

