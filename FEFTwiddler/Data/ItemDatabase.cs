﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FEFTwiddler.Extensions;

namespace FEFTwiddler.Data
{
    public class ItemDatabase : BaseDatabase
    {
        public ItemDatabase(Enums.Language language) : base(language)
        {
            _data = XElement.Parse(Properties.Resources.Data_Items);
        }

        public Item GetByID(Enums.Item itemId)
        {
            var row = _data
                .Elements("item")
                .Where((x) => x.Attribute("id").Value == ((ushort)itemId).ToString())
                .First();

            return FromElement(row);
        }

        /// <summary>
        /// Get all skills (even unlearnable ones)
        /// </summary>
        public IEnumerable<Item> GetAll()
        {
            var elements = _data.Elements("item");
            var rows = new List<Item>();
            foreach (var e in elements)
            {
                rows.Add(FromElement(e));
            }
            return rows;
        }

        private Item FromElement(XElement row)
        {
            var displayName = GetDisplayName(row);

            return new Item
            {
                ItemID = (Enums.Item)row.GetAttribute<ushort>("id"),
                DisplayName = displayName,
                Type = (Enums.ItemType)Enum.Parse(typeof(Enums.ItemType), row.GetAttribute("type")),
                MaximumUses = row.GetAttribute<byte>("maxUses"),
                IsEnemyOnly = row.GetAttribute("enemyOnly", false),
                IsMapOnly = row.GetAttribute("mapOnly", false)
            };
        }
    }
}
