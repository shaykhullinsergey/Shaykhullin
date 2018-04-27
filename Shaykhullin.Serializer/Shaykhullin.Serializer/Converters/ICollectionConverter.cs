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

			if (type.IsInterface)
			{
				type = typeof(Collection<>).MakeGenericType(elementType);
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
