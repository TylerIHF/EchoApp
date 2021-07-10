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
using EchoApp.API.DTO;
using EchoApp.Persistence.Models;
using EchoApp.Services;
using EchoApp.Services.Impl;
using NUnit.Framework;

namespace EchoApp.Tests.Services
{
    [TestFixture]
    public class TranslatorServiceTest
    {
        [Test]
        public void TranslateEchoPristene()
        {
            // Setup
            ITranslator translator = new TranslatorImpl();
            string user = Guid.NewGuid().ToString();
            string message = Guid.NewGuid().ToString();
            DateTime nowish = DateTime.Now;
            Echo echo = new Echo
            {
                MessageText = message,
                UserName = user,
                MessageDateTime = nowish
            };

            // Act
            EchoResponseDTO dto = translator.Translate(echo);

            // Assert
            Assert.AreEqual(dto.MessageText, message);
            Assert.AreEqual(dto.MessageDateTime, nowish);
        }

        [Test]
        public void TranslateEchoNullOK()
        {
            // Setup
            ITranslator translator = new TranslatorImpl();
            Echo echo = null;

            // Act
            EchoResponseDTO dto = translator.Translate(echo);

            // Assert
            Assert.AreEqual(dto, null);
        }

        [Test]
        public void TranslateEchoPagePristene()
        {
            // Setup
            ITranslator translator = new TranslatorImpl();
            int limit = 10;
            int page = 1;
            List<Echo> echoPage = new List<Echo>();
            string user = Guid.NewGuid().ToString();
            string message = Guid.NewGuid().ToString();
            DateTime nowish = DateTime.Now;
            Echo echo = new Echo
            {
                MessageText = message,
                UserName = user,
                MessageDateTime = nowish
            };
            echoPage.Add(echo);

            // Act
            EchoPageDTO dto = translator.TranslateEchoPage(echoPage, limit, page);

            // Assert
            Assert.AreEqual(dto.Content.Length, 1);
            Assert.AreEqual(dto.Content[0].MessageDateTime, nowish);
            Assert.AreEqual(dto.Content[0].MessageText, message);
        }

        [Test]
        public void TranslateEchoPageNullOK()
        {
            // Setup
            ITranslator translator = new TranslatorImpl();
            int limit = 10;
            int page = 1;
            List<Echo> echoPage = null;

            // Act
            EchoPageDTO dto = translator.TranslateEchoPage(echoPage, limit, page);

            // Assert
            Assert.AreEqual(dto, null);
        }

        [Test]
        public void TranslateEchoPageEmpty()
        {
            // Setup
            ITranslator translator = new TranslatorImpl();
            int limit = 10;
            int page = 1;
            List<Echo> echoPage = new List<Echo>();

            // Act
            EchoPageDTO dto = translator.TranslateEchoPage(echoPage, limit, page);

            // Assert
            Assert.AreEqual(dto.Content.Length, 0);
            Assert.AreEqual(dto.Metadata.Page, page);
            Assert.AreEqual(dto.Metadata.Limit, limit);
        }

        [Test]
        public void TranslateEchoPageSimpleMetadata()
        {
            // Setup
            ITranslator translator = new TranslatorImpl();
            int limit = 10;
            int page = 1;
            List<Echo> echoPage = new List<Echo>();
            string user = Guid.NewGuid().ToString();
            string message = Guid.NewGuid().ToString();
            DateTime nowish = DateTime.Now;
            Echo echo = new Echo
            {
                MessageText = message,
                UserName = user,
                MessageDateTime = nowish
            };
            echoPage.Add(echo);

            // Act
            EchoPageDTO dto = translator.TranslateEchoPage(echoPage, limit, page);

            // Assert
            Assert.AreEqual(dto.Metadata.Count, 1);
            Assert.AreEqual(dto.Metadata.Page, page);
            Assert.AreEqual(dto.Metadata.Limit, limit);
        }
    }
}
