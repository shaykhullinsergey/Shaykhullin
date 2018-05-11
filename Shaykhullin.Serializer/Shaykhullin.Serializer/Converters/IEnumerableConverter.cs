using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Shaykhullin.Activator;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class IEnumerableConverter : Converter<IEnumerable>
	{
		private static readonly Type OpenListType = typeof(List<>);
		
		private readonly IActivator activator;
		private readonly ISerializer serializer;

		public IEnumerableConverter(ISerializer serializer, IActivator activator)
		{
			this.serializer = serializer;
			this.activator = activator;
		}

		public override IEnumerable Deserialize(ValueStream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(ValueStream stream, IEnumerable data)
		{
			var elements = data.Cast<object>().ToList();
			var elementType = data.GetType().GetGenericArguments()[0];

			var union = new UnifiedUnion(elements.Count);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);

			foreach (var element in elements)
			{
				serializer.Serialize(stream, element, elementType);
			}
		}

		public override object DeserializeObject(ValueStream stream, Type type)
		{
			var elementType = type.GetGenericArguments()[0];

			var length = new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int32;

			if(type.IsInterface)
			{
				type = OpenListType.MakeGenericType(elementType);
			}

			var list = (IList)activator.Create(type);

			for (var i = 0; i < length; i++)
			{
				list.Add(serializer.Deserialize(stream, elementType));
			}

			return list;
		}
	}
}
