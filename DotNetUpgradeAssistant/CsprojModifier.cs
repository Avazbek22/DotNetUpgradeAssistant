using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUpgradeAssistant
{
	internal static class CsprojModifier
	{
		private static readonly Regex _rxSingle =
			new(@"<(TargetFramework|TargetFrameworkVersion)>([^<]+)</\1>",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex _rxMulti =
			new(@"<TargetFrameworks>([^<]+)</TargetFrameworks>",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>Физически копирует файл в каталог Backups и возвращает путь к копии.</summary>
		public static string MakeBackup(string csprojPath, string backupRoot)
		{
			Directory.CreateDirectory(backupRoot);

			string fileName = Path.GetFileName(csprojPath);
			string dst = Path.Combine(backupRoot, fileName);

			File.Copy(csprojPath, dst, overwrite: true);
			return dst;
		}

		/// <summary>Заменяет TFM. Предполагается, что резервная копия уже сделана.</summary>
		public static void ReplaceTfm(string path, string newTfm)
		{
			string txt = File.ReadAllText(path, Encoding.UTF8);

			txt = _rxSingle.Replace(txt, m =>
			{
				var tag = m.Groups[1].Value;
				return $"<{tag}>{newTfm}</{tag}>";
			});

			txt = _rxMulti.Replace(txt, $"<TargetFrameworks>{newTfm}</TargetFrameworks>");

			txt = Regex.Replace(txt, @"\s*<RuntimeIdentifier>.*?</RuntimeIdentifier>\s*",
								string.Empty, RegexOptions.Singleline);

			File.WriteAllText(path, txt,
				new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}
	}

}
