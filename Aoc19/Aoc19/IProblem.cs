using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc19
{
    public interface IProblem
    {
        object Solve1(string input);

        object Solve2(string input);
    }
}
