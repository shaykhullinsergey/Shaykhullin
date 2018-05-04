using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Shaykhullin.Network.Core;
using Xunit;

namespace Shaykhullin.Network.Tests
{
	public class UnitTest1
	{
		class Command : ICommand<int>
		{
			public IConnection Connection { get; }
			public int Message { get; set; }
		}

		class Handler : IHandler<int, Command>
		{
			public Task Execute(Command command)
			{
				command.Message = (int)Math.Sqrt(Math.Pow(command.Message, 2));
				return Task.CompletedTask;
			}
		}

		[Fact]
		public async void Test1()
		{
			var handlerDto = new HandlerDto(typeof(Handler));
			var command = new Command();
			var handler = new Handler() as object;

			var sw = Stopwatch.StartNew();
			for (int i = 0; i < 1_000_000; i++)
			{
				await handlerDto.ExecuteMethod(handler, command);
			}
			sw.Stop();
		}

		[Fact]
		public async void Test2()
		{
			var type = typeof(Handler);
			var handler = new Handler();
			var command = new object[]
			{
				new Command()
			};

			var sw = Stopwatch.StartNew();
			for (int i = 0; i < 1_000_000; i++)
			{
				await (Task)type.InvokeMember(
					"Execute",
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
					null,
					handler,
					command);
			}
			sw.Stop();
		}

		interface ITest
		{
			void Hello();
		}

		class TestClass : ITest
		{
			public void Hello()
			{
				Console.WriteLine();
			}
		}

		struct TestStruct : ITest
		{
			public void Hello()
			{
				Console.WriteLine();
			}
		}

		[Fact]
		public void PerformanceStructClassCastAsInterface()
		{
			var list = new List<ITest>();
			
			for (int i = 0; i < 100_00; i++)
			{
				var s = (ITest)new TestClass();
				s.Hello();
				list.Add(s);
			}
		}
	}
}