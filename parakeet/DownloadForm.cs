using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
namespace parakeet
{
	public class DownloadForm : Form
	{
		private IContainer components;
		private GroupBox groupBox1;
		private CheckedListBox soundListBox;
		private ComboBox feedSelectBox;
		private GroupBox groupBox2;
		private Label showAllLabel;
		private ProgressBar currentProgress;
		private Label showFileLabel;
		private Button downloadButton;
		private Button abortButton;
		private ProgressBar allProgress;
		private GroupBox groupBox3;
		private Button disselectAllButton;
		private Button selectAllButton;
		private Button directoryDialogButton;
		private TextBox saveDirectory;
		private AxWindowsMediaPlayer wMediaPlayer;
		private List<RSSElement> rssElements = new List<RSSElement>();
		private int currentIndex;
		private WebClient wc = new WebClient();
		private string currentFileLabel = "";
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DownloadForm));
			this.groupBox1 = new GroupBox();
			this.wMediaPlayer = new AxWindowsMediaPlayer();
			this.disselectAllButton = new Button();
			this.selectAllButton = new Button();
			this.downloadButton = new Button();
			this.abortButton = new Button();
			this.soundListBox = new CheckedListBox();
			this.feedSelectBox = new ComboBox();
			this.groupBox2 = new GroupBox();
			this.allProgress = new ProgressBar();
			this.showAllLabel = new Label();
			this.currentProgress = new ProgressBar();
			this.showFileLabel = new Label();
			this.groupBox3 = new GroupBox();
			this.directoryDialogButton = new Button();
			this.saveDirectory = new TextBox();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.wMediaPlayer).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.wMediaPlayer);
			this.groupBox1.Controls.Add(this.disselectAllButton);
			this.groupBox1.Controls.Add(this.selectAllButton);
			this.groupBox1.Controls.Add(this.downloadButton);
			this.groupBox1.Controls.Add(this.abortButton);
			this.groupBox1.Controls.Add(this.soundListBox);
			this.groupBox1.Controls.Add(this.feedSelectBox);
			this.groupBox1.Location = new Point(3, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(550, 349);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ダウンロードファイルの選択";
			this.wMediaPlayer.Enabled = true;
			this.wMediaPlayer.Location = new Point(9, 292);
			this.wMediaPlayer.Name = "wMediaPlayer";
			this.wMediaPlayer.OcxState = (AxHost.State)componentResourceManager.GetObject("wMediaPlayer.OcxState");
			this.wMediaPlayer.Size = new Size(349, 46);
			this.wMediaPlayer.TabIndex = 8;
			this.disselectAllButton.Location = new Point(445, 304);
			this.disselectAllButton.Name = "disselectAllButton";
			this.disselectAllButton.Size = new Size(99, 23);
			this.disselectAllButton.TabIndex = 3;
			this.disselectAllButton.Text = "すべて選択解除";
			this.disselectAllButton.UseVisualStyleBackColor = true;
			this.disselectAllButton.Click += new EventHandler(this.Click_disselectAllButton);
			this.selectAllButton.Location = new Point(364, 304);
			this.selectAllButton.Name = "selectAllButton";
			this.selectAllButton.Size = new Size(75, 23);
			this.selectAllButton.TabIndex = 2;
			this.selectAllButton.Text = "すべて選択";
			this.selectAllButton.UseVisualStyleBackColor = true;
			this.selectAllButton.Click += new EventHandler(this.Click_selectAllButton);
			this.downloadButton.Location = new Point(358, 16);
			this.downloadButton.Name = "downloadButton";
			this.downloadButton.Size = new Size(90, 23);
			this.downloadButton.TabIndex = 6;
			this.downloadButton.Text = "ダウンロード";
			this.downloadButton.UseVisualStyleBackColor = true;
			this.downloadButton.Click += new EventHandler(this.Click_downloadButton);
			this.abortButton.Enabled = false;
			this.abortButton.Location = new Point(454, 15);
			this.abortButton.Name = "abortButton";
			this.abortButton.Size = new Size(90, 23);
			this.abortButton.TabIndex = 7;
			this.abortButton.Text = "中止";
			this.abortButton.UseVisualStyleBackColor = true;
			this.abortButton.Click += new EventHandler(this.Click_abortButton);
			this.soundListBox.CheckOnClick = true;
			this.soundListBox.FormattingEnabled = true;
			this.soundListBox.Location = new Point(9, 44);
			this.soundListBox.Name = "soundListBox";
			this.soundListBox.Size = new Size(535, 242);
			this.soundListBox.TabIndex = 1;
			this.soundListBox.SelectedValueChanged += new EventHandler(this.SChange_soundListBox);
			this.feedSelectBox.FormattingEnabled = true;
			this.feedSelectBox.Location = new Point(9, 18);
			this.feedSelectBox.Name = "feedSelectBox";
			this.feedSelectBox.Size = new Size(343, 20);
			this.feedSelectBox.TabIndex = 0;
			this.feedSelectBox.SelectedIndexChanged += new EventHandler(this.IChange_feedSelectBox);
			this.groupBox2.Controls.Add(this.allProgress);
			this.groupBox2.Controls.Add(this.showAllLabel);
			this.groupBox2.Controls.Add(this.currentProgress);
			this.groupBox2.Controls.Add(this.showFileLabel);
			this.groupBox2.Location = new Point(3, 413);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(550, 113);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "進捗状況";
			this.allProgress.Location = new Point(8, 80);
			this.allProgress.Name = "allProgress";
			this.allProgress.Size = new Size(536, 23);
			this.allProgress.Style = ProgressBarStyle.Continuous;
			this.allProgress.TabIndex = 3;
			this.showAllLabel.AutoSize = true;
			this.showAllLabel.Location = new Point(6, 65);
			this.showAllLabel.Name = "showAllLabel";
			this.showAllLabel.Size = new Size(29, 12);
			this.showAllLabel.TabIndex = 2;
			this.showAllLabel.Text = "全体";
			this.currentProgress.Location = new Point(8, 30);
			this.currentProgress.Name = "currentProgress";
			this.currentProgress.Size = new Size(536, 23);
			this.currentProgress.Style = ProgressBarStyle.Continuous;
			this.currentProgress.TabIndex = 1;
			this.showFileLabel.AutoSize = true;
			this.showFileLabel.Location = new Point(6, 15);
			this.showFileLabel.Name = "showFileLabel";
			this.showFileLabel.Size = new Size(39, 12);
			this.showFileLabel.TabIndex = 0;
			this.showFileLabel.Text = "ファイル";
			this.groupBox3.Controls.Add(this.directoryDialogButton);
			this.groupBox3.Controls.Add(this.saveDirectory);
			this.groupBox3.Location = new Point(3, 367);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(550, 40);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "保存先ディレクトリ";
			this.directoryDialogButton.Location = new Point(482, 15);
			this.directoryDialogButton.Name = "directoryDialogButton";
			this.directoryDialogButton.Size = new Size(65, 19);
			this.directoryDialogButton.TabIndex = 5;
			this.directoryDialogButton.Text = "参照...";
			this.directoryDialogButton.UseVisualStyleBackColor = true;
			this.directoryDialogButton.Click += new EventHandler(this.Click_directoryDialogButton);
			this.saveDirectory.Location = new Point(7, 15);
			this.saveDirectory.Name = "saveDirectory";
			this.saveDirectory.ReadOnly = true;
			this.saveDirectory.Size = new Size(469, 19);
			this.saveDirectory.TabIndex = 4;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(556, 531);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DownloadForm";
			base.ShowInTaskbar = false;
			this.Text = "ファイルのダウンロード";
			base.FormClosing += new FormClosingEventHandler(this.FClosing_DownloadForm);
			this.groupBox1.ResumeLayout(false);
			((ISupportInitialize)this.wMediaPlayer).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
		}
		public DownloadForm(List<RSSElement> temp)
		{
			this.InitializeComponent();
			this.rssElements = temp;
			this.InitializeWindow();
		}
		private void InitializeWindow()
		{
			this.saveDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			foreach (RSSElement current in this.rssElements)
			{
				if (current.elementEnabled)
				{
					this.feedSelectBox.Items.Add(current.title);
				}
			}
			this.wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.wc_DownloadProgressChanged);
			this.wc.DownloadFileCompleted += new AsyncCompletedEventHandler(this.wc_DownloadFileCompleted);
			this.wc.Headers.Add("User-Agent", "PPD/1.0; (Parakeet Podcast Downloader " + Application.ProductVersion + ")");
		}
		private void RefreshSoundBox()
		{
			this.soundListBox.Items.Clear();
			int index = 0;
			for (int i = 0; i < this.rssElements.Count; i++)
			{
				if (this.rssElements[i].title == this.feedSelectBox.Text)
				{
					index = i;
				}
			}
			foreach (string current in this.rssElements[index].soundFiles)
			{
				this.soundListBox.Items.Add(current);
			}
		}
		private void SelectAll()
		{
			for (int i = 0; i < this.soundListBox.Items.Count; i++)
			{
				if (!this.soundListBox.GetItemChecked(i))
				{
					this.soundListBox.SetItemChecked(i, true);
				}
			}
		}
		private void DisSelectAll()
		{
			for (int i = 0; i < this.soundListBox.Items.Count; i++)
			{
				if (this.soundListBox.GetItemChecked(i))
				{
					this.soundListBox.SetItemChecked(i, false);
				}
			}
		}
		private void SetDirectory()
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = "保存先のディレクトリを選択してください";
			folderBrowserDialog.ShowDialog();
			this.saveDirectory.Text = folderBrowserDialog.SelectedPath;
		}
		private void DownloadFiles()
		{
			if (this.soundListBox.CheckedItems.Count == 0)
			{
				MessageBox.Show("ファイルを選択してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			this.soundListBox.Enabled = false;
			this.feedSelectBox.Enabled = false;
			this.downloadButton.Enabled = false;
			this.selectAllButton.Enabled = false;
			this.disselectAllButton.Enabled = false;
			this.directoryDialogButton.Enabled = false;
			this.abortButton.Enabled = true;
			this.allProgress.Maximum = this.soundListBox.CheckedItems.Count;
			this.DownloadNext();
		}
		private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.currentProgress.Value = e.ProgressPercentage;
			double num = (double)e.BytesReceived / 1024.0;
			double num2 = (double)e.TotalBytesToReceive / 1024.0;
			this.showFileLabel.Text = string.Concat(new string[]
			{
				this.currentFileLabel,
				"(",
				num2.ToString("#.0"),
				"Kbyte 中 ",
				num.ToString("#.0"),
				"Kbyte受信)"
			});
		}
		private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			this.currentIndex++;
			this.allProgress.Value++;
			if (e.Cancelled)
			{
				MessageBox.Show("キャンセルされました", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.InitializeDownloadForm();
				return;
			}
			if (this.soundListBox.CheckedItems.Count == this.currentIndex)
			{
				this.showAllLabel.Text = string.Concat(new object[]
				{
					"全体 (",
					this.currentIndex,
					"/",
					this.allProgress.Maximum,
					")"
				});
				MessageBox.Show("ダウンロードが完了しました", "完了", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				this.InitializeDownloadForm();
				return;
			}
			this.currentProgress.Value = 0;
			this.DownloadNext();
		}
		private void InitializeDownloadForm()
		{
			this.currentIndex = 0;
			this.currentProgress.Value = 0;
			this.allProgress.Value = 0;
			this.soundListBox.Enabled = true;
			this.feedSelectBox.Enabled = true;
			this.downloadButton.Enabled = true;
			this.selectAllButton.Enabled = true;
			this.disselectAllButton.Enabled = true;
			this.directoryDialogButton.Enabled = true;
			this.abortButton.Enabled = false;
			this.showFileLabel.Text = "ファイル";
			this.showAllLabel.Text = "全体";
			this.currentFileLabel = "";
		}
		private void DownloadNext()
		{
			string text = this.soundListBox.CheckedItems[this.currentIndex].ToString();
			this.wc.DownloadFileAsync(new Uri(text), this.saveDirectory.Text + "\\" + this.GetFileNameInURL(text));
			this.currentFileLabel = this.GetFileNameInURL(text);
			this.showFileLabel.Text = this.GetFileNameInURL(text);
			this.showAllLabel.Text = string.Concat(new object[]
			{
				"全体 (",
				this.currentIndex,
				"/",
				this.allProgress.Maximum,
				")"
			});
		}
		private string GetFileNameInURL(string str)
		{
			int num = str.LastIndexOf("/");
			return str.Substring(num + 1, str.Length - num - 1);
		}
		private void SetPlayerURL()
		{
			if (this.wMediaPlayer.status.IndexOf("再生中") == -1)
			{
				this.wMediaPlayer.URL = this.soundListBox.Text;
				this.wMediaPlayer.Ctlcontrols.stop();
			}
		}
		private void DisposePlayer()
		{
			this.wMediaPlayer.Dispose();
		}
		private void IChange_feedSelectBox(object sender, EventArgs e)
		{
			this.RefreshSoundBox();
		}
		private void Click_selectAllButton(object sender, EventArgs e)
		{
			this.SelectAll();
		}
		private void Click_disselectAllButton(object sender, EventArgs e)
		{
			this.DisSelectAll();
		}
		private void Click_directoryDialogButton(object sender, EventArgs e)
		{
			this.SetDirectory();
		}
		private void Click_downloadButton(object sender, EventArgs e)
		{
			this.DownloadFiles();
		}
		private void Click_abortButton(object sender, EventArgs e)
		{
			this.wc.CancelAsync();
		}
		private void SChange_soundListBox(object sender, EventArgs e)
		{
			this.SetPlayerURL();
		}
		private void FClosing_DownloadForm(object sender, FormClosingEventArgs e)
		{
			this.DisposePlayer();
		}
	}
}
