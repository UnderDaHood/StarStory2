// Lic:
// Map Data Editor
// Bosses
// 
// 
// 
// (c) Jeroen P. Broks, 2022
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
// Version: 22.05.19
// EndLic

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrickyUnits;

namespace SS2MapData {
	partial class KthuraData { 
		public class Boss {
			readonly string Tag;
			readonly KthuraData Parent;

			public int this[int idx,int skill] {
				get => qstr.ToInt(Parent.Foes[Tag, $"Level{idx}_{skill}"]);
				set { Parent.Foes[Tag, $"Level{idx}_{skill}"] = value.ToString(); }
			}
			public string this[int idx] {
				get => Parent.Foes[Tag, $"Foe{idx}"];
				set { Parent.Foes[Tag, $"Foe{idx}"] = value; }
			}
			
			public string Arena {
				get => Parent.Foes[Tag, "Arena"];
				set { Parent.Foes[Tag, "Arena"] = value; }

			}

			public string Music {
				get {
					if (Parent.Foes[Tag, "Tune"] == "") Parent.Foes[Tag, "Tune"] = "Music/Combat/Boss/";
					return Parent.Foes[Tag, "Tune"];
				}
				set {
					Parent.Foes[Tag, "Tune"] = value;
				}
			}

			internal Boss(string _Tag,KthuraData _Parent) {
				Parent = _Parent;
				Tag = _Tag;
			}
		}
		SortedDictionary<string, Boss> BossList = new SortedDictionary<string, Boss>();
		public static ListBox GadgetBossList = null;
		public Boss CurBoss {
			get {
				var B = GadgetBossList.SelectedItem;
				if (B == null) return null;
				return BossList[(string)B];
			}
		}

		void ScanBosses() {
			BossList.Clear();
			foreach (var item in Foes.EachSections) { if (qstr.Prefixed(item, "BOSS::VITAL::")) BossList[item] = new Boss(item,this); }
			foreach (var lay in TheMap.Layers) {
				if (lay.Value.HasTag("Boss_Activate") && lay.Value.HasTag("Boss_Actor")) {
					var tag = $"BOSS::REGLR::{lay.Key}";
					BossList[tag] = new Boss(tag,this);
					Debug.WriteLine($"Boss detected on layer {lay.Key}");
				}
			}
			GadgetBossList.Items.Clear();
			foreach(var b in BossList.Keys) GadgetBossList.Items.Add(b);   
		}
	}
}