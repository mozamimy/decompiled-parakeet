using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace parakeet
{
	internal class ParakeetEnvironment
	{
		public static List<string> rssUrlList;
		public static string fileSavePath;
		static ParakeetEnvironment()
		{
			ParakeetEnvironment.rssUrlList = new List<string>();
			ParakeetEnvironment.fileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString() + "\\Parakeet";
			try
			{
				using (StreamReader streamReader = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\RSSList.ini", Encoding.GetEncoding("UTF-8")))
				{
					string item;
					while ((item = streamReader.ReadLine()) != null)
					{
						ParakeetEnvironment.rssUrlList.Add(item);
					}
				}
			}
			catch
			{
				MessageBox.Show("RSSList.iniを読み込むことができませんでした", "設定読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}
}
