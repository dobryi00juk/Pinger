//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Pinger;
//using Pinger.Configuration;
//using PingerLibTests;
//using System;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using System.Text;

//namespace Pinger.Tests
//{
//    [TestClass()]
//    public class TcpPingerTests
//    {
//        //[TestMethod()]
//        //public void TcpPingerTest()
//        //{
//        //    Assert.Fail();
//        //}

//        [TestMethod()]
//        public void CheckStatusAsyncTest()
//        {
//            //Arrage
//            var th = new TestHelper();
//            var config = th.LoadConfiguration();
//            var setting = new Settings(config);
//            var tcpClient = new TcpClient();

//            //Act
//            var tcpPinger = new TcpPinger(tcpClient, setting);
//            var result = tcpPinger.CheckStatus();

//            //Assert
//            Assert.AreEqual("OK", result);
//        }
//    }
//}