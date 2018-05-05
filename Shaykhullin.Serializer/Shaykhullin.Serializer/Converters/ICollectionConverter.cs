using Shaykhullin.Activator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class CollectionConverter : Converter<ICollection>
	{
		private static readonly Type OpenCollectionType = typeof(Collection<>);
		
		private readonly IActivator activator;
		private readonly ISerializer serializer;

		public CollectionConverter(ISerializer serializer, IActivator activator, ConverterContainer converterContainer)
		{
			this.serializer = serializer;
			this.activator = activator;
		}

		public override ICollection Deserialize(Stream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(Stream stream, ICollection elements)
		{
			var elementType = elements.GetType().GetGenericArguments()[0];

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

		public override object DeserializeObject(Stream stream, Type type)
		{
			var elementType = type.GetGenericArguments()[0];

			var length = new UnifiedUnion(
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte(),
				(byte)stream.ReadByte()).Int32;

			if (type.IsInterface)
			{
				type = OpenCollectionType.MakeGenericType(elementType);
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
