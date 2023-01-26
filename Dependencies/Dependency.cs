namespace ConsoleApp6.Dependencies
{
    public class Dependency : IDependency
    {
        public string Method(int parameter, List<string> otherArguments) =>
            throw new InvalidOperationException("Test exception");

        public async Task<string> MethodAsync(int parameter, List<string> otherArguments)
        {
            await Task.Delay(1000);
            return "something";
        }
    }
}
