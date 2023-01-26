using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6.Dependencies
{
    public interface IDependency
    {
        public string Method(int parameter, List<string> otherArguments);

        public Task<string> MethodAsync(int parameter, List<string> otherArguments);
    }
}
