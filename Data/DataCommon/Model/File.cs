using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommon.Model
{
    public class FileRashev: BaseDeletableModel<int>, IFile
    {
        public byte[] Content { get; set; }
        public string UntrustedName { get; set; }
        public string NewTrustedName { get; set; }
        public long Size { get; set; }
        public string MIMEtype { get; set; }

    }
}
