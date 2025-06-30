using System.Windows.Forms;

namespace DotNetUpgradeAssistant
{
	internal sealed class ProjectInfo
	{
		public required string Path { get; init; }
		public required string CurrentTfm { get; set; }
		public required string Sdk { get; set; }
		public required TreeNode Node { get; init; }
	}
}
