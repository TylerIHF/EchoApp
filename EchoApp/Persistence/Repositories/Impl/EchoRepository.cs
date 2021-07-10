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
using EchoApp.Persistence.Models;

namespace EchoApp.Persistence.Repositories.Impl
{
    // TODO Figure out how to hook this up to a DB for real persistence
    public class EchoRepositoryImpl : IEchoRepository
    {
        // Fake persistence
        private static Dictionary<string, List<Echo>> echos = new Dictionary<string, List<Echo>>();

        public Echo save(Echo echo)
        {
            if (!echos.ContainsKey(echo.UserName))
            {
                echos.Add(echo.UserName, new List<Echo>());
            }

            List<Echo> userEchos = null;
            if (echos.TryGetValue(echo.UserName, out userEchos))
            {
                userEchos.Add(echo);
                return echo;
            }
            else
            {
                // TODO Throw a meaningful exception, this is probably and error
                throw new Exception();
            }
        }

        public List<Echo> GetEchoes(String user, Int32 limit, Int32 page)
        {
            List<Echo> userEchos = null;
            if (echos.TryGetValue(user, out userEchos))
            {
                Int32 offset = limit * page;

                if (offset > userEchos.Count)
                {
                    return new List<Echo>();
                }

                limit = Math.Min(limit, userEchos.Count - offset);
                return userEchos.GetRange(offset, limit);
            }
            else
            {
                // User not found => empty result
                return new List<Echo>();
            }
        }
    }
}
