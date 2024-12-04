using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC.Services.DataReaders;

public interface ITextFileReader
{
    string ReadFile(string fileName);
}
