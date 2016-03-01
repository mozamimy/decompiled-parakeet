using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace parakeet
{
	public class VersionDialog : Form
	{
		private IContainer components;
		private Label label1;
		private Label label2;
		private LinkLabel linkLabel1;
		private Label label3;
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
			this.label1 = new Label();
			this.label2 = new Label();
			this.linkLabel1 = new LinkLabel();
			this.label3 = new Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(158, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Parakeet Podcast Downloader";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 30);
			this.label2.Name = "label2";
			this.label2.Size = new Size(50, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "Version: ";
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new Point(12, 51);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new Size(173, 12);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://hmx-17server.jpn.ch:8080/";
			this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.Click_linkLabel1);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(12, 72);
			this.label3.Name = "label3";
			this.label3.Size = new Size(96, 12);
			this.label3.TabIndex = 3;
			this.label3.Text = "by MOSAIC/Flost";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(220, 96);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.linkLabel1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "VersionDialog";
			base.ShowInTaskbar = false;
			this.Text = "バージョン情報";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public VersionDialog()
		{
			this.InitializeComponent();
			Label expr_12 = this.label2;
			expr_12.Text += Application.ProductVersion;
		}
		private void Click_linkLabel1(object sender, LinkLabelLinkClickedEventArgs e)
		{
			new Process
			{
				StartInfo = 
				{
					FileName = "http://hmx-17server.jpn.ch:8080/"
				}
			}.Start();
		}
	}
}
