namespace Script.Rail
{
	public interface ICancelableRailEvent : IRailEvent
	{
		void RequestCancel();
	}
}
