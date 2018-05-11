using Shaykhullin.Activator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Shaykhullin.Stream;

namespace Shaykhullin.Serializer.Core
{
	internal class IListConverter : Converter<IList>
	{
		private static readonly Type OpenListType = typeof(List<>);
		
		private readonly IActivator activator;
		private readonly ISerializer serializer;

		public IListConverter(ISerializer serializer, IActivator activator)
		{
			this.activator = activator;
			this.serializer = serializer;
		}

		public override IList Deserialize(ValueStream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(ValueStream stream, IList elements)
		{
			var elementType = elements.GetType().GetGenericArguments()[0];

			var union = new UnifiedUnion(elements.Count);
			stream.WriteByte(union.Byte1);
			stream.WriteByte(union.Byte2);
			stream.WriteByte(union.Byte3);
			stream.WriteByte(union.Byte4);

			for (var i = 0; i < elements.Count; i++)
			{
				serializer.Serialize(stream, elements[i], elementType);
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

			if (type.IsInterface)
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
