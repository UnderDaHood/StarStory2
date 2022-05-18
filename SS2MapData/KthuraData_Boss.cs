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
