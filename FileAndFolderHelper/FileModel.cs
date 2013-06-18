using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelper
{
    public class FileModel
    {
        private string readableSize = string.Empty;

        public string FileName { get; set; }

        public string Size
        {
            get
            {
                if (!string.IsNullOrEmpty(readableSize))
                {
                    return readableSize;
                }

                if (this.ByteSize < 1024L)
                {
                    readableSize = string.Format("{0} Bytes", ByteSize);
                    return readableSize;
                }
                else if (this.ByteSize < 1048576L)
                {
                    readableSize = string.Format("{0} Kilobytes", ByteSize / 1024);
                    return readableSize;
                }
                else if (this.ByteSize < 1073741824L)
                {
                    readableSize = string.Format("{0} Megabytes", ByteSize / 1048576L);
                    return readableSize;
                }
                else
                {
                    readableSize = string.Format("{0} Gigabytes", (ByteSize / 1073741824L));
                    return readableSize;
                }
            }
        }

        public long ByteSize { get; set; }

        public string FullFileName { get; set; }

        public bool IsDuplicated { get; set; }
    }
}
