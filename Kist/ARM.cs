// Lic:
// Star Story II - Kist Utility
// ARMS scanner
// 
// 
// 
// (c) Jeroen P. Broks, 2023
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 23.02.28
// EndLic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyUnits;

namespace Kist {
    internal class ARM {
        public static SortedDictionary<string, ARM> ARMs = new SortedDictionary<string, ARM>();
        public string
            Name = "",
            Map = "",
            Layer = "";

        public static void Add(string _name, string _map, string _layer) {
            var ret = new ARM();
            ret.Name = _name;
            ret.Map = _map;
            ret.Layer = _layer;
            var Tag = ret.Name.ToUpper();
            if (ARMs.ContainsKey(Tag)) {
                QCol.QuickError("Duplicate ARM Tag!");
                return;
            }
            ARMs[Tag] = ret;
        }
    }
}