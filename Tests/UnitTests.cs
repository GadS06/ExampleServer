using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Collections.Generic;

namespace Tests
{
	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void Usual()
		{
			Server.Program.countingMethod = new Model.ParallelCountingMethod();

			string baseAddress = "http://localhost:9000/";

			using (WebApp.Start<Server.Startup>(url: baseAddress))
			{
				Model.InitialData inData = new Model.InitialData
				{
					BorderFunc = "0",
					ZeroFunc = "x*(1-x)*y*(1-y)",
					PointX = 0.5f,
					PointY = 0.5f,
					NumOfStepsX = 1000,
					NumOfStepsY = 1000,
					StepLengthX = 0.001f,
					StepLengthY = 0.001f,
					StepLengthTime = 0.001f,
					NumOfStepsTime = 1
				};

				HttpClient client = new HttpClient();

				client.PutAsJsonAsync(baseAddress + "api/count", inData).Wait();

				var response = client.GetAsync(baseAddress + "api/count").Result;

				var Result = response.Content.ReadAsAsync<List<float>>().Result;

				if (Result != null && Result.Count == 2) Assert.AreEqual(true, true);
				else Assert.Fail(Result.Count.ToString());
			}
		}
	}
}
