using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace parakeet
{
	public class MainForm : Form
	{
		private int rssFeeds;
		private bool feedLoadFlag;
		private List<RSSElement> rssElements = new List<RSSElement>();
		private string styleSheet = "";
		private IContainer components;
		private StatusStrip statusBar;
		private MenuStrip menuStrip1;
		private Panel panel1;
		private SplitContainer splitContainer1;
		private TreeView rssTree;
		private ToolStripMenuItem fileMenu;
		private ToolStripMenuItem quitMenu;
		private ToolStripMenuItem hhelpMenu;
		private ToolStripMenuItem helpFileOpenMenu;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem versionInfoMenu;
		private ToolStripMenuItem refreshRssFeedMenu;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem saveFileMenu;
		private ToolStripStatusLabel statusLabel;
		private WebBrowser feedBrowser;
		private ToolStripMenuItem editMenu;
		private ToolStripMenuItem addRSSFeedMenu;
		private ToolStripMenuItem removeRSSFeedMenu;
		public MainForm()
		{
			this.InitializeComponent();
			this.InitializeWindow();
		}
		private void InitializeWindow()
		{
			this.Text = "Parakeet Podcast Downloader " + Application.ProductVersion;
			this.feedBrowser.DocumentText = "<html><body>ここに詳細が表示されます</body></html>";
			this.rssTree.Nodes.Add("RSSフィードツリー");
			foreach (string current in ParakeetEnvironment.rssUrlList)
			{
				this.rssTree.Nodes[0].Nodes.Add(current);
				this.rssFeeds++;
			}
			using (StreamReader streamReader = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\default.css", Encoding.GetEncoding("UTF-8")))
			{
				this.styleSheet = streamReader.ReadToEnd();
			}
		}
		private void RefreshRSSFeedTree()
		{
			this.statusLabel.Text = "RSSフィードの読み込み中... フィード数が多い場合は時間がかかります...";
			this.statusBar.Refresh();
			this.rssElements.Clear();
			int i = 0;
			while (i < this.rssFeeds)
			{
				Stream stream = null;
				try
				{
					stream = new WebClient
					{
						Headers = 
						{

							{
								"User-Agent",
								"PPD/1.0; (Parakeet Podcast Downloader " + Application.ProductVersion + ")"
							}
						}
					}.OpenRead(ParakeetEnvironment.rssUrlList[i]);
				}
				catch
				{
					MessageBox.Show(ParakeetEnvironment.rssUrlList[i] + "\nを開くことができませんでした\n処理をスキップします", "フィード読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					this.rssElements.Add(new RSSElement(false));
					goto IL_51D;
				}
				goto IL_A7;
				IL_51D:
				i++;
				continue;
				IL_A7:
				StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
				string s = streamReader.ReadToEnd();
				StringReader input = new StringReader(s);
				XmlTextReader xmlTextReader = new XmlTextReader(input);
				this.rssElements.Add(new RSSElement());
				try
				{
					while (xmlTextReader.Read())
					{
						if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "title" && this.rssElements[i].title == "")
						{
							while (xmlTextReader.Read())
							{
								if (xmlTextReader.NodeType == XmlNodeType.Text)
								{
									this.rssElements[i].title = xmlTextReader.Value;
									break;
								}
							}
						}
						if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "link" && this.rssElements[i].link == "")
						{
							while (xmlTextReader.Read())
							{
								if (xmlTextReader.NodeType == XmlNodeType.Text)
								{
									this.rssElements[i].link = xmlTextReader.Value;
									break;
								}
							}
						}
						if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "description" && this.rssElements[i].description == "")
						{
							while (xmlTextReader.Read())
							{
								if (xmlTextReader.NodeType == XmlNodeType.Text)
								{
									this.rssElements[i].description = xmlTextReader.Value;
									break;
								}
							}
						}
						if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "item")
						{
							this.rssElements[i].items.Add(new RSSItem());
							while (xmlTextReader.Read())
							{
								if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "title")
								{
									xmlTextReader.Read();
									if (xmlTextReader.Value == "")
									{
										this.rssElements[i].items[this.rssElements[i].items.Count - 1].title = "無題";
									}
									else
									{
										this.rssElements[i].items[this.rssElements[i].items.Count - 1].title = xmlTextReader.Value;
									}
								}
								if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "link")
								{
									xmlTextReader.Read();
									this.rssElements[i].items[this.rssElements[i].items.Count - 1].url = xmlTextReader.Value;
								}
								if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "description")
								{
									xmlTextReader.Read();
									this.rssElements[i].items[this.rssElements[i].items.Count - 1].description = xmlTextReader.Value;
								}
								if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "pubDate")
								{
									xmlTextReader.Read();
									this.rssElements[i].items[this.rssElements[i].items.Count - 1].pubDate = xmlTextReader.Value;
								}
								if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "enclosure")
								{
									xmlTextReader.MoveToFirstAttribute();
									do
									{
										if (xmlTextReader.Name == "url")
										{
											this.rssElements[i].items[this.rssElements[i].items.Count - 1].soundURL = xmlTextReader.Value;
											this.rssElements[i].soundFiles.Add(xmlTextReader.Value);
										}
									}
									while (xmlTextReader.MoveToNextAttribute());
								}
								if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name == "item")
								{
									break;
								}
							}
						}
					}
				}
				catch
				{
					MessageBox.Show("RSSフィードの構文エラーが発生しました\n以降の操作により，予期しないエラーが発生する可能性があります", "XMLパースエラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				goto IL_51D;
			}
			for (int j = 0; j < this.rssFeeds; j++)
			{
				if (this.rssElements[j].elementEnabled)
				{
					this.rssTree.Nodes[0].Nodes[j].Text = this.rssElements[j].title;
				}
			}
			this.rssTree.ExpandAll();
			this.feedLoadFlag = true;
			this.saveFileMenu.Enabled = true;
			this.statusLabel.Text = "RSSフィードの読み込みが完了しました";
		}
		private void ShowRSSView()
		{
			if (this.rssFeeds == 0)
			{
				return;
			}
			int num = -1;
			if (this.feedLoadFlag)
			{
				this.statusLabel.Text = "RSSビューを更新しています";
				for (int i = 0; i < this.rssElements.Count; i++)
				{
					if (this.rssElements[i].title == this.rssTree.SelectedNode.Text)
					{
						num = i;
					}
				}
				if (num == -1)
				{
					MessageBox.Show("指定されたRSSフィードは無効です", "RSSビュー更新エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					this.statusLabel.Text = "エラーにより，RSSビュー更新ができませんでした";
					return;
				}
				string text = "<html>\n<head>\n";
				text = text + "<title>\nPPD " + this.rssElements[num].title + " </title>\n";
				text += "<style type=\"text/css\">\n";
				text += this.styleSheet;
				text += "\n</style>\n";
				text += "</head>\n\n";
				text += "<body>\n";
				text += "<div class=\"podcastTitle\">\n";
				text = text + this.rssElements[num].title + "\n";
				text += "</div>\n";
				text += "<div class=\"podcastLink\">\n";
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"ソースURL: <a target=\"_blank\" href=\"",
					this.rssElements[num].link,
					"\">",
					this.rssElements[num].link,
					"</a>\n"
				});
				text += "</div>\n";
				text += "<div class=\"podcastDescription\">\n";
				text = text + this.rssElements[num].description + "\n";
				text += "</div>\n\n";
				foreach (RSSItem current in this.rssElements[num].items)
				{
					text += "<div class=\"item\">\n";
					text += "<div class=\"itemTitle\">\n";
					text = text + current.title + "\n";
					text += "</div>\n";
					text += "<div class=\"itemLink\">\n";
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						"<a target=\"_blank\" href=\"",
						current.url,
						"\">",
						current.url,
						"</a>\n"
					});
					text += "</div>\n";
					text += "<div class=\"itemDescription\">\n";
					text += current.description;
					text += "</div>\n";
					text += "<div class=\"itemSoundURL\">\n";
					string text4 = text;
					text = string.Concat(new string[]
					{
						text4,
						"サウンド: <a target=\"_blank\" href=\"",
						current.soundURL,
						"\">",
						current.soundURL,
						"</a>\n"
					});
					text += "</div>\n";
					text += "<div class=\"itemPubDate\">\n";
					text = text + current.pubDate + "\n";
					text += "</div>\n";
					text += "</div>\n\n";
				}
				text += "<div class=\"sounds\">\n";
				text += "サウンドファイルの一覧\n";
				text += "<ul>\n";
				foreach (string current2 in this.rssElements[num].soundFiles)
				{
					string text5 = text;
					text = string.Concat(new string[]
					{
						text5,
						"<li><a target=\"_blank\" href=\"",
						current2,
						"\">",
						current2,
						"</a></li>\n"
					});
				}
				text += "</ul>\n";
				text += "</div>\n\n";
				text += "</doby>\n";
				text += "</html>\n";
				this.feedBrowser.DocumentText = text;
				this.statusLabel.Text = "RSSビュー更新完了";
			}
		}
		private void DownloadAndSaveSound()
		{
			DownloadForm downloadForm = new DownloadForm(this.rssElements);
			downloadForm.ShowDialog();
		}
		private void RemoveRSSFeed()
		{
			if (this.rssFeeds != 0)
			{
				DialogResult dialogResult = MessageBox.Show(this.rssTree.Nodes[0].Nodes[this.rssTree.SelectedNode.Index].Text + "\nを削除します\nよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					int index = 0;
					if (this.feedLoadFlag)
					{
						this.rssElements.RemoveAt(this.rssTree.SelectedNode.Index);
						ParakeetEnvironment.rssUrlList.RemoveAt(this.rssTree.SelectedNode.Index);
					}
					else
					{
						for (int i = 0; i < ParakeetEnvironment.rssUrlList.Count; i++)
						{
							if (ParakeetEnvironment.rssUrlList[i] == this.rssTree.SelectedNode.Text)
							{
								index = i;
							}
						}
						ParakeetEnvironment.rssUrlList.RemoveAt(index);
					}
					this.rssTree.Nodes[0].Nodes[this.rssTree.SelectedNode.Index].Remove();
					this.rssFeeds--;
				}
			}
		}
		private void OpenHelpFile()
		{
			try
			{
				new Process
				{
					StartInfo = 
					{
						FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\Readme.chm"
					}
				}.Start();
			}
			catch
			{
				MessageBox.Show("ヘルプファイルが見つかりませんでした", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		private void AddRSSFeed()
		{
			AddRssForm addRssForm = new AddRssForm(this.rssTree);
			if (addRssForm.ShowDialog() == DialogResult.OK)
			{
				this.rssFeeds++;
			}
		}
		private void SaveRSSList()
		{
			using (StreamWriter streamWriter = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\RSSList.ini", false, Encoding.GetEncoding("UTF-8")))
			{
				foreach (string current in ParakeetEnvironment.rssUrlList)
				{
					streamWriter.WriteLine(current);
				}
			}
		}
		private void Click_refreshRssFeedMenu(object sender, EventArgs e)
		{
			this.RefreshRSSFeedTree();
		}
		private void DClick_rssTree(object sender, EventArgs e)
		{
			this.ShowRSSView();
		}
		private void Click_quitMenu(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
		private void Click_saveFileMenu(object sender, EventArgs e)
		{
			this.DownloadAndSaveSound();
		}
		private void Click_removeRssFeedMenu(object sender, EventArgs e)
		{
			this.RemoveRSSFeed();
		}
		private void Click_versionInfoMenu(object sender, EventArgs e)
		{
			VersionDialog versionDialog = new VersionDialog();
			versionDialog.ShowDialog();
		}
		private void Click_helpFileOpenMenu(object sender, EventArgs e)
		{
			this.OpenHelpFile();
		}
		private void Click_addRSSFeedMenu(object sender, EventArgs e)
		{
			this.AddRSSFeed();
		}
		private void Closing_MainForm(object sender, FormClosingEventArgs e)
		{
			this.SaveRSSList();
		}
		private void Click_removeRSSFeedMenu(object sender, EventArgs e)
		{
			this.RemoveRSSFeed();
		}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
			this.statusBar = new StatusStrip();
			this.statusLabel = new ToolStripStatusLabel();
			this.menuStrip1 = new MenuStrip();
			this.fileMenu = new ToolStripMenuItem();
			this.refreshRssFeedMenu = new ToolStripMenuItem();
			this.saveFileMenu = new ToolStripMenuItem();
			this.toolStripSeparator3 = new ToolStripSeparator();
			this.quitMenu = new ToolStripMenuItem();
			this.editMenu = new ToolStripMenuItem();
			this.addRSSFeedMenu = new ToolStripMenuItem();
			this.removeRSSFeedMenu = new ToolStripMenuItem();
			this.hhelpMenu = new ToolStripMenuItem();
			this.helpFileOpenMenu = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.versionInfoMenu = new ToolStripMenuItem();
			this.panel1 = new Panel();
			this.splitContainer1 = new SplitContainer();
			this.rssTree = new TreeView();
			this.feedBrowser = new WebBrowser();
			this.statusBar.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.statusBar.Items.AddRange(new ToolStripItem[]
			{
				this.statusLabel
			});
			this.statusBar.Location = new Point(0, 551);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new Size(792, 22);
			this.statusBar.TabIndex = 1;
			this.statusBar.Text = "statusStrip1";
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new Size(37, 17);
			this.statusLabel.Text = "Ready";
			this.menuStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.fileMenu,
				this.editMenu,
				this.hhelpMenu
			});
			this.menuStrip1.Location = new Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new Size(792, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			this.fileMenu.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.refreshRssFeedMenu,
				this.saveFileMenu,
				this.toolStripSeparator3,
				this.quitMenu
			});
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.Size = new Size(85, 20);
			this.fileMenu.Text = "ファイル(&F)";
			this.refreshRssFeedMenu.Name = "refreshRssFeedMenu";
			this.refreshRssFeedMenu.ShortcutKeys = Keys.F5;
			this.refreshRssFeedMenu.Size = new Size(285, 22);
			this.refreshRssFeedMenu.Text = "RSSフィードの更新(&R)";
			this.refreshRssFeedMenu.Click += new EventHandler(this.Click_refreshRssFeedMenu);
			this.saveFileMenu.Enabled = false;
			this.saveFileMenu.Name = "saveFileMenu";
			this.saveFileMenu.ShortcutKeys = (Keys)131155;
			this.saveFileMenu.Size = new Size(285, 22);
			this.saveFileMenu.Text = "音声ファイルの一括保存(&S)...";
			this.saveFileMenu.Click += new EventHandler(this.Click_saveFileMenu);
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new Size(282, 6);
			this.quitMenu.Name = "quitMenu";
			this.quitMenu.ShortcutKeys = (Keys)262259;
			this.quitMenu.Size = new Size(285, 22);
			this.quitMenu.Text = "終了(&X)";
			this.quitMenu.Click += new EventHandler(this.Click_quitMenu);
			this.editMenu.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.addRSSFeedMenu,
				this.removeRSSFeedMenu
			});
			this.editMenu.Name = "editMenu";
			this.editMenu.Size = new Size(61, 20);
			this.editMenu.Text = "編集(&E)";
			this.addRSSFeedMenu.Name = "addRSSFeedMenu";
			this.addRSSFeedMenu.ShortcutKeys = Keys.Insert;
			this.addRSSFeedMenu.Size = new Size(237, 22);
			this.addRSSFeedMenu.Text = "RSSフィードの追加(&A)...";
			this.addRSSFeedMenu.Click += new EventHandler(this.Click_addRSSFeedMenu);
			this.removeRSSFeedMenu.Name = "removeRSSFeedMenu";
			this.removeRSSFeedMenu.ShortcutKeys = Keys.Delete;
			this.removeRSSFeedMenu.Size = new Size(237, 22);
			this.removeRSSFeedMenu.Text = "RSSフィードの削除(&D)...";
			this.removeRSSFeedMenu.Click += new EventHandler(this.Click_removeRSSFeedMenu);
			this.hhelpMenu.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.helpFileOpenMenu,
				this.toolStripSeparator2,
				this.versionInfoMenu
			});
			this.hhelpMenu.Name = "hhelpMenu";
			this.hhelpMenu.Size = new Size(73, 20);
			this.hhelpMenu.Text = "ヘルプ(&H)";
			this.helpFileOpenMenu.Name = "helpFileOpenMenu";
			this.helpFileOpenMenu.ShortcutKeys = Keys.F1;
			this.helpFileOpenMenu.Size = new Size(249, 22);
			this.helpFileOpenMenu.Text = "ヘルプファイルを開く(&O)...";
			this.helpFileOpenMenu.Click += new EventHandler(this.Click_helpFileOpenMenu);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new Size(246, 6);
			this.versionInfoMenu.Name = "versionInfoMenu";
			this.versionInfoMenu.Size = new Size(249, 22);
			this.versionInfoMenu.Text = "バージョン情報(&V)...";
			this.versionInfoMenu.Click += new EventHandler(this.Click_versionInfoMenu);
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(792, 527);
			this.panel1.TabIndex = 3;
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.rssTree);
			this.splitContainer1.Panel2.Controls.Add(this.feedBrowser);
			this.splitContainer1.Size = new Size(792, 527);
			this.splitContainer1.SplitterDistance = 231;
			this.splitContainer1.TabIndex = 0;
			this.rssTree.Dock = DockStyle.Fill;
			this.rssTree.Location = new Point(0, 0);
			this.rssTree.Name = "rssTree";
			this.rssTree.Size = new Size(231, 527);
			this.rssTree.TabIndex = 0;
			this.rssTree.DoubleClick += new EventHandler(this.DClick_rssTree);
			this.feedBrowser.AllowWebBrowserDrop = false;
			this.feedBrowser.Dock = DockStyle.Fill;
			this.feedBrowser.Location = new Point(0, 0);
			this.feedBrowser.MinimumSize = new Size(20, 20);
			this.feedBrowser.Name = "feedBrowser";
			this.feedBrowser.ScriptErrorsSuppressed = true;
			this.feedBrowser.Size = new Size(557, 527);
			this.feedBrowser.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(792, 573);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.statusBar);
			base.Controls.Add(this.menuStrip1);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "MainForm";
			this.Text = "Parakeet Podcast Downloader";
			base.FormClosing += new FormClosingEventHandler(this.Closing_MainForm);
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
