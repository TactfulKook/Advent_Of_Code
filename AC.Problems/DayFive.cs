using AC.Services.DataReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC.Problems;

public class DayFive(ITextFileReader textFileReader) : IProblem
{
    private readonly ITextFileReader _textFileReader = textFileReader;

    public void Solve(int half)
    {
        throw new NotImplementedException();
    }
}
