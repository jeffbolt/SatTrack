using SatTrack.Service.Services.Interfaces;

using System;
using System.Text;

namespace SatTrack.Service.Services
{
	public class StringService : IStringService
	{
		public string RandomString(int length)
		{
			StringBuilder sb = new();

			for (int i = 1; i <= length; i++)
			{
				Random rnd = new();
				char randomChar = (char)rnd.Next('a', 'z');
				sb.Append(randomChar);
			}

			return sb.ToString();
		}
	}
}
