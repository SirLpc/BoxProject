using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AceNetFrameWork.ace;
using AceNetFrameWork.ace.auto;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IOCPServ serv = new IOCPServ(9999);
                serv.lengthEncode = LengthEncoding.encode;
                serv.lengthDecode = LengthEncoding.decode;
                serv.serEncode = MessageEncoding.Encode;
                serv.serDecode = MessageEncoding.Decode;
                serv.center = new HandlerCenter();
                serv.init();
                serv.Start(6650);
                Console.WriteLine(String.Format( "LOL Server Start At Port:{0},Press Any Key To Shutdown",6650));
            }
            catch (Exception e) {
                Console.WriteLine("Server Error "+e.TargetSite);
                Console.WriteLine( e.Source);
                Console.WriteLine( e.Message);
            }
            LOLServer.tool.ScheduleUtil ins= LOLServer.tool.ScheduleUtil.Instance;
            while (true) { }
        }
    }
}
