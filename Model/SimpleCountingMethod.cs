using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class SimpleCountingMethod : BaseCountingMethod, ICountingMethod
	{
		public void CountGraph()
		{
			Result = new List<float>();

			InputCheck();
			InitialSet();

			int normalizedPointX = Convert.ToInt32(Data.PointX / Data.StepLengthX);
			int normalizedPointY = Convert.ToInt32(Data.PointY / Data.StepLengthY);

			for (int t = 0; t < Data.NumOfStepsTime; t++)
			{
				Result.Add(OldGrid[Convert.ToInt32(normalizedPointX), Convert.ToInt32(normalizedPointY)]);

				for (int i = 1; i < Data.NumOfStepsX - 1; i++) MainCycleInside(i);

				EdgesSet();

				OldGrid = NewGrid;

				// statusString = "Counting: " + (((float)t / (float)Data.NumOfStepsTime) * 100f).ToString() + "%";
			}

			Result.Add(OldGrid[Convert.ToInt32(normalizedPointX), Convert.ToInt32(normalizedPointY)]);
		}
	}
}
