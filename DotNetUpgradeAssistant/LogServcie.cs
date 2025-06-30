using System.Drawing;
using System.Windows.Forms;

namespace DotNetUpgradeAssistant
{
	internal sealed class LogService
	{
		private readonly RichTextBox _rtb;
		public LogService(RichTextBox richTextBox) => _rtb = richTextBox;

		public void Header(string text) => Append(text, FontStyle.Bold, true);
		public void Info(string text) => Append(text, FontStyle.Regular, false);

		private void Append(string text, FontStyle style, bool extraLine)
		{
			_rtb.Invoke(new MethodInvoker(() =>
			{
				_rtb.SelectionStart = _rtb.TextLength;
				_rtb.SelectionLength = 0;
				_rtb.SelectionFont = new Font(_rtb.Font, style);
				_rtb.AppendText(text + (extraLine ? "\n\n" : "\n"));
				_rtb.SelectionFont = _rtb.Font;
			}));
		}
	}
}
