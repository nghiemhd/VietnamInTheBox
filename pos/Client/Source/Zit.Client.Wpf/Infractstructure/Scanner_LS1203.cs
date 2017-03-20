using CoreScanner;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Zit.Client.Wpf.Infractstructure
{
    public class Scanner_LS1203 : IScanner
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(Scanner_LS1203));

        CCoreScanner m_Scanner;

        public Scanner_LS1203()
        {
        }

        void m_Scanner_BarcodeEvent(short eventType, ref string pscanData)
        {
            if (ScanEvent != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pscanData);

                string strData = String.Empty;
                string barcode = xmlDoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
                string symbology = xmlDoc.DocumentElement.GetElementsByTagName("datatype").Item(0).InnerText;
                string[] numbers = barcode.Split(' ');

                foreach (string number in numbers)
                {
                    if (String.IsNullOrEmpty(number))
                    {
                        break;
                    }

                    strData += ((char)Convert.ToInt32(number, 16)).ToString();
                }

                ScanEvent(strData);
            }
        }

        private void __createScanner()
        {
            try
            {
                m_Scanner = new CCoreScanner();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                _log.Debug("Try Factory CCoreScanner Again !");

                Thread.Sleep(1000);
                try
                {
                    m_Scanner = new CCoreScanner();
                }
                catch (Exception ex2)
                {
                    _log.Error(ex2);
                }
            }
            finally {
                if (m_Scanner != null)
                {
                    m_Scanner.BarcodeEvent += m_Scanner_BarcodeEvent;
                }
            }
        }

        public bool TryOpen()
        {
            Status = false;

            __createScanner();

            if (m_Scanner == null) return Status;

            _log.Debug("Try open scanner");

            int status = 1;
            try
            {
                m_Scanner.Open(0, new short[10] { 1,0,0,0,0,0,0,0,0,0 }, (short)1, out status);
                if (status == 0)
                {
                    //Try 
                    string inXml = "<inArgs><cmdArgs><arg-int>6</arg-int><arg-int>1,2,4,8,16,32</arg-int></cmdArgs></inArgs>";
                    string outXml = "";
                    m_Scanner.ExecCommand(1001, ref inXml, out outXml, out status);
                    if(status == 0)
                        Status = true;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return Status;
        }

        public event Action<string> ScanEvent;

        public bool Status { get; private set; }

        public void Dispose()
        {
            if (m_Scanner != null)
            {
                try
                {
                    int status = 1;
                    m_Scanner.Close(0, out status);
                }
                catch(Exception ex)
                {
                    _log.Error(ex);
                }
            }
        }
    }
}
