using Datory.DatabaseImpl;
using SqlKata.Compilers;

namespace Datory.Utils
{
    internal class DmCompiler : OracleCompiler
    {
        public override string Wrap(string value)
        {
            return DmImpl.Wrap(value);
        }

        public override string CompileTrue()
        {
            return "1";
        }

        public override string CompileFalse()
        {
            return "0";
        }
    }
}