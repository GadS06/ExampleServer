using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using OxyPlot;

namespace ViewModel
{
	public class ViewModel : INotifyPropertyChanged
	{
		private const string STATUS_READY = "Doing nothing";
		private const string STATUS_COUNTING = "Counting...";

		// PlotModel property begin
		private OxyPlot.PlotModel _plotModel;
		public OxyPlot.PlotModel PlotModel
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
		// PlotModel property end

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

			StatusString = STATUS_READY;

			IsReadyToCount = true;

			//// создаём оси - X и Y
			//var xAxis = new OxyPlot.Axes.LinearAxis();
			//PlotModel.Axes.Add(xAxis);

			//var yAxis = new OxyPlot.Axes.LinearAxis();
			//PlotModel.Axes.Add(yAxis);

			//// создаём и размещаем линию на графике ака серия точек
			//var lineSerie = new OxyPlot.Series.LineSeries();
			//for (int i = 0; i < 10; i++)
			//{
			//	lineSerie.Points.Add(new DataPoint(i, i));
			//}
			//PlotModel.Series.Add(lineSerie);
		}

		public void Count()
		{
			StatusString = STATUS_COUNTING;
			IsReadyToCount = false;

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

			var lineSerie = new OxyPlot.Series.LineSeries();
			float time = 0;
			foreach (float f in CountingMethod.Result)
			{
				lineSerie.Points.Add(new DataPoint(time, f));
				time += timeStep;
			}
			PlotModel.Series.Add(lineSerie);

			StatusString = STATUS_READY;
			IsReadyToCount = true;


			//var lineSerie = new OxyPlot.Series.LineSeries();
			//for (int i = 0; i < 10; i++)
			//{
			//	lineSerie.Points.Add(new DataPoint(i, i * WindowData.PointX));
			//}
			//PlotModel.Series.Add(lineSerie);
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
