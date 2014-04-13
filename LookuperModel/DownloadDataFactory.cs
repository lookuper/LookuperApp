using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    public class DownloadDataFactory
    {
        private static volatile DownloadDataFactory instance;
        private static Object syncRoot = new Object();

        public static DownloadDataFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DownloadDataFactory();
                    }
                }

                return instance;
            }
        }

        public WebClient WebClient
        {
            get
            {
                var client = new WebClient();
                client.Encoding = Encoding.UTF8;

                client.Headers.Add("Content-Type: application/x-www-form-urlencoded");
                client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2");
                client.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                client.Headers.Add("Accept-Encoding: identity");
                client.Headers.Add("Accept-Language: en-US,en;q=0.8");
                client.Headers.Add("Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3");

                return client;
            }
        }

        private DownloadDataFactory()
        {
        }
    }
}
