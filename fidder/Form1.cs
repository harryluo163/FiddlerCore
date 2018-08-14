using Fiddler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fidder
{
    public partial class 船货不二抓取系统 : Form
    {
        static Proxy oSecureEndpoint;
        static string sSecureEndpointHostname = "localhost";
        static int iSecureEndpointPort = 8888;
        private IList<data> dataList = new List<data>();
        RegFunc rf = new RegFunc();
        public 船货不二抓取系统()
        {
            InitializeComponent();
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
            txtlog.AppendText("代理端口为8877" + Environment.NewLine);
            foreach (var item in ipEntry.AddressList)
            {
                if (item.ToString().IndexOf('.') > 0)
                {
                    txtlog.AppendText("IP：" + item.ToString() + Environment.NewLine);
                }

            }
            txtlog.AppendText("请先修改wifi代理" + Environment.NewLine);

            string path = "down\\";
            if (Directory.Exists(path))
            {
               
            }
            else
            {
                Directory.CreateDirectory(path);
               
            }

        }

        private void btnstart_Click(object sender, EventArgs e)
        {

            btnout.Enabled = true;
            btnstart.Enabled = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            //设置别名
            Fiddler.FiddlerApplication.SetAppDisplayName("FiddlerCoreDemoApp");

            //启动方式
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;

            //定义http代理端口
            int iPort = 8877;
            //启动代理程序，开始监听http请求
            //端口,是否使用windows系统代理（如果为true，系统所有的http访问都会使用该代理）我使用的是
            Fiddler.FiddlerApplication.Startup(iPort, false, false, true);

            // 我们还将创建一个HTTPS监听器，当FiddlerCore被伪装成HTTPS服务器有用
            // 而不是作为一个正常的CERN样式代理服务器。
            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
            txtlog.AppendText("启动代理成功" + Environment.NewLine);

            List<Fiddler.Session> oAllSessions = new List<Fiddler.Session>();

            ////请求出错时处理
            //Fiddler.FiddlerApplication.BeforeReturningError += FiddlerApplication_BeforeReturningError;

            //在发送请求之前执行的操作
            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS)
            {
                //请求的全路径
                Console.WriteLine("Before request for:\t" + oS.fullUrl);
                // 为了使反应篡改，必须使用缓冲模式
                // 被启用。这允许FiddlerCore以允许修改
                // 在BeforeResponse处理程序中的反应，而不是流
                // 响应给客户机作为响应进来。

                oS.bBufferResponse = true;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
                oS["X-AutoAuth"] = "(default)";

                /* 如果请求是要我们的安全的端点，我们将回显应答。
                
                注：此BeforeRequest是越来越要求我们两个主隧道代理和我们的安全的端点，
                让我们来看看它的Fiddler端口连接到（pipeClient.LocalPort）客户端，以确定是否该请求
                被发送到安全端点，或为了达到**安全端点被仅仅发送到主代理隧道（例如，一个CONNECT）。
                因此，如果你运行演示和参观的https：//本地主机：7777在浏览器中，你会看到
                Session list contains...
                 
                    1 CONNECT http://localhost:7777
                    200                                         <-- CONNECT tunnel sent to the main proxy tunnel, port 8877
                    2 GET https://localhost:7777/
                    200 text/html                               <-- GET request decrypted on the main proxy tunnel, port 8877
                    3 GET https://localhost:7777/               
                    200 text/html                               <-- GET request received by the secure endpoint, port 7777
                */

                //oS.utilCreateResponseAndBypassServer();
                //oS.oResponse.headers.SetStatus(200, "Ok");
                //string str = oS.GetResponseBodyAsString();
                //oS.utilSetResponseBody(str + "aaaaaaaaaaaaaaaaaaaaa");

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
                //if ((oS.oRequest.pipeClient.LocalPort == 8877) && (oS.hostname == "www.baidu.com"))
                //{
                //    string url = oS.fullUrl;
                //    oS.utilCreateResponseAndBypassServer();
                //    oS.oResponse.headers.SetStatus(200, "Ok");
                //    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                //    oS.oResponse["Cache-Control"] = "private, max-age=0";
                //    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                //}
            };

            /*
                // 下面的事件，您可以检查由Fiddler阅读每一响应缓冲区。  
             *     请注意，这不是为绝大多数应用非常有用，因为原始缓冲区几乎是无用的;它没有解压，它包括标题和正文字节数等。
                //
                // 本次仅适用于极少数的应用程序这就需要一个原始的，未经处理的字节流获取有用
                Fiddler.FiddlerApplication.OnReadResponseBuffer += new EventHandler<RawReadEventArgs>(FiddlerApplication_OnReadResponseBuffer);
            */


            Fiddler.FiddlerApplication.BeforeResponse += delegate (Fiddler.Session oS)
            {


                if (oS.fullUrl.IndexOf("http://www.chuanhuobu2.com/mercury/api/ships/detail?") >= 0)
                {
                    //已抓到数据http://www.chuanhuobu2.com/mercury/api/ships/detail?ship=10299742&origin=0
                    //已抓到数据{ "id":10299742,"version":1741228,"item":{ "startPort":"徐州","blankStart":"","aimPort":"","date":"08月14日±4天","dateText":"今天","weight":"2500吨","weightText":2500,"note":""},"state":{ "ordered":0,"pause":0,"showTime":"1小时前"},"tag":{ "level":"10级","relation":null,"certified":true,"goodPhone":true,"idcardCertified":true,"honestyAvatar":false,"geoLocation":"","photo":null},"user":{ "name":"毛传北","avatar":null,"newAvatar":null,"mobiles":"15951271769","vessel":"豫泓翔588"},"attitude":{ "friend":0,"baddie":0} }
                    //已抓到数据http://www.chuanhuobu2.com/mercury/api/ships/detail?ship=10299742&origin=0
                    //已抓到数据{ "id":10299742,"version":1741228,"item":{ "startPort":"徐州","blankStart":"","aimPort":"","date":"08月14日±4天","dateText":"今天","weight":"2500吨","weightText":2500,"note":""},"state":{ "ordered":0,"pause":0,"showTime":"1小时前"},"tag":{ "level":"10级","relation":null,"certified":true,"goodPhone":true,"idcardCertified":true,"honestyAvatar":false,"geoLocation":"","photo":null},"user":{ "name":"***","newAvatar":null,"mobiles":"159********","vessel":"豫泓翔***"},"attitude":{ "friend":0,"baddie":0} }

                    oS.utilDecodeResponse();
                    string content = oS.GetResponseBodyAsString();
                    Regex rx = new Regex(@"^0{0,1}(13[4-9]|15[7-9]|15[0-2]|18[7-8])[0-9]{8}$");
                    txtlog.AppendText("已抓到数据" + oS.GetResponseBodyAsString() + Environment.NewLine);
                    if (System.Text.RegularExpressions.Regex.IsMatch(rf.GetStr(content, "\"mobiles\":\"", "\","), @"^[0-9]*$"))
                    {
                        txtlog.AppendText("手机号是" + rf.GetStr(content, "\"mobiles\":", ",") + "可以保存" + Environment.NewLine);
                        CleanData(content, "");
                    }
                    else
                    {
                        txtlog.AppendText("手机号是" + rf.GetStr(content, "\"mobiles\":", ",") + "需要查看哦" + Environment.NewLine);
                        dataList.Add(new data { id = rf.GetStr(content, "\"id\":", ","), content = content });
                    }


                }
                else if (oS.fullUrl.IndexOf("http://www.chuanhuobu2.com/mercury/api/pallets/make-link-contact") >= 0)
                {
                    //已抓到数据http://www.chuanhuobu2.com/mercury/api/pallets/make-link-contact
                    //已抓到数据{ "id":10278241,"mobiles":["18136310777"],"ordered":0,"pause":0,"avatar":null,"status":0,"username":"冯对桃","vesselName":"徐货9780","attitude":{ "friend":0,"baddie":0},"originScore":0}
                    oS.utilDecodeResponse();
                    string content = oS.GetResponseBodyAsString();
                    if (content.Length > 50)
                    {
                        CleanData("", content);

                    }
                    else {
                        txtlog.AppendText("数据异常"+ content + Environment.NewLine);
                    }

                }


                if (txtlog.Lines.Count() > 3000)
                {
                    txtlog.Text = "";
                    txtlog.AppendText("清空缓存" + Environment.NewLine);
                }


            };

        }

        public void CleanData(string one, string two)
        {
            string content = "";
            string mobiles = ""; string vesselName = "";
            string _datastr = "";
            string Path = "down\\船期数据.txt";
            if (!File.Exists(Path))
            {
                using (new FileStream(Path, FileMode.Create, FileAccess.Write)) { };
            }
            //获取手机号
            if (one == "")
            {

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].id != rf.GetStr(two, "\"id\":", ","))
                    {
                        content = dataList[i].content;
                        dataList.Remove(dataList[i]);
                        break;
                    }

                }
                #region 相同
                _datastr += "<id>" + rf.GetStr(content, "\"id\":", ",") + "</id>";
                _datastr += "<version>" + rf.GetStr(content, "\"version\":", ",") + "</version>";
                _datastr += "<startPort>" + rf.GetStr(content, "\"startPort\":\"", "\",") + "</startPort>";
                _datastr += "<blankStart>" + rf.GetStr(content, "\"blankStart\":\"", "\",") + "</blankStart>";
                _datastr += "<aimPort>" + rf.GetStr(content, "\"aimPort\":\"", "\",") + "</aimPort>";
                _datastr += "<date>" + rf.GetStr(content, "\"date\":\"", "\",") + "</date>";
                _datastr += "<dateText>" + rf.GetStr(content, "\"dateText\":\"", "\",") + "</dateText>";
                _datastr += "<weight>" + rf.GetStr(content, "\"weight\":\"", "\",") + "</weight>";
                _datastr += "<weightText>" + rf.GetStr(content, "\"weightText\":\"", "\"") + "</weightText>";
                _datastr += "<pause>" + rf.GetStr(content, "\"pause\":", ",") + "</pause>";
                _datastr += "<showTime>" + rf.GetStr(content, "\"showTime\":\"", "\"") + "</showTime>";

                _datastr += "<level>" + rf.GetStr(content, "\"level\":\"", "\"") + "</level>";
                _datastr += "<relation>" + rf.GetStr(content, "\"relation\":", ",") + "</relation>";
                _datastr += "<certified>" + rf.GetStr(content, "\"certified\":", ",") + "</certified>";
                _datastr += "<goodPhone>" + rf.GetStr(content, "\"goodPhone\":", ",") + "</goodPhone>";
                _datastr += "<idcardCertified>" + rf.GetStr(content, "\"idcardCertified\":", ",") + "</idcardCertified>";
                _datastr += "<honestyAvatar>" + rf.GetStr(content, "\"honestyAvatar\":", ",") + "</honestyAvatar>";
                _datastr += "<geoLocation>" + rf.GetStr(content, "\"geoLocation\":", ",") + "</geoLocation>";
                _datastr += "<file>" + rf.GetStr(content, "\"file\":\"", "\",") + "</file>";
                _datastr += "<time>" + rf.GetStr(content, "\"time\":\"", "\",") + "</time>";
                _datastr += "<location>" + rf.GetStr(content, "\"location\":\"", "\",") + "</location>";

                #endregion

                _datastr += "<username>" + rf.GetStr(two, "\"username\":\"", "\",") + "</username>";
                _datastr += "<vesselName>" + rf.GetStr(two, "\"vesselName\":\"", "\",") + "</vesselName>";
                _datastr += "<mobiles>" + rf.GetStr(two, "\"mobiles\":\\[\"", "\"],") + "</mobiles>";
                mobiles = rf.GetStr(two, "\"mobiles\":\\[\"", "\"],");
                vesselName = rf.GetStr(two, "\"vesselName\":\"", "\",");
            }
            else if (two == "")
            {
                //直接保存
                content = one;
                #region 相同
                _datastr += "<id>" + rf.GetStr(content, "\"id\":", ",") + "</id>";
                _datastr += "<version>" + rf.GetStr(content, "\"version\":", ",") + "</version>";
                _datastr += "<startPort>" + rf.GetStr(content, "\"startPort\":\"", "\",") + "</startPort>";
                _datastr += "<blankStart>" + rf.GetStr(content, "\"blankStart\":\"", "\",") + "</blankStart>";
                _datastr += "<aimPort>" + rf.GetStr(content, "\"aimPort\":\"", "\",") + "</aimPort>";
                _datastr += "<date>" + rf.GetStr(content, "\"date\":\"", "\",") + "</date>";
                _datastr += "<dateText>" + rf.GetStr(content, "\"dateText\":\"", "\",") + "</dateText>";
                _datastr += "<weight>" + rf.GetStr(content, "\"weight\":\"", "\",") + "</weight>";
                _datastr += "<weightText>" + rf.GetStr(content, "\"weightText\":", ",") + "</weightText>";
                _datastr += "<pause>" + rf.GetStr(content, "\"pause\":", ",") + "</pause>";
                _datastr += "<showTime>" + rf.GetStr(content, "\"showTime\":\"", "\"") + "</showTime>";
                _datastr += "<level>" + rf.GetStr(content, "\"level\":\"", "\"") + "</level>";
                _datastr += "<relation>" + rf.GetStr(content, "\"relation\":", ",") + "</relation>";
                _datastr += "<certified>" + rf.GetStr(content, "\"certified\":", ",") + "</certified>";
                _datastr += "<goodPhone>" + rf.GetStr(content, "\"goodPhone\":", ",") + "</goodPhone>";
                _datastr += "<idcardCertified>" + rf.GetStr(content, "\"idcardCertified\":", ",") + "</idcardCertified>";
                _datastr += "<honestyAvatar>" + rf.GetStr(content, "\"honestyAvatar\":", ",") + "</honestyAvatar>";
                _datastr += "<geoLocation>" + rf.GetStr(content, "\"geoLocation\":", ",") + "</geoLocation>";
                _datastr += "<file>" + rf.GetStr(content, "\"file\":\"", "\",") + "</file>";
                _datastr += "<time>" + rf.GetStr(content, "\"time\":\"", "\",") + "</time>";
                _datastr += "<location>" + rf.GetStr(content, "\"location\":\"", "\",") + "</location>";
                #endregion

                _datastr += "<username>" + rf.GetStr(content, "\"name\":\"", "\",") + "</username>";
                _datastr += "<vesselName>" + rf.GetStr(content, "\"vessel\":\"", "\"") + "</vesselName>";
                _datastr += "<mobiles>" + rf.GetStr(content, "\"mobiles\":\"", "\",") + "</mobiles>";
                mobiles = rf.GetStr(content, "\"mobiles\":\"", "\",");
                vesselName = rf.GetStr(content, "\"vessel\":\"", "\"");

            }

            using (StreamWriter sw = new StreamWriter(Path, true, Encoding.Default))
            {

                sw.Write(_datastr + "\r\n");
                txtlog.AppendText("已抓到" + rf.GetStr(two, "\"username\":\"", "\",") + "船舶" + vesselName + "的手机号" + mobiles + Environment.NewLine);
            }

        }

        private void btnout_Click(object sender, EventArgs e)
        {
            btnstart.Enabled = true;
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
            Fiddler.FiddlerApplication.Shutdown();
            txtlog.AppendText("断开代理成功" + Environment.NewLine);
            btnout.Enabled = false;
            Thread.Sleep(500);

        }

        private void txtview_TextChanged(object sender, EventArgs e)
        {

        }
    }
    class RegFunc
    {
        public ArrayList GetStrArr(string pContent, string regBegKey, string regEndKey)
        {
            ArrayList arr = new ArrayList();
            string regular = "(?<={0})(.|\n)*?(?={1})";
            regular = string.Format(regular, regBegKey, regEndKey);
            Regex regex = new Regex(regular, RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(pContent);
            foreach (Match m in mc)
            {
                arr.Add(m.Value.Trim());
            }
            return arr;
        }

        public string GetStr(string pContent, string regBegKey, string regEndKey)
        {
            string regstr = "";
            string regular = "(?<={0})(.|\n)*?(?={1})";
            regular = string.Format(regular, regBegKey, regEndKey);
            Regex regex = new Regex(regular, RegexOptions.IgnoreCase);
            Match m = regex.Match(pContent);
            if (m.Length > 0)
            {
                regstr = m.Value.Trim();
            }
            return regstr;
        }



    }
}
