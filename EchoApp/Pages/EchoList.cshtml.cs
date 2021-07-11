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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using EchoApp.API.DTO;
using EchoApp.Persistence.Models;
using EchoApp.Services;
using EchoApp.Services.Impl;

namespace EchoApp.Pages
{
    [BindProperties]
    public class EchoListModel : PageModel
    {
        private readonly IEchoService _echoService;
        private readonly ITranslator _translator;

        private readonly ILogger<EchoListModel> _logger;

	public EchoPageDTO DTO { get; set; }

        public EchoListModel(
			IEchoService echoService,
                        ITranslator translator,
			ILogger<EchoListModel> logger)
        {
	    _echoService = echoService;
	    _translator = translator;
            _logger = logger;
        }

        public void OnGet()
        {
	    int limit = 5;
	    StringValues pageString = Request.Query["page"];
	    int page = pageString == StringValues.Empty ? 0 : Int32.Parse(pageString[0]);
	    List<Echo> echoPage = _echoService.GetEchoes(User.Identity.Name, limit, page);
	    DTO = _translator.TranslateEchoPage(echoPage, limit, page);
        }
    }
}
