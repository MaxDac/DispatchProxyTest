using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConsoleApp6.Decorators
{
	internal class ObjectAwaiter<TDecorated>
		where TDecorated : class
	{
		private readonly TDecorated? decorated;
		private readonly MethodInfo? targetMethod;
		private readonly object?[]? args;

		public ObjectAwaiter(TDecorated? decorated, MethodInfo? targetMethod, object?[]? args)
		{
			this.decorated = decorated;
			this.targetMethod = targetMethod;
			this.args = args;
		}

		public TaskAwaiter GetAwaiter()
		{
			var genericTaskType = targetMethod?.ReturnType?.GetGenericArguments()[0];

			if (genericTaskType == null)
			{
				throw new InvalidOperationException("The task is not generic");
			}

			var result = targetMethod?.Invoke(decorated, args);
			var getAwaiterMethod = genericTaskType.GetMethod(nameof(GetAwaiter));
			var uncastedResult = getAwaiterMethod?.Invoke(result, null);

			if (uncastedResult == null)
			{
				throw new InvalidOperationException("The awaiter is null");
			}

			return (TaskAwaiter)uncastedResult;
		}
	}
}
