using System;
using System.IO;

namespace Shaykhullin.Serializer.Core
{
	internal class ArrayConverter : Converter<Array>
	{
		private readonly ISerializer serializer;
		private readonly Configuration configuration;

		public ArrayConverter(ISerializer serializer, Configuration configuration)
		{
			this.serializer = serializer;
			this.configuration = configuration;
		}

		public override Array Deserialize(Stream stream)
		{
			throw new InvalidOperationException();
		}

		public override void Serialize(Stream stream, Array data)
		{
			var elementType = data.GetType().GetElementType();

			stream.Write(BitConverter.GetBytes(data.Length), 0, 4);

			foreach (var element in data)
			{
				serializer.Serialize(stream, element, elementType);
			}
		}

		public override object DeserializeObject(Stream stream, Type type)
		{
			var elementType = type.GetElementType();

			var lengthBuffer = new byte[4];
			stream.Read(lengthBuffer, 0, 4);
			var length = BitConverter.ToInt32(lengthBuffer, 0);

			var array = Array.CreateInstance(elementType, length);

			for (int i = 0; i < length; i++)
			{
				array.SetValue(serializer.Deserialize(stream, elementType), i);
			}

			return array;
		}
	}
}
