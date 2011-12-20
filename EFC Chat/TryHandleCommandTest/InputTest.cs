using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SignalR.Samples.MVCChat.Hubs.Chat;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
//using Moq;

namespace ChatUnitTests
{
    [TestFixture]
    public class InputTest
    {
        [Test]
        public void InputError()
        {
            // arrange
            Chat target = new Chat(); // TODO: Initialize to an appropriate value
            string message = "/invalid"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            
            
            // act
            actual = target.TryHandleCommand(message);

            // assert
            Assert.AreEqual(expected,actual, "wrong input");
        }

        [Test]
        public void InputNick()
        {
            // arrange
            Chat target = new Chat(); // TODO: Initialize to an appropriate value
            string message = "/nick user"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;


            // act
            actual = target.TryHandleCommand(message);

            // assert
            Assert.AreEqual(expected, actual, "/nick user OK");
        }

        [Test]
        public void InputRoomSoftEng()
        {
            // arrange
            Chat target = new Chat(); // TODO: Initialize to an appropriate value
            string message = "/room SoftEng"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;


            // act
            actual = target.TryHandleCommand(message);

            // assert
            Assert.AreEqual(expected, actual, "SoftEng doesn't exists");
        }

        [Test]
        public void MessageSend()
        {
            // arrange
            Chat target = new Chat(); // TODO: Initialize to an appropriate value
            string message = "Some Message"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;


            // act
            actual = target.TryHandleCommand(message);

            // assert
            Assert.AreEqual(expected, actual, "Some Message Not OK");
        }

        [Test]
        public void ChangeUserNameNOTOK()
        {
            // arrange
            Chat target = new Chat(); // TODO: Initialize to an appropriate value
            string message = "/nick user"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;


            // act
            actual = target.TryHandleCommand(message);
            message = "/nick user2";
            actual = target.TryHandleCommand(message);
            
            // assert
            Assert.AreEqual(expected, actual, "User not changed to User2 Not OK");
        }

        [Test]
        public void ChangeUserNameOK()
        {
            // arrange
            Chat target = new Chat(); // TODO: Initialize to an appropriate value
            string message = "/nick user"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;


            // act
            actual = target.TryHandleCommand(message);
            message = "/nick user2";
            actual = target.TryHandleCommand(message);

            // assert
            Assert.AreEqual(expected, actual, "User not changed to User2 OK");
        }
       
    }
}