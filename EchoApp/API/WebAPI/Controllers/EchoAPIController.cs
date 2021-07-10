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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using EchoApp.API.DTO;
using EchoApp.Persistence.Models;
using EchoApp.Services;
using EchoApp.Services.Impl;

namespace EchoApp.API.WebAPI.Controllers
{
    [ApiController]
    public class EchoAPIController : ControllerBase
    {
        private readonly IEchoService _echoService;
	private readonly ITranslator _translator;

	private readonly ILogger<EchoAPIController> _logger;

        public EchoAPIController(
			IEchoService echoService,
			ITranslator translator,
		       	ILogger<EchoAPIController> logger)
        {
            _echoService = echoService;
	    _translator = translator;
	    _logger = logger;
        }

        [HttpPost]
        [Route("api/echo")]
        public EchoResponseDTO DoEcho([FromBody] EchoRequestDTO request)
        {
            Echo echo = _echoService.DoEcho("me", request.MessageText);
            return _translator.Translate(echo);
        }

        [HttpGet]
        [Route("api/echo")]
        public EchoPageDTO GetEchos(Int32 limit, Int32 page)
        {
            List<Echo> echoPage = _echoService.GetEchoes("me", limit, page);
            return _translator.TranslateEchoPage(echoPage, limit, page);
        }
    }
}
