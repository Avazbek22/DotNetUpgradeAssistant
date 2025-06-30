using System.Drawing;
using System.Windows.Forms;

namespace DotNetUpgradeAssistant
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null!;
		private Label lblFolder;
		private TextBox txtFolderPath;
		private Button btnBrowse;
		private TreeView treeProjects;
		private CheckBox chkSelectAll;
		private Label lblTarget;
		private ComboBox cboTarget;
		private Button btnUpgrade;
		private ProgressBar progressBar;
		private RichTextBox rtbLog;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components is not null)
				components.Dispose();
			base.Dispose(disposing);
		}

		#region Windows-Forms Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			lblFolder = new Label();
			txtFolderPath = new TextBox();
			btnBrowse = new Button();
			treeProjects = new TreeView();
			chkSelectAll = new CheckBox();
			lblTarget = new Label();
			cboTarget = new ComboBox();
			btnUpgrade = new Button();
			progressBar = new ProgressBar();
			rtbLog = new RichTextBox();
			SuspendLayout();
			// 
			// lblFolder
			// 
			lblFolder.AutoSize = true;
			lblFolder.Location = new Point(12, 15);
			lblFolder.Name = "lblFolder";
			lblFolder.Size = new Size(123, 20);
			lblFolder.TabIndex = 0;
			lblFolder.Text = "Folder / Solution:";
			// 
			// txtFolderPath
			// 
			txtFolderPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtFolderPath.Location = new Point(141, 12);
			txtFolderPath.Name = "txtFolderPath";
			txtFolderPath.ReadOnly = true;
			txtFolderPath.Size = new Size(686, 27);
			txtFolderPath.TabIndex = 1;
			// 
			// btnBrowse
			// 
			btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnBrowse.Location = new Point(833, 11);
			btnBrowse.Name = "btnBrowse";
			btnBrowse.Size = new Size(92, 28);
			btnBrowse.TabIndex = 2;
			btnBrowse.Text = "Browse…";
			btnBrowse.Click += btnBrowse_Click;
			// 
			// treeProjects
			// 
			treeProjects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			treeProjects.CheckBoxes = true;
			treeProjects.Location = new Point(12, 50);
			treeProjects.Name = "treeProjects";
			treeProjects.Size = new Size(388, 453);
			treeProjects.TabIndex = 3;
			// 
			// chkSelectAll
			// 
			chkSelectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			chkSelectAll.AutoSize = true;
			chkSelectAll.Checked = true;
			chkSelectAll.CheckState = CheckState.Checked;
			chkSelectAll.Location = new Point(12, 508);
			chkSelectAll.Name = "chkSelectAll";
			chkSelectAll.Size = new Size(148, 24);
			chkSelectAll.TabIndex = 4;
			chkSelectAll.Text = "Select all projects";
			chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
			// 
			// lblTarget
			// 
			lblTarget.AutoSize = true;
			lblTarget.Location = new Point(418, 46);
			lblTarget.Name = "lblTarget";
			lblTarget.Size = new Size(128, 20);
			lblTarget.TabIndex = 5;
			lblTarget.Text = "Target framework:";
			// 
			// cboTarget
			// 
			cboTarget.DropDownStyle = ComboBoxStyle.DropDownList;
			cboTarget.Location = new Point(418, 69);
			cboTarget.Name = "cboTarget";
			cboTarget.Size = new Size(228, 28);
			cboTarget.TabIndex = 6;
			// 
			// btnUpgrade
			// 
			btnUpgrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnUpgrade.Enabled = false;
			btnUpgrade.Location = new Point(800, 69);
			btnUpgrade.Name = "btnUpgrade";
			btnUpgrade.Size = new Size(125, 28);
			btnUpgrade.TabIndex = 7;
			btnUpgrade.Text = "Upgrade";
			btnUpgrade.Click += btnUpgrade_Click;
			// 
			// progressBar
			// 
			progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			progressBar.Location = new Point(418, 105);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(507, 15);
			progressBar.TabIndex = 8;
			// 
			// rtbLog
			// 
			rtbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			rtbLog.BackColor = SystemColors.Window;
			rtbLog.BorderStyle = BorderStyle.FixedSingle;
			rtbLog.Font = new Font("Consolas", 9F);
			rtbLog.Location = new Point(418, 130);
			rtbLog.Name = "rtbLog";
			rtbLog.ReadOnly = true;
			rtbLog.Size = new Size(507, 423);
			rtbLog.TabIndex = 9;
			rtbLog.Text = "";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(120F, 120F);
			AutoScaleMode = AutoScaleMode.Dpi;
			ClientSize = new Size(937, 543);
			Controls.Add(lblFolder);
			Controls.Add(txtFolderPath);
			Controls.Add(btnBrowse);
			Controls.Add(treeProjects);
			Controls.Add(chkSelectAll);
			Controls.Add(lblTarget);
			Controls.Add(cboTarget);
			Controls.Add(btnUpgrade);
			Controls.Add(progressBar);
			Controls.Add(rtbLog);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = ".NET SDK Upgrade Assistant by Avazbek";
			ResumeLayout(false);
			PerformLayout();
		}
		#endregion
	}
}
