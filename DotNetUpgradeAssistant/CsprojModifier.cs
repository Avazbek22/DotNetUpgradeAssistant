using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUpgradeAssistant
{
	internal static class CsprojModifier
	{
		private static readonly Regex RxSingle =
			new(@"<(TargetFramework|TargetFrameworkVersion)>([^<]+)</\1>",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex RxMulti =
			new(@"<TargetFrameworks>([^<]+)</TargetFrameworks>",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static string MakeBackup(string csprojPath, string backupRoot)
		{
			Directory.CreateDirectory(backupRoot);

			string fileName = Path.GetFileName(csprojPath);
			string dst = Path.Combine(backupRoot, fileName);

			File.Copy(csprojPath, dst, overwrite: true);
			return dst;
		}

		public static void ReplaceTfm(string path, string newTfm)
		{
			string txt = File.ReadAllText(path, Encoding.UTF8);

			txt = RxSingle.Replace(txt, m =>
			{
				var tag = m.Groups[1].Value;
				return $"<{tag}>{newTfm}</{tag}>";
			});

			txt = RxMulti.Replace(txt, $"<TargetFrameworks>{newTfm}</TargetFrameworks>");

			txt = Regex.Replace(txt, @"\s*<RuntimeIdentifier>.*?</RuntimeIdentifier>\s*", string.Empty, RegexOptions.Singleline);

			File.WriteAllText(path, txt, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}
	}

}
