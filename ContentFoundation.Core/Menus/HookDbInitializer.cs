﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CustomEntityFoundation;

namespace ContentFoundation.Core.Menus
{
    public class HookDbInitializer : IHookDbInitializer
    {
        public int Priority => 10;

        public void Load(IConfiguration config, EntityDbContext dc)
        {
            Directory.GetFiles(EntityDbContext.Options.ContentRootPath + "\\App_Data\\DbInitializer", "*.Menus.json")
                .ToList()
                .ForEach(path =>
                {
                    string json = File.ReadAllText(path);
                    var dbContent = JsonConvert.DeserializeObject<JObject>(json);

                    if(dbContent["menus"] != null)
                    {
                        SaveMenu(dc, dbContent["menus"].ToList());
                    }

                });
        }

        private void SaveMenu(EntityDbContext dc, List<JToken> menus)
        {
            menus.ForEach(jMenu => {

                var dm = jMenu.ToObject<Menu>();
                if (!dm.IsExist<Menu>(dc))
                {
                    dc.Table<Menu>().Add(dm);

                    if (dm.Items != null)
                    {
                        dm.Items.ForEach(subMenu => {

                            subMenu.BundleId = dm.BundleId;
                            subMenu.ParentId = dm.Id;

                            dc.Table<Menu>().Add(subMenu);
                        });
                    }
                }

            });
        }
    }
}
