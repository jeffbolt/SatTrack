using Newtonsoft.Json;

using SatTrack.Service.Services;

using System;

namespace SatTrack.Service.Models
{
	public class SatelliteLocation
	{
		[JsonIgnore]
		public string Craft { get; set; }

		[JsonProperty("iss_position")]
		public LatLong Location { get; set; }

		[JsonProperty("timestamp")]
		public long Timestamp { get; set; }

		[JsonIgnore]
		public DateTime DateTime
		{
			get { return UnixTimeHelper.FromUnixTime(Timestamp); }
		}
	}

	//public class SatelliteLocationConverter : JsonConverter
	//{
	//	public override bool CanConvert(Type objectType)
	//	{
	//		return false;
	//	}

	//	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	//	{
	//		if (reader.TokenType == JsonToken.StartArray)
	//		{
	//			return serializer.Deserialize(reader, objectType);
	//		}
	//		return new decimal[] { decimal.Parse(reader.Value.ToString()) };
	//	}

	//	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
}
