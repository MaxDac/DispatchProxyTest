using System.Reflection;

namespace ConsoleApp6.Decorators
{
    public class TryCatchDecorator<TDecorated> : DispatchProxy
        where TDecorated : class
    {
        private TDecorated? decorated;

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            // Try catching
            try
            {
                return targetMethod?.Invoke(decorated, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decorator caught the exception");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static TDecorated Create(TDecorated decorated)
        {
            object proxy = Create<TDecorated, TryCatchDecorator<TDecorated>>();
            ((TryCatchDecorator<TDecorated>)proxy).SetParameter(decorated);

            return (TDecorated)proxy;
        }

        private void SetParameter(TDecorated decorated) =>
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
    }
}
