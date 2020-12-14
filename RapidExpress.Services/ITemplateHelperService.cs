using System.Threading.Tasks;

namespace RapidExpress.Services
{
	public interface ITemplateHelperService
	{
		Task<string> GetTemplateHtmlAsString(string viewName);
	}
}
