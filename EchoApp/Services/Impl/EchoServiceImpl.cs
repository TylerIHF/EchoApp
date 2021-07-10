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

namespace EchoApp.Services.Impl
{
    public class EchoServiceImpl : IEchoService
    {
        private IEchoRepository echoRepository;

        public EchoServiceImpl(IEchoRepository echoRepository)
        {
            this.echoRepository = echoRepository;
        }

        public Echo DoEcho(String user, string message)
        {
            if (user == null)
            {
                throw new MissingUserException();
            }

            if (message == null)
            {
                throw new MissingMessageException();
            }

            Echo echo = new Echo
            {
                MessageText = message,
                UserName = user,
                MessageDateTime = DateTime.Now
            };

            return echoRepository.save(echo);
        }

        public List<Echo> GetEchoes(String user, Int32 limit, Int32 page)
        {
            return echoRepository.GetEchoes(user, limit, page);
        }
    }
}
