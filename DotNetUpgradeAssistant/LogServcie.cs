
namespace DotNetUpgradeAssistant
{
	internal sealed class LogService(RichTextBox richTextBox)
	{
		public void Header(string text) => Append(text, FontStyle.Bold, true);
		
		public void Info(string text) => Append(text, FontStyle.Regular, false);

		private void Append(string text, FontStyle style, bool extraLine)
		{
			richTextBox.Invoke(new MethodInvoker(() =>
			{
				richTextBox.SelectionStart = richTextBox.TextLength;
				richTextBox.SelectionLength = 0;
				richTextBox.SelectionFont = new Font(richTextBox.Font, style);
				richTextBox.AppendText(text + (extraLine ? "\n\n" : "\n"));
				richTextBox.SelectionFont = richTextBox.Font;
			}));
		}
	}
}
