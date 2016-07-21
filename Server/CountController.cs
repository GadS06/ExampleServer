using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Server
{
	// api/count --- "count" берётся автоматически из имени этого класса
	[EnableCors(origins: "*", headers: "*", methods: "*")]
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