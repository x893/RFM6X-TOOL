namespace SemtechLib.Devices.SX1231.Enumerations
{
	public enum ExitConditionEnum
	{
		OFF,
		FallingEdgeFifoNotEmpty,
		RisingEdgeFifoLevel,
		RisingEdgeCrcOk,
		RisingEdgePayloadReadyOrTimeout,
		RisingEdgeSyncAddressOrTimeout,
		RisingEdgePacketSent,
		RisingEdgeTimeout,
	}
}