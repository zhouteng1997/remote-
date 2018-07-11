using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Threading;

namespace Manager
{
    public class RemoteMachine
    {
        /// <summary>
        /// 桌面名称
        /// </summary>
        public string DesktopName { get { return this._DesktopName ?? this.Server; } set { this._DesktopName = value; } }
        private string _DesktopName = null;
        /// <summary>
        /// 域名
        /// </summary>
        public string WWW { get; set; }
        /// <summary>
        /// 远程桌面的IP地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 服务器类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 在线状态----------暂未启用
        /// </summary>
        public bool Status { get { return _Status; } set { _Status = value; } }
        private bool _Status = false;
        /// <summary>
        /// 允许查询进程
        /// </summary>
        public bool ShowProcess { get { return _ShowProcess; } set { _ShowProcess = value; } }
        private bool _ShowProcess = false;
        /// <summary>
        /// 允许远程
        /// </summary>
        public bool RemoteAble { get { return _RemoteAble; } set { _RemoteAble = value; } }
        private bool _RemoteAble = false;
        /// <summary>
        /// 允许查询服务
        /// </summary>
        public bool ShowService { get { return _ShowService; } set { _ShowService = value; } }
        private bool _ShowService = false;
        /// <summary>
        /// 共享磁盘驱动器
        /// </summary>
        public bool RedirectDrives { get { return _RedirectDrives; } set { _RedirectDrives = value; } }
        private bool _RedirectDrives = false;
        /// <summary>
        /// 共享打印机
        /// </summary>
        public bool RedirectPrinters { get { return _RedirectPrinters; } set { _RedirectPrinters = value; } }
        private bool _RedirectPrinters = false;
        /// <summary>
        /// 共享端口
        /// </summary>
        public bool RedirectPorts { get { return _RedirectPorts; } set { _RedirectPorts = value; } }
        private bool _RedirectPorts = false;


        public override string ToString()
        {
            return string.Format("{0}:{1}", this.DesktopName, this.Server);
        }

        public void Save(IniFile iniFile)
        {
            Save(this, iniFile);
        }

        public void Delete(IniFile iniFile)
        {
            string section = string.Format("Remote({0})", this.DesktopName);
            iniFile.DeleteSection(section);
        }

        public void Load(IniFile iniFile, string desktopName)
        {
            string section = string.Format("Remote({0})", desktopName);
            this.DesktopName = desktopName;
            this.WWW = iniFile.ReadString(section, "WWW", "");
            this.Server = iniFile.ReadString(section, "Server", "");
            this.UserName = iniFile.ReadString(section, "UserName", "");
            this.Password = iniFile.ReadString(section, "Password", "");
        }

        public static RemoteMachine Load(string desktopName, IniFile iniFile)
        {
            string section = string.Format("Remote({0})", desktopName);
            RemoteMachine mac = new RemoteMachine();
            mac.DesktopName = desktopName;
            mac.WWW = iniFile.ReadString(section, "WWW", "");
            mac.Server = iniFile.ReadString(section, "Server", "");
            mac.UserName = iniFile.ReadString(section, "UserName", "");
            mac.Password = iniFile.ReadString(section, "Password", "");
            mac.Type = iniFile.ReadString(section, "Type", "");
            //委托使用0.1秒无响应自动跳过
            //mac= CallWithTimeout(ServerState, mac, 100);
            //ServerState(mac);
            return mac;
        }

        public static void Save(RemoteMachine machine, IniFile iniFile)
        {
            string section = string.Format("Remote({0})", machine.DesktopName);
            iniFile.WriteString(section, "WWW", machine.WWW);
            iniFile.WriteString(section, "DesktopName", machine.DesktopName);
            iniFile.WriteString(section, "Server", machine.Server);
            iniFile.WriteString(section, "UserName", machine.UserName);
            iniFile.WriteString(section, "Password", machine.Password);
            iniFile.WriteString(section, "Type", machine.Type);
        }

        public static List<RemoteMachine> Load(IniFile iniFile,string type,string kongge)
        {
            string[] infos = iniFile.ReadAllSectionNames();
            if (infos != null)
            {
                List<RemoteMachine> macs = new List<RemoteMachine>();
                foreach (string info in infos)
                {
                    string section = info.Substring(7, info.Length - 8);
                    RemoteMachine mac = RemoteMachine.Load(section, iniFile);
                    //如果类型=选择类型，显示列表之中添加
                    if(type=="默认")
                        macs.Add(mac);
                    else
                        if (mac.Type==type) macs.Add(mac);
                }
                return macs;
            }
            return null;
        }

        #region
        /// <summary>
        /// 在线状态查询
        /// </summary>
        /// <param name="remoteMachine"></param>
        /// <returns></returns>
        //public static RemoteMachine ServerState(RemoteMachine remoteMachine)
        //{
        //    Ping ping = new Ping();
        //    PingReply pingReply = ping.Send(remoteMachine.Server,100);
        //    if (pingReply.Status == IPStatus.Success)
        //        remoteMachine.Status = true;
        //    return remoteMachine;
        //    //remoteMachine.Server
        //}
        #endregion


        #region
        //public delegate RemoteMachine chaxun(RemoteMachine remoteMachine);
        //chaxun aaa = ServerState;
        //static RemoteMachine CallWithTimeout(chaxun chaxun, RemoteMachine remoteMachine, int timeoutMilliseconds)
        //{
        //    //线程
        //    Thread threadToKill = null;

        //    //这是一个委托
        //    Action wrappedAction = () =>
        //    {
        //        threadToKill = Thread.CurrentThread;
        //        chaxun(remoteMachine);
        //    };

        //    //委托开始运行
        //    IAsyncResult result = wrappedAction.BeginInvoke(null, null);
        //    //是否到了时间，委托运行的时间result.AsyncWaitHandle.WaitOne
        //    if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
        //    {
        //        //在规定时间完成
        //        wrappedAction.EndInvoke(result);
        //    }
        //    else
        //    {
        //        //未在规定时间内完成，引发异常，结束线程
        //        //threadToKill.Abort();
        //        //throw new TimeoutException();
        //    }
        //    return remoteMachine;
        //}
        #endregion
    }





}
