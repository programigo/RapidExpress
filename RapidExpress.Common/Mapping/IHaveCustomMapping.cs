using AutoMapper;

namespace RapidExpress.Common.Mapping
{
	public interface IHaveCustomMapping
	{
		void ConfigureMapping(Profile mapper);
	}
}
