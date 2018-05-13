using System;
using System.Collections.Generic;

namespace Network.Core
{
	internal class DataTypeConfiguration
	{
		private readonly Dictionary<Type, CommandConfiguration> dataTypes;

		public DataTypeConfiguration()
		{
			dataTypes = new Dictionary<Type, CommandConfiguration>();
		}
		
		public CommandConfiguration EnsureDataType(Type dataType)
		{
			if (!dataTypes.TryGetValue(dataType, out var dataTypeConfiguration))
			{
				dataTypeConfiguration = new CommandConfiguration();
				dataTypes.Add(dataType, dataTypeConfiguration);
			}

			return dataTypeConfiguration;
		}
		
		public CommandConfiguration DataType(Type dataType)
		{
			return dataTypes[dataType];
		}
	}
}