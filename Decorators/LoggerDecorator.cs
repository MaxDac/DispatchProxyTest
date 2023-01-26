using System.Reflection;

namespace ConsoleApp6.Decorators
{
    public class LoggerDecorator<TDecorated> : DispatchProxy
        where TDecorated : class
    {
        private TDecorated? decorated;

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            // Logging parameters
            var methodName = targetMethod?.Name;
            IEnumerable<string?> parameters = targetMethod?.GetParameters().Select(p => p.Name) ?? Array.Empty<string>();

            Console.WriteLine($"Method name: {methodName}");
            Console.WriteLine($"Parameters: {string.Join(',', parameters)}");

            return targetMethod?.Invoke(decorated, args);
        }

        public static TDecorated Create(TDecorated decorated)
        {
            object proxy = Create<TDecorated, LoggerDecorator<TDecorated>>();
            ((LoggerDecorator<TDecorated>)proxy).SetParameter(decorated);

            return (TDecorated)proxy;
        }

        private void SetParameter(TDecorated decorated) =>
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
    }
}
