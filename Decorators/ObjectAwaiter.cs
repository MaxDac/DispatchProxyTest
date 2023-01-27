using System.Runtime.CompilerServices;

namespace ConsoleApp6.Decorators
{
	internal class ObjectAwaiter
	{
		private readonly object? thisIsATaskISwear;
		private readonly Type genericTaskType;

		public ObjectAwaiter(object? thisIsATaskISwear, Type genericTaskType)
		{
			this.thisIsATaskISwear = thisIsATaskISwear;
			this.genericTaskType = genericTaskType;
		}

		public TaskAwaiter GetAwaiter()
		{
			var getAwaiterMethod = this.genericTaskType.GetMethod(nameof(GetAwaiter));
			var uncastedResult = getAwaiterMethod?.Invoke(this.thisIsATaskISwear, null);

			if (uncastedResult == null)
			{
				throw new InvalidOperationException("The awaiter is null");
			}

			return (TaskAwaiter)uncastedResult;
		}
	}
}
