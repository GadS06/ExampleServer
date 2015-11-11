using System;
using System.ComponentModel;
using Model;
using OxyPlot;

namespace ViewModel
{
	public class ViewModel : INotifyPropertyChanged
	{
		private const string StatusReady = "Doing nothing";
		private const string StatusCounting = "Counting...";

		private PlotModel _plotModel;
		public PlotModel PlotModel
		{
			get
			{
				return _plotModel;
			}
			set
			{
				_plotModel = value;
				CallPropertyChangedEvent("PlotModel");
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void CallPropertyChangedEvent(string argPropertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(argPropertyName));
			}
		}

		private InitialData _windowData;
		public InitialData WindowData
		{
			get { return _windowData; }
			set
			{
				_windowData = value;
				CallPropertyChangedEvent("WindowData");
			}
		}

		private ICountingMethod CountingMethod { set; get; }

		private string _statusString;
		public string StatusString
		{
			get { return _statusString; }
			set
			{
				_statusString = value;
				CallPropertyChangedEvent("StatusString");
			}
		}

		private bool _isReadyToCount;
		public bool IsReadyToCount
		{
			get { return _isReadyToCount; }
			set
			{
				_isReadyToCount = value;
				CallPropertyChangedEvent("IsReadyToCount");
			}
		}

		public ViewModel()
		{
			// создаём график
			PlotModel = new PlotModel();

			WindowData = new InitialData();

			CountingMethod = new ParallelCountingMethod();
			//CountingMethod = new SimpleCountingMethod();

			StatusString = StatusReady;

			IsReadyToCount = true;
		}

		public void Count()
		{
			// закрываем возможность считать что-то ещё
			StatusString = StatusCounting;
			IsReadyToCount = false;

			// считаем
			CountingMethod.Data = WindowData;
			float timeStep = WindowData.StepLengthTime;
			try
			{
				CountingMethod.CountGraph();
			}
			catch (Exception exception)
			{
				StatusString = exception.Message;
				IsReadyToCount = true;
				return;
			}

			// забрасываем результат в график
			var lineSerie = new OxyPlot.Series.LineSeries();
			float time = 0;
			foreach (float f in CountingMethod.Result)
			{
				lineSerie.Points.Add(new DataPoint(time, f));
				time += timeStep;
			}
			PlotModel.Series.Add(lineSerie);

			// возвращаем возможность считать что-то ещё
			StatusString = StatusReady;
			IsReadyToCount = true;
		}

		public void ExampleFill()
		{
			WindowData.PointX = 0.5f;
			WindowData.PointY = 0.5f;
			WindowData.NumOfStepsTime = 1000;
			WindowData.StepLengthTime = 0.001f;
			WindowData.NumOfStepsX = 1000;
			WindowData.StepLengthX = 0.001f;
			WindowData.NumOfStepsY = 1000;
			WindowData.StepLengthY = 0.001f;
			WindowData.ZeroFunc = "x*(1-x)*y*(1-y)";
			WindowData.BorderFunc = "0";
		}
	}
}
