﻿namespace Shaykhullin.Network.Core
{
	public interface IMessage
	{
		int EventId { get; }
		byte[] Data { get; }
	}
}