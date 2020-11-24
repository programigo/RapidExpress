namespace RapidExpress.Data.Models
{
	public interface IMeasureable
	{
		int? LengthFirstPart { get; set; }

		int? LengthSecondPart { get; set; }

		int? WidthFirstPart { get; set; }

		int? WidthSecondPart { get; set; }

		int? HeightFirstPart { get; set; }

		int? HeightSecondPart { get; set; }

		int? Weight { get; set; }
	}
}
