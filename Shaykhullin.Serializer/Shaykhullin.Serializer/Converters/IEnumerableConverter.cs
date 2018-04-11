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
		private readonly Configuration configuration;

		public IEnumerableConverter(ISerializer serializer, IActivator activator, Configuration configuration)
		{
			this.serializer = serializer;
			this.configuration = configuration;
			this.activator = activator;
		}

		public override IEnumerable Deserialize(Stream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(Stream stream, IEnumerable data)
		{
			var elementType = data.GetType().GetGenericArguments()[0];

			var alias = configuration.GetAliasFromType(elementType);
			stream.Write(BitConverter.GetBytes(alias), 0, 4);

			var elements = data.Cast<object>().ToList();

			stream.Write(BitConverter.GetBytes(elements.Count), 0, 4);

			foreach (var element in elements)
			{
				serializer.Serialize(stream, element, elementType);
			}
		}

		public override object DeserializeObject(Stream stream, Type type)
		{
			var aliasBytes = new byte[4];
			stream.Read(aliasBytes, 0, aliasBytes.Length);
			var alias = BitConverter.ToInt32(aliasBytes, 0);

			var elementType = configuration.GetTypeFromAlias(alias);

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
