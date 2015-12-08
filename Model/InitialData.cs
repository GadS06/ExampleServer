using System;
using System.ComponentModel;

namespace Model
{
	[Serializable]
	public class InitialData : INotifyPropertyChanged
	{
		public float StepLengthTime { set { _stepLengthTime = value; CallPropertyChangedEvent("StepLengthTime"); } get { return _stepLengthTime; } }
		public float StepLengthX { set { _stepLengthX = value; CallPropertyChangedEvent("StepLengthX"); } get { return _stepLengthX; } }
		public float StepLengthY { set { _stepLengthY = value; CallPropertyChangedEvent("StepLengthY"); } get { return _stepLengthY; } }

		public int NumOfStepsTime { set { _numOfStepsTime = value; CallPropertyChangedEvent("NumOfStepsTime"); } get { return _numOfStepsTime; } }
		public int NumOfStepsX { set { _numOfStepsX = value; CallPropertyChangedEvent("NumOfStepsX"); } get { return _numOfStepsX; } }
		public int NumOfStepsY { set { _numOfStepsY = value; CallPropertyChangedEvent("NumOfStepsY"); } get { return _numOfStepsY; } }

		public float PointX { set { _pointX = value; CallPropertyChangedEvent("PointX"); } get { return _pointX; } }
		public float PointY { set { _pointY = value; CallPropertyChangedEvent("PointY"); } get { return _pointY; } }

		public string ZeroFunc { set { _zeroFunc = value; CallPropertyChangedEvent("ZeroFunc"); } get { return _zeroFunc; } } // string потом запихивается в MathEvaluator
		public string BorderFunc { set { _borderFunc = value; CallPropertyChangedEvent("BorderFunc"); } get { return _borderFunc; } }

		private float _stepLengthTime;
		private float _stepLengthX;
		private float _stepLengthY;

		private int _numOfStepsTime;
		private int _numOfStepsX;
		private int _numOfStepsY;

		private float _pointX;
		private float _pointY;

		private string _zeroFunc;
		private string _borderFunc;

		[field: NonSerialized()]
		public event PropertyChangedEventHandler PropertyChanged;
		private void CallPropertyChangedEvent(string argPropertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(argPropertyName));
			}
		}
	}
}
