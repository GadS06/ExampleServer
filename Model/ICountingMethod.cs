using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public interface ICountingMethod
	{
		InitialData Data { get; set; } // входные данные
		List<float> Result { get; set; } // результат работы ЧМ - последовательность точек F_xy(t)

		// входные данные -> результат (или исключение)
		void CountGraph();
	}
}
