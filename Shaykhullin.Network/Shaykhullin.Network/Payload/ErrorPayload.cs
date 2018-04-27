using System;

namespace Shaykhullin.Network.Core
{
	internal class ErrorPayload : Payload
	{
		private static readonly Type ErrorCommandType = typeof(Error);
		
		public ErrorPayload(string reason)
		{
			CommandType = ErrorCommandType;
			Data = new ErrorInfo
			{
				Reason = reason
			};
		}
	}
}