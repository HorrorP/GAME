namespace Script.Rail
{
	public interface IRailEvent
	{
		bool IsPerformed { get; }

		void Perform();
	}
}
