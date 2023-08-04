using Horizon.Halo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Horizon.Halo
{
    internal static class Tools
    {
        internal static int? GetIDField(string content)
        {
            var data = JsonSerializer.Deserialize<IDObject>(content);
            return data.ID;
        }
    }
}
