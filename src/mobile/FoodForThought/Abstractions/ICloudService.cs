using System;
using System.Threading.Tasks;

namespace FoodForThought.Abstractions
{
	public interface ICloudService
	{
		ICloudTable<T> GetTable<T>() where T : TableData;

		Task<UpcLookupResponse> GetInformationForUPC(string upc);
	}
}

