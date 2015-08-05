using System;
using System.IO;

namespace PF.IFS.TongHua.DataReader
{
    internal abstract class FileBase
    {
        protected readonly string FilePaht;

         protected FileBase(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("filePath");
            }

            this.FilePaht = filePath;
        }
    }
}