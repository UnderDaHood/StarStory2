// Lic:
// SS2MapData
// Door
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
// Version: 22.05.13
// EndLic

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrickyUnits;
using UseJCR6;

namespace SS2MapData {
	class CDoor {
		public static ListBox Lijst = null;
		public GINIE Data { get; private set; } = GINIE.FromSource($"[meta]\nCreated={DateTime.Now}\nCopyright=Jeroen P. Broks\n");
		public readonly KthuraData Parent;
		static readonly Dictionary<TextBox, string> _TBRegister = new Dictionary<TextBox, string>();

		static public void Register(TextBox TB, string fld) { _TBRegister[TB] = fld; }

		public CDoor(KthuraData _p) { Parent = _p; }

		public List<string> Collections(string Layer) => Data.List("Collections", Layer);
		public List<string> DoorLayers => Data.List("Main", "Layers");

		void AddNew(List<string> L, string item) {
			if (L == DoorLayers) item = $"L:{item}";
			if (!L.Contains(item)) L.Add(item);
			L.Sort();
		}

		public void LoadData(TJCRDIR J) {
			Debug.WriteLine("Loading Doors Data");
			Data = GINIE.FromSource(J.JCR_B("Doors"));
		}

		public void Scan() {
			Lijst.Items.Clear();
			DoorLayers.Clear();
			foreach (var p in Parent.TheMap.Layers) {
				Debug.WriteLine($"DOORS> Scanning layer: {p.Key}");				
				foreach (var o in p.Value.Objects) {
					if (qstr.Prefixed(o.Tag, "DOOR")) {
						if (o.kind == "TiledArea" || o.kind == "StretchedArea") {
							var OTS = o.Tag.Trim().ToUpper().Split('_');
							if (OTS.Length >= 2 && OTS[0].Length == 5) {
								Debug.WriteLine($"DOORS> Door found: {p.Key}::{o.Tag}");
								AddNew(DoorLayers, p.Key);
								var DoorTag = OTS[0][4];
								var Collection = OTS[1];
								var FullTag = $"Object {p.Key}::{o.Tag}";
								Lijst.Items.Add(FullTag);
								AddNew(Collections(p.Key), Collection);
								Data[FullTag, "Collection"] = Collection;
								AddNew(Data.List($"Objects {p.Key}", Collection), o.Tag);
								if (Data[FullTag, "Open"] == "") Data[FullTag, "Open"] = "AUTO";
								if (Data[FullTag, "FirstTime"] == "") {
									Data[FullTag, "FirstTime"] = $"{DateTime.Now}";
									switch (DoorTag) {
										case 'U':
											Data[FullTag, "Frames"] = $"{o.h - 1}";
											Data[FullTag, "MoveX"] = "0";
											Data[FullTag, "MoveY"] = "-1";
											break;
										case 'D':
											Data[FullTag, "Frames"] = $"{o.h - 1}";
											Data[FullTag, "MoveX"] = "0";
											Data[FullTag, "MoveY"] = "1";
											break;
										case 'L':
											Data[FullTag, "Frames"] = $"{o.w - 1}";
											Data[FullTag, "MoveX"] = "-1";
											Data[FullTag, "MoveY"] = "0";
											break;
										case 'R':
											Data[FullTag, "Frames"] = $"{o.w - 1}";
											Data[FullTag, "MoveX"] = "1";
											Data[FullTag, "MoveY"] = "0";
											break;
									}
									Data[FullTag, "AudioOpen"] = "Audio/SFX/Door/Shhh.ogg";
									Data[FullTag, "AudioClose"] = "Audio/SFX/Door/Shhh.ogg";
								}
							} else {
								Confirm.Annoy($"Falsely tagged door ({o.Tag}) found!\nThis door cannot be processed any further!", "ERROR", System.Windows.Forms.MessageBoxIcon.Error);
							}
						} else {
							Confirm.Annoy($"Door ({o.Tag}) is of kind: {o.kind}.\n Only a TiledArea or a StretchedArea can be processed", "ERROR", System.Windows.Forms.MessageBoxIcon.Error);
						}
					}
				}
			}			
		}

		static private bool AutoUpdate = true;
		public void Sync(string ft) {
			AutoUpdate = false;
			foreach(var f in _TBRegister) {
				f.Key.Text = Data[ft, f.Value];
			}
			AutoUpdate = true;
		}

		public void Update(TextBox t,string ft) {
			if (!AutoUpdate) return;
			Data[ft, _TBRegister[t]] = t.Text;
			Debug.WriteLine($"DOORS> ${ft}::{_TBRegister[t]} = {t.Text}");
#if DEBUG
			Debug.WriteLine($"<Doors>\n{Data.ToSource()}\n</Doors>");
#endif
		}
	}
}