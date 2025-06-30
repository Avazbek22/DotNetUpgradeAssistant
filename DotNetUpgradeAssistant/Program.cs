using System.Diagnostics;
using System.Security.Principal;
using System.Text;

namespace DotNetUpgradeAssistant
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			// legacy single-byte encodings (866, 1251, …)
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

			// elevate if not running as admin (needed for some global dotnet installs)
			if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
			{
				try
				{
					Process.Start(new ProcessStartInfo(Application.ExecutablePath)
					{
						UseShellExecute = true,
						Verb = "runas"
					});
				}
				catch
				{
					MessageBox.Show("Administrator privileges are required.",
						"Access denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
