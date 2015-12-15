using System.Collections.Generic;
using System.Web.Http;

namespace Server
{
	// api/count --- "count" берётся автоматически из имени этого класса
	public class CountController : ApiController
	{
		public List<float> Get()
		{
			Program.countingMethod.CountGraph();
			return Program.countingMethod.Result;
		}

		public void Put(Model.InitialData data)
		{
			Program.countingMethod.Data = data;
		}

	}
}