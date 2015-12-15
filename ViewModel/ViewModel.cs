using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Model;
using OxyPlot;
using System.Linq;

namespace ViewModel
{
	public class ViewModel : INotifyPropertyChanged
	{
		private const string StatusReady = "Doing nothing";
		private const string StatusCounting = "Counting...";

		private DBContext dbContext;

		public ObservableCollection<int> SavedResults { get; set; } 

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

		private bool _isReadyToSave;
		public bool IsReadyToSave
		{
			get { return _isReadyToSave; }
			set
			{
				_isReadyToSave = value;
				CallPropertyChangedEvent("IsReadyToSave");
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

				IsReadyToSave = IsReadyToCount;
			}
		}

		public ViewModel()
		{
			// создаём график
			PlotModel = new PlotModel();

			WindowData = new InitialData();

			CountingMethod = new ParallelCountingMethod();
			//CountingMethod = new SimpleCountingMethod();

			dbContext = new DBContext();

			SavedResults = new ObservableCollection<int>();
			UpdateSavedResultsList();

			StatusString = StatusReady;

			IsReadyToCount = true;
			IsReadyToSave = false;
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
			UpdatePlot();

			// возвращаем возможность считать что-то ещё
			StatusString = StatusReady;
			IsReadyToCount = true;
		}

		private void UpdatePlot()
		{
			float timeStep = WindowData.StepLengthTime;
			var lineSerie = new OxyPlot.Series.LineSeries();
			float time = 0;
			foreach (float f in CountingMethod.Result)
			{
				lineSerie.Points.Add(new DataPoint(time, f));
				time += timeStep;
			}
			PlotModel.Series.Add(lineSerie);
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

		public void SaveCurrent()
		{
			
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream initialDataMS = new MemoryStream();
			MemoryStream dataMS = new MemoryStream();

			if (CountingMethod.Result == null)
			{
				StatusString = "Data is empty! Nothing to save.";
				return;
			}

			bf.Serialize(initialDataMS,WindowData);
			bf.Serialize(dataMS,CountingMethod.Result);

			var e = new Entity { InitialData = initialDataMS.GetBuffer(), Data = dataMS.GetBuffer()}; // БЛОБ
			dbContext.EntitySet.Add(e);
			dbContext.SaveChanges();

			UpdateSavedResultsList();
		}

		private void UpdateSavedResultsList()
		{
			var query = from elem in dbContext.EntitySet
				orderby elem.Id
				select elem.Id;

			SavedResults.Clear();
			foreach (var elem in query)
			{
				SavedResults.Add(elem);
			}
		}

		public void RemoveById(int id)
		{
			var e = dbContext.EntitySet.Find(id);
			dbContext.EntitySet.Remove(e);
			dbContext.SaveChanges();

			UpdateSavedResultsList();
		}

		public void LoadById(int id)
		{
			var e = dbContext.EntitySet.Find(id);

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream initialDataMS = new MemoryStream(e.InitialData);
			MemoryStream dataMS = new MemoryStream(e.Data);

			WindowData = (InitialData)bf.Deserialize(initialDataMS);
			CountingMethod.Result = (List<float>)bf.Deserialize(dataMS);

			UpdatePlot();
			IsReadyToSave = true;
		}
	}
}
