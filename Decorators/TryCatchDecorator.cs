using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApp6.Decorators
{
	public class TryCatchDecorator<TDecorated> : DispatchProxy
		where TDecorated : class
	{
		private TDecorated? decorated;

		private object? HandleException(Exception ex)
		{
			Console.WriteLine("Decorator caught the exception");
			Console.WriteLine(ex.ToString());
			return null;
		}

		private Expression<Func<object?>> CreateExpressionLambda(ObjectAwaiter<TDecorated> awaitable)
		{
			// Create parameter that will be passed to catch block
			var excepParam = Expression.Parameter(typeof(InvalidCastException));

			MethodInfo? handleExceptionMethodInfo = GetType().GetMethod(nameof(HandleException));
			MethodCallExpression returnMethodWithParameters2 = Expression.Call(Expression.Constant(), handleExceptionMethodInfo, excepParam);
			UnaryExpression returnMethodWithParametersAsObject2 = Expression.Convert(returnMethodWithParameters2, typeof(object));

			// Put created parameter before to CatchBlock.Variable using Expression.Catch
			// that takes the first argument as ParameterExpression
			TryExpression tryCatchMethod2 = TryExpression.TryCatch(returnMethodWithParametersAsObject, Expression.Catch(excepParam, returnMethodWithParametersAsObject2));
			var exppp = Expression.Lambda<Func<object, object, object>>(tryCatchMethod2, parameters);
			Func<object, object, object> func2 = Expression.Lambda<Func<object, object, object>>(tryCatchMethod2, parameters).Compile();
			object result2 = func2(20, "f"); // result = 100
		}

		protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
		{
			// Try catching
			var isTask = IsTask(targetMethod?.ReturnType);
			var isGeneric = targetMethod?.ReturnType?.IsGenericType == true;
			var genericTaskType = targetMethod?.ReturnType?.GetGenericArguments()[0];

			if (isTask)
			{
				// Generic Task, means that it returns a value
				if (isGeneric)
				{
					var awaitable = new ObjectAwaiter<TDecorated>(decorated, targetMethod, args);

					var expression = CreateExpressionLambda(awaitable);

					var method = typeof(Task).MakeGenericType(genericTaskType).GetMethod(nameof(Task.Run))?.MakeGenericMethod(genericTaskType);
					return method?.Invoke(null, new[] { expression.Compile() });
				}
				else
				{
					return Task.Run(async () =>
					{
						try
						{
							if (targetMethod?.Invoke(decorated, args) is not Task task)
							{
								throw new InvalidOperationException("Error while getting the task result");
							}

							await task;
						}
						catch (Exception ex)
						{
							Console.WriteLine("Decorator caught the exception");
							Console.WriteLine(ex.ToString());
						}
					}).GetAwaiter();
				}
			}
			else
			{
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
		}

		public static TDecorated Create(TDecorated decorated)
		{
			object proxy = Create<TDecorated, TryCatchDecorator<TDecorated>>();
			((TryCatchDecorator<TDecorated>)proxy).SetParameter(decorated);

			return (TDecorated)proxy;
		}

		private void SetParameter(TDecorated decorated) =>
			this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));

		private static bool IsTask(Type? type) => type?.Name == nameof(Task);
	}
}

