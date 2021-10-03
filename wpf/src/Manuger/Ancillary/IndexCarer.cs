
namespace Manuger.Ancillary
{
	internal static class IndexCarer
	{
		internal static void Decrement(ref int index, int lowerBound)
		{
			index--;
			if (index < lowerBound)
			{
				index = lowerBound;
			}
		}

		internal static void Increment(ref int index, int upperBound)
		{
			index++;
			if (index > upperBound)
			{
				index = upperBound;
			}
		}
	}
}
