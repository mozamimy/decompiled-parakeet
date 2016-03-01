using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace parakeet
{
	public class AddRssForm : Form
	{
		private IContainer components;
		private TextBox feedURLbox;
		private GroupBox groupBox1;
		private Button OKButton;
		private Button cancelButton;
		private Label label1;
		private TreeView refRssTree;
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.feedURLbox = new TextBox();
			this.groupBox1 = new GroupBox();
			this.OKButton = new Button();
			this.cancelButton = new Button();
			this.label1 = new Label();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.feedURLbox.Location = new Point(6, 18);
			this.feedURLbox.Name = "feedURLbox";
			this.feedURLbox.Size = new Size(259, 19);
			this.feedURLbox.TabIndex = 0;
			this.groupBox1.Controls.Add(this.feedURLbox);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(271, 46);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "フィードのURL";
			this.OKButton.DialogResult = DialogResult.OK;
			this.OKButton.Location = new Point(127, 64);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new Size(75, 23);
			this.OKButton.TabIndex = 1;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new EventHandler(this.Click_OKButton);
			this.cancelButton.DialogResult = DialogResult.Cancel;
			this.cancelButton.Location = new Point(208, 64);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "キャンセル";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new EventHandler(this.Click_cancelButton);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(6, 69);
			this.label1.Name = "label1";
			this.label1.Size = new Size(115, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "D＆Dすることもできます";
			base.AcceptButton = this.OKButton;
			this.AllowDrop = true;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new Size(295, 90);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.OKButton);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "AddRssForm";
			base.ShowInTaskbar = false;
			this.Text = "RSSフィードの追加";
			base.DragDrop += new DragEventHandler(this.DDrop_AddRssForm);
			base.DragEnter += new DragEventHandler(this.DEnter_AddRssForm);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public AddRssForm(TreeView tempTree)
		{
			this.refRssTree = tempTree;
			this.InitializeComponent();
		}
		private void CloseThisWindow()
		{
			base.Close();
		}
		private void AddFeedURL()
		{
			this.refRssTree.Nodes[0].Nodes.Add(this.feedURLbox.Text);
			ParakeetEnvironment.rssUrlList.Add(this.feedURLbox.Text);
			base.Close();
		}
		private void DEnter_AddRssForm(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.UnicodeText))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}
		private void DDrop_AddRssForm(object sender, DragEventArgs e)
		{
			this.feedURLbox.Text = (string)e.Data.GetData(DataFormats.UnicodeText, true);
		}
		private void Click_cancelButton(object sender, EventArgs e)
		{
			this.CloseThisWindow();
		}
		private void Click_OKButton(object sender, EventArgs e)
		{
			this.AddFeedURL();
		}
	}
}
