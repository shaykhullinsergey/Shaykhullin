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
		private readonly Configuration configuration;

		public CollectionConverter(ISerializer serializer, IActivator activator, Configuration configuration)
		{
			this.serializer = serializer;
			this.configuration = configuration;
			this.activator = activator;
		}

		public override ICollection Deserialize(Stream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(Stream stream, ICollection data)
		{
			var elementType = data.GetType().GetGenericArguments()[0];

			stream.Write(BitConverter.GetBytes(data.Count), 0, 4);

			foreach (var element in data)
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
