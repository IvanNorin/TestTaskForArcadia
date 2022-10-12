using System.Collections.Generic;
using ClassLibrary1.Models;

namespace ClassLibrary1
{
    public interface ITestlet
    {
        IEnumerable<Item> Randomize();
    }
}