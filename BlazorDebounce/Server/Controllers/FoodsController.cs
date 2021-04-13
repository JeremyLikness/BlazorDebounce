// Copyright (c) Jeremy Likness. All rights reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BlazorDebounce.Client;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDebounce.Server.Controllers
{
    /// <summary>
    /// The foods controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        /// <summary>
        /// The list of food items.
        /// </summary>
        private static readonly List<FoodItem> FoodItems;

        static FoodsController()
        {
            FoodItems = new ();
            var resource = $"{typeof(FoodsController).Namespace}.food.csv";
            using var stream = typeof(FoodsController).Assembly
                .GetManifestResourceStream(resource);
            using var textReader = new StreamReader(stream);
            var rows = new List<string>();
            string row = string.Empty;
            do
            {
                row = textReader.ReadLine();
                if (string.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                var cols = row.Split(",")
                    .Select(col => col.Replace("\"", string.Empty))
                    .ToArray();
                if (cols.Length == 6)
                {
                    if (int.TryParse(cols[0], out int id) && id > 0)
                    {
                        var foodItem = new FoodItem
                        {
                            Id = id,
                            Description = cols[2],
                        };
                        FoodItems.Add(foodItem);
                    }
                }
            }
            while (!string.IsNullOrWhiteSpace(row));
        }

        /// <summary>
        /// Fetch food items.
        /// </summary>
        /// <param name="text">The search text.</param>
        /// <returns>The list of matching items.</returns>
        [HttpGet]
        public IEnumerable<FoodItem> Get([FromQuery] string text)
        {
            IEnumerable<FoodItem> result = Enumerable.Empty<FoodItem>();

            if (text != null)
            {
                var safeText = text.Trim().ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(safeText))
                {
                    result = FoodItems.Where(fi => fi.Description
                    .Trim().ToLowerInvariant().Contains(safeText));
                }
            }

            Thread.Sleep(1000);
            return result;
        }
    }
}
