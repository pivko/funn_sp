using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.SharePoint.Client;
using FUNN_SP_PROXIES;
using FUNN_SP_AUTH;
using FUNN_SP_CRAWLER;

using SP = Microsoft.SharePoint.Client;

namespace FUNN_SP {
	
    class Program {
       
        static void Main(string[] args) {
			FunnelbackCrawler oCrawler = new FunnelbackCrawler();
			oCrawler.Crawl();
		}
    }
}
