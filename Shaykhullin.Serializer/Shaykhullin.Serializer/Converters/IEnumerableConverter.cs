using Shaykhullin.Activator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shaykhullin.Serializer.Core
{
	internal class IEnumerableConverter : Converter<IEnumerable>
	{
		private readonly IActivator activator;
		private readonly ISerializer serializer;

		public IEnumerableConverter(ISerializer serializer, IActivator activator)
		{
			this.serializer = serializer;
			this.activator = activator;
		}

		public override IEnumerable Deserialize(Stream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(Stream stream, IEnumerable data)
		{
			var elements = data.Cast<object>().ToList();
			var elementType = data.GetType().GetGenericArguments()[0];

			stream.Write(BitConverter.GetBytes(elements.Count), 0, 4);

			foreach (var element in elements)
			{
				serializer.Serialize(stream, element, elementType);
			}
		}

		public override object DeserializeObject(Stream stream, Type type)
		{
			var elementType = type.GetGenericArguments()[0];

			var lengthBuffer = new byte[4];
			stream.Read(lengthBuffer, 0, 4);
			var length = BitConverter.ToInt32(lengthBuffer, 0);

			if(type.IsInterface)
			{
				type = typeof(List<>).MakeGenericType(elementType);
			}

			var list = (IList)activator.Create(type);

			for (int i = 0; i < length; i++)
			{
				list.Add(serializer.Deserialize(stream, elementType));
			}

			return list;
		}
	}
}
