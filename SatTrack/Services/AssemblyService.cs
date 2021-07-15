using SatTrack.Service.Services.Interfaces;

using System.Reflection;

namespace SatTrack.Service.Services
{
	public class AssemblyService : IAssemblyService
	{
		private readonly Assembly _assembly;

		public AssemblyService()
		{
			_assembly = Assembly.GetExecutingAssembly();
		}

		public string GetName()
		{
			return _assembly.GetName().Name.ToString();
		}

		public string GetVersion()
		{
			var version = _assembly.GetName().Version;
			return version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
		}
	}
}
