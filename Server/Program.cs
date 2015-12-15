using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace Server
{
	public class Program
	{
		static public Model.ICountingMethod countingMethod { get; set; }

		static void Main(string[] args)
		{
			countingMethod = new Model.ParallelCountingMethod();

			string baseAddress = "http://localhost:9000/";

			using (WebApp.Start<Startup>(url: baseAddress))
			{
				Console.ReadLine();
			}
		}
	}
}
