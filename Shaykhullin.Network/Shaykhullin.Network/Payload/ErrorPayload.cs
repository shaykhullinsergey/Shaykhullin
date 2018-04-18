namespace Shaykhullin.Network.Core
{
	internal class ErrorPayload : Payload
	{
		public ErrorPayload(string reason)
		{
			Event = typeof(Error);
			Data = new ErrorInfo
			{
				Reason = reason
			};
		}
	}
}