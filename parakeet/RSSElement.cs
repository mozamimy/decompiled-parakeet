using System;
using System.Collections.Generic;
namespace parakeet
{
	public class RSSElement
	{
		public string title = "";
		public string link = "";
		public string description = "";
		public List<RSSItem> items = new List<RSSItem>();
		public List<string> soundFiles = new List<string>();
		public bool elementEnabled = true;
		public RSSElement()
		{
		}
		public RSSElement(bool p)
		{
			this.elementEnabled = p;
		}
	}
}
