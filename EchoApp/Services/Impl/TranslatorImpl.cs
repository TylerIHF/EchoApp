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

namespace EchoApp.Services.Impl
{
    public class TranslatorImpl : ITranslator
    {
        public EchoResponseDTO Translate(Echo echo)
        {
            if (echo == null)
            {
                return null;
            }

            EchoResponseDTO dto = new EchoResponseDTO();
            dto.MessageText = echo.MessageText;
            dto.MessageDateTime = echo.MessageDateTime;
            return dto;
        }

        public EchoPageDTO TranslateEchoPage(List<Echo> echoPage, int limit, int page)
        {
            if (echoPage == null)
            {
                return null;
            }

            EchoPageDTO dto = new EchoPageDTO();
            dto.Metadata = TranslateMetadata(limit, page, echoPage.Count);
            dto.Content = new EchoResponseDTO[echoPage.Count];

            for (int index = 0; index < echoPage.Count; index++)
            {
                dto.Content[index] = Translate(echoPage[index]);
            }

            return dto;
        }

        private PageMetadataDTO TranslateMetadata(int limit, int page, int count)
        {
            PageMetadataDTO dto = new PageMetadataDTO();
            dto.Page = page;
            dto.Limit = limit;
            dto.Count = count;
            return dto;
        }
    }
}
