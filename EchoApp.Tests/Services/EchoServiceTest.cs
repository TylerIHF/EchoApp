/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using EchoApp.Exceptions;
using EchoApp.Persistence.Models;
using EchoApp.Persistence.Repositories;
using EchoApp.Persistence.Repositories.Impl;
using EchoApp.Services;
using EchoApp.Services.Impl;
using NUnit.Framework;

namespace EchoApp.Tests.Services
{
    [TestFixture]
    public class EchoServiceTest
    {
        [Test]
        public void DoEchoPristene()
        {
            // Setup
            IEchoRepository echoRepository = new EchoRepositoryImpl();
            IEchoService echoService = new EchoServiceImpl(echoRepository);
            string user = Guid.NewGuid().ToString();
            string message = Guid.NewGuid().ToString();

            // Act
            DateTime before = DateTime.Now;
            System.Threading.Thread.Sleep(1);
            Echo echo = echoService.DoEcho(user, message);
            System.Threading.Thread.Sleep(1);
            DateTime after = DateTime.Now;

            // Assert
            Assert.AreEqual(echo.MessageText, message);
            Assert.AreEqual(echo.UserName, user);
            Assert.IsTrue(echo.MessageDateTime.CompareTo(before) > 0);
            Assert.IsTrue(echo.MessageDateTime.CompareTo(after) < 0);
        }

        [Test]
        public void DoEchoUserNotNull()
        {
            // Setup
            IEchoRepository echoRepository = new EchoRepositoryImpl();
            IEchoService echoService = new EchoServiceImpl(echoRepository);
            string user = null;
            string message = Guid.NewGuid().ToString();

            // Assert
            Assert.Throws<MissingUserException>(delegate
            {
                echoService.DoEcho(user, message);
            });
        }

        [Test]
        public void DoEchoMessageNotNull()
        {
            // Setup
            IEchoRepository echoRepository = new EchoRepositoryImpl();
            IEchoService echoService = new EchoServiceImpl(echoRepository);
            string user = Guid.NewGuid().ToString();
            string message = null;

            // Assert
            Assert.Throws<MissingMessageException>(delegate
            {
                echoService.DoEcho(user, message);
            });
        }

        [Test]
        public void DoEchoSeperateHistory()
        {
            // Setup
            IEchoRepository echoRepository = new EchoRepositoryImpl();
            IEchoService echoService = new EchoServiceImpl(echoRepository);
            string user1 = Guid.NewGuid().ToString();
            string user2 = Guid.NewGuid().ToString();
            string message1 = Guid.NewGuid().ToString();
            string message2 = Guid.NewGuid().ToString();
            string message3 = Guid.NewGuid().ToString();
            string message4 = Guid.NewGuid().ToString();

            // Act
            echoService.DoEcho(user1, message1);
            echoService.DoEcho(user2, message2);
            echoService.DoEcho(user1, message3);
            echoService.DoEcho(user2, message4);
            List<Echo> user1History = echoService.GetEchoes(user1, 5, 0);
            List<Echo> user2History = echoService.GetEchoes(user2, 5, 0);
            List<string> user1Messages = GetMessages(user1History);
            List<string> user2Messages = GetMessages(user2History);

            // Assert
            Assert.AreEqual(user1History.Count, 2);
            Assert.Contains(message1, user1Messages);
            Assert.Contains(message3, user1Messages);
            Assert.AreEqual(user2History.Count, 2);
            Assert.Contains(message2, user2Messages);
            Assert.Contains(message4, user2Messages);
        }

        private List<string> GetMessages(List<Echo> history)
        {
            List<string> messages = new List<string>();

            foreach (Echo echo in history)
            {
                messages.Add(echo.MessageText);
            }

            return messages;
        }
    }
}
