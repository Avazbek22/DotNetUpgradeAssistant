
namespace DotNetUpgradeAssistant
{
	internal static class FrameworkCatalog
	{
		private static readonly (string Friendly, string Tfm)[] Map =
		[
			(".NET 9.0",       "net9.0"),
			(".NET 8.0",       "net8.0"),
			(".NET 7.0",       "net7.0"),
			(".NET 6.0",       "net6.0"),
			(".NET Framework 4.8",   "net48"),
			(".NET Framework 4.7.2", "net472")
		];

		public static IEnumerable<string> FriendlyNames => Map.Select(p => p.Friendly);

		public static string ToTfm(string friendly) =>
			Array.Find(Map, m => m.Friendly.Equals(friendly, StringComparison.OrdinalIgnoreCase)).Tfm;

		public static string ToFriendly(string tfm) =>
			Array.Find(Map, m => m.Tfm.Equals(tfm, StringComparison.OrdinalIgnoreCase)).Friendly;
	}
}
