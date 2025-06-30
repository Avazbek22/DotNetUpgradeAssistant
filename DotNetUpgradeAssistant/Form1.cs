using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetUpgradeAssistant
{
	public sealed partial class Form1 : Form
	{
		private readonly List<ProjectInfo> _projects = new();
		private readonly LogService _log;
		private string? _solutionRoot;
		private bool _hadNugetIssues;

		public Form1()
		{
			InitializeComponent();

			cboTarget.Items.AddRange(FrameworkCatalog.FriendlyNames.ToArray());
			if (cboTarget.Items.Count > 0) cboTarget.SelectedIndex = 1;      // .NET 8.0 по умолчанию

			_log = new LogService(rtbLog);
		}

		/*─────────── выбор папки ───────────*/

		private void btnBrowse_Click(object? sender, EventArgs e)
		{
			using var dlg = new FolderBrowserDialog
			{
				Description = "Select a folder that contains one or more .csproj files"
			};
			if (dlg.ShowDialog(this) != DialogResult.OK) return;

			_solutionRoot = dlg.SelectedPath;
			txtFolderPath.Text = _solutionRoot;
			LoadProjects(_solutionRoot);
		}

		private void LoadProjects(string root)
		{
			treeProjects.Nodes.Clear();
			_projects.Clear();

			var files = Directory.EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories)
								 .Where(p => !p.Contains($"{Path.DirectorySeparatorChar}Backups{Path.DirectorySeparatorChar}",
														  StringComparison.OrdinalIgnoreCase));

			foreach (string csproj in files)
			{
				string xml = File.ReadAllText(csproj, Encoding.UTF8);

				string tfm = Regex.Match(xml,
							  @"<(TargetFramework|TargetFrameworks|TargetFrameworkVersion)>([^<]+)</",
							  RegexOptions.IgnoreCase)
							 .Groups[2].Value.Trim();                         // "net8.0-windows;net48"

				string firstTfm = tfm.Split(';', ',')[0].Trim();        // "net8.0-windows"
				string normalizedTfm = Regex.Match(firstTfm, @"^net\d+(\.\d+)?").Value; // "net8.0"

				string friendly = FrameworkCatalog.ToFriendly(normalizedTfm) ?? normalizedTfm;

				var node = new TreeNode($"{Path.GetFileNameWithoutExtension(csproj)}  [{friendly}]")
				{
					Checked = true
				};
				treeProjects.Nodes.Add(node);

				_projects.Add(new ProjectInfo
				{
					Path = csproj,
					CurrentTfm = normalizedTfm,   // храню нормализованное значение
					Sdk = friendly,
					Node = node
				});
			}

			treeProjects.ExpandAll();
			btnUpgrade.Enabled = _projects.Any();
		}

		private void chkSelectAll_CheckedChanged(object? sender, EventArgs e)
		{
			foreach (TreeNode n in treeProjects.Nodes)
				n.Checked = chkSelectAll.Checked;
		}

		/*─────────── апгрейд ───────────*/

		/*─────────── апгрейд ───────────*/

		private async void btnUpgrade_Click(object? sender, EventArgs e)
		{
			if (_solutionRoot is null) return;

			// если нечего менять – просто выходим без логов
			var selected = _projects.Where(p => p.Node.Checked).ToList();
			if (selected.Count == 0 ||
				selected.All(p => p.Sdk.Equals(cboTarget.SelectedItem)))
			{
				MessageBox.Show("Nothing to upgrade – selected projects already target " +
								cboTarget.SelectedItem + ".", "No changes",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			btnUpgrade.Enabled = false;
			progressBar.Value = 0;
			_hadNugetIssues = false;

			/* ---------- общие данные ---------- */

			string newTfm = FrameworkCatalog.ToTfm((string)cboTarget.SelectedItem!);
			string newFriendly = FrameworkCatalog.ToFriendly(newTfm) ?? newTfm;

			// если все проекты одной версии – покажем её, иначе «various»
			string oldFriendly = selected.Select(p => p.Sdk)
										 .Distinct(StringComparer.OrdinalIgnoreCase)
										 .SingleOrDefault() ?? "various";

			_log.Header($"[{DateTime.Now:HH:mm:ss}]  Upgrade started  {oldFriendly} → {newFriendly}");

			/* ---------- ФАЗА 1: резервные копии ---------- */

			string backupDir = Path.Combine(
				_solutionRoot,
				"Backups",
				$"{oldFriendly.Replace(' ', '_')}_to_{newFriendly.Replace(' ', '_')}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}");

			foreach (var prj in selected)
			{
				string bak = CsprojModifier.MakeBackup(prj.Path, backupDir);
				_log.Info($"backup → {Path.GetFileName(bak)}");
			}

			/* ---------- ФАЗА 2: смена SDK и restore ---------- */

			int total = selected.Count;
			int done = 0;

			foreach (var prj in selected)
			{
				_log.Header($"• {Path.GetFileName(prj.Path)}");

				CsprojModifier.ReplaceTfm(prj.Path, newTfm);
				_log.Info($"  SDK changed  {prj.Sdk} → {newFriendly}");

				(string stdout, string stderr, int code) =
					await RunDotnetAsync($"restore \"{prj.Path}\" --verbosity quiet");

				if (code != 0 || Regex.IsMatch(stdout + stderr, @"\bNU\d{4}\b", RegexOptions.IgnoreCase))
					_hadNugetIssues = true;

				progressBar.Value = Math.Min(100, ++done * 100 / total);
			}

			_log.Header("=== Finished ===");

			MessageBox.Show(
				_hadNugetIssues
					? "Projects upgraded, but some NuGet packages are incompatible.\n" +
					  "Please review them manually."
					: "Projects upgraded successfully.",
				"Upgrade result",
				MessageBoxButtons.OK,
				_hadNugetIssues ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

			// перечитать .csproj-файлы, чтобы TreeView отобразил новые SDK
			LoadProjects(_solutionRoot);
			btnUpgrade.Enabled = true;
		}


		/*─────────── запуск dotnet ───────────*/

		/*─────────── запуск dotnet ───────────*/

		private static async Task<(string stdout, string stderr, int exitCode)> RunDotnetAsync(string args)
		{
			var psi = new ProcessStartInfo("dotnet", args)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				StandardOutputEncoding = Encoding.UTF8,
				StandardErrorEncoding = Encoding.UTF8
			};

			var sbOut = new StringBuilder();
			var sbErr = new StringBuilder();

			using var p = new Process { StartInfo = psi };
			p.OutputDataReceived += (_, e) => { if (e.Data is not null) sbOut.AppendLine(e.Data); };
			p.ErrorDataReceived += (_, e) => { if (e.Data is not null) sbErr.AppendLine(e.Data); };

			p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();
			await p.WaitForExitAsync().ConfigureAwait(false);

			return (sbOut.ToString(), sbErr.ToString(), p.ExitCode);
		}

	}
}
