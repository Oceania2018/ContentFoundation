﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CustomEntityFoundation;

namespace ContentFoundation.RestApi
{
    public class ConfigController : CoreController
    {
        [AllowAnonymous]
        [HttpGet("site")]
        public IActionResult GetSettings()
        {
            IEnumerable<IConfigurationSection> settings = EntityDbContext.Configuration.GetSection("SiteSetting").GetChildren();

            JObject result = new JObject();
            settings.ToList().ForEach(setting => {
                result.Add(setting.Key, setting.Value);
            });

            return  Ok(result);
        }
    }
}