// Lic:
// Map Extra Data Editor
// Kthura Data
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
// Version: 22.04.20
// EndLic

using NSKthura;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrickyUnits;
using UseJCR6;

namespace SS2MapData {
	partial class KthuraData {
		const string Author = "Jeroen P. Broks";
		const string Notes = "Part of the Star Story universe or the Phantasar universe. Copyrighted and owned by Jeroen P. Broks. This file may only be distributed with an official UNMODIFIED version of the game and not be extracted from the game";
		public const string _IAADir = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Script/Data/IAA/";
		public const string _FoeDir = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/Dev/Foes/Data";
		public static string IAADir => Dirry.AD(_IAADir);
		public static string FoeDir => Dirry.AD(_FoeDir);
		public static string[] ItemFiles;
		public static string[] FoeFiles;

		readonly string pathlessfile;
		public string file => $"{Config.KthuraDir}{pathlessfile}";
		public Kthura TheMap { private set; get; } = null;
		Dictionary<string, byte[]> Unknown = new Dictionary<string, byte[]>();
		public GINIE Layers = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		public GINIE Foes = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		public GINIE Treasures = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		public CDoor Door = null;
		public string BehaviorSource = "";
		//GINIE Layers => SS2MapData.Layers.Data; //GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		public bool HasItem(string item) => Treasures.List("Main", "List").Contains(item.ToUpper());
		public bool HasFoe(string foe)=>Foes.List("Main","List").Contains(foe.ToUpper());

		public void HasItem(string item, bool value) {
			if (value && (!HasItem(item)))
				Treasures.ListAdd("Main", "List", item.ToUpper());
			else if (!value)
				Treasures.ListRemove("Main", "List", item.ToUpper());
			//Treasures["Main", "ListUpdate"] = $"{DateTime.Now}";
		}

		static public KthuraData Current { private set; get; } = null;

		public void KillBehavior(byte[] bh=null) {
			void K(string k) { if (Unknown.ContainsKey(k)) Unknown.Remove(k); }
			K("Behavior");
			K("BEHAVIOR");
			if (bh != null) Unknown["Behavior"] = bh;
		}
			

		static KthuraData() {
			JCR6_zlib.Init();
			JCR6_lzma.Init();
			Dirry.InitAltDrives();
			Debug.WriteLine($"Reading {_IAADir}");
			ItemFiles = FileList.GetDir(IAADir);
			Debug.WriteLine($"Reading {_FoeDir}");
			FoeFiles = FileList.GetTree(FoeDir);
		}

		KthuraData(string f) {
			pathlessfile = f;
			Debug.WriteLine($"Loading Kthura Map: {file}");
			Current = this;
			Door = new CDoor(this);
			Load();
		}

		public override string ToString() => $"KthuraData: {pathlessfile}";

		void Load() {
			GC.Collect(); // I must be sure all old data has been saved and disposed or data loss can occur due to conflicts.
			var J = JCR6.Dir(file);
			TheMap = Kthura.Load(J);
			if (File.Exists($"{Config.ScriptDir}{pathlessfile}.aqs"))
				Behavior.SourceBox.Text = QuickStream.LoadString($"{Config.ScriptDir}{pathlessfile}.aqs").Trim();
			else
				Behavior.SourceBox.Text = $"; Behavior Apollo Quick Script for '{file}'\n; Created {DateTime.Now}\n\nInclude Basis.aqs\n\nChunk Load\n\tcall CSay,\"Welcome to {pathlessfile}\"\n\n";
			if (!File.Exists($"{Config.ScriptDir}Basis.aqs")) {
				Debug.WriteLine("Creating Basis.aqs");
				Directory.CreateDirectory(Config.ScriptDir);
				QuickStream.SaveString($"{Config.ScriptDir}Basis.aqs", Behavior.Basis);
			}
			foreach (var E in J.Entries.Keys) {
				try {
					switch (E) {
						case "DATA":
						case "OBJECTS":
							break; // Kthura can handle this by itself 
						case "LAYERS":
							Layers.FromBytes(J.JCR_B("LAYERS"));
							break;
						case "TREASURE":
							//Treasures.FromBytes(J.JCR_B("TREASURE"));
							Confirm.Annoy("Treasure data was in bytecode, but that is a bit buggy, so I had to destroy this data!");
							break;
						case "TREASURES":
							Treasures = GINIE.FromSource(J.JCR_B("TREASURES"));
							break;
						case "FOES":
							//Treasures.FromBytes(J.JCR_B("FOES"));
							//Confirm.Annoy("Foes data was in bytecode, but that is a bit buggy, so I had to destroy this data!");
							//break;
							// May fall through. Old code no longer important!
						case "RENCFOES": // Part of a fix 						
							Foes = GINIE.FromSource(J.JCR_B(E));
							ScanBosses();
							break;
						case "DOORS":
							//Door.Data.Auto(J.JCR_B("DOORS"));
							Door.LoadData(J);
							break;
						/*case "BEHAVIOR":
							Debug.WriteLine("Behavior data present, but not needed for the editor!");
							break;
						//*/
						default:
							Debug.WriteLine($"Unknown data: {E}");
							Unknown[J.Entries[E].Entry] = J.JCR_B(E);
							break;
					}
				} catch (Exception e) {
#if DEBUG
					Confirm.Annoy($"Error on the way!\n\n{e.Message}\n\n{e.StackTrace}", "ERROR", MessageBoxIcon.Error);
#else
					Confirm.Annoy($"Error on the way!\n\n{e.Message}", "ERROR", MessageBoxIcon.Error);
#endif
				}
			}
			Meta.Load();
			MainWindow.MySelf.UpdateItems();
			MainWindow.MySelf.UpdateFoes();
		}

		public void Save(bool compilebox=false) {
			var J = new TJCRCreate(file);
			KthuraSave.Save(TheMap, J, "", "zlib", Author, Notes);
			void SaveGINIE(GINIE G, string Entry) => J.AddBytes(G.ToBytes(), Entry, "zlib", Author, Notes);
			void SaveGINIES(GINIE g, string Entry) => J.AddString(g.ToSource(), Entry, "zlib", Author, Notes);
			SaveGINIE(Layers, "Layers");
			SaveGINIES(Foes, "Foes");
			SaveGINIES(Treasures, "Treasures");
			SaveGINIES(Door.Data, "Doors");
			/*if (compilebox)*/ //Behavior.Compile(this, J, $"{Config.ScriptDir}{pathlessfile}.aqs", compilebox);
			foreach (var k in Unknown) J.AddBytes(k.Value, k.Key, "zlib", Author, Notes);
			J.Close();
		}

		public struct rect {
			public long sx;
			public long sy;
			public long ex;
			public long ey;
			public rect(int fuckyou) { sx = 0; sy = 0; ex = 0; ey = 0; }

		}
		public rect ScanScrollBoundaries() {
			var ret = new rect(0);
			var l = SS2MapData.Layers.Selected;
			var Lay = TheMap.Layers[l];
			var forcedalready = false;
			foreach(var o in Lay.Objects) {
				switch (o.Tag.ToUpper()) {
					case "SCROLLMIN":
					case "SCROLLMINI":
						//ret.sx = Math.Max(ret.sx, o.x);
						//ret.sy = Math.Max(ret.sy, o.y);
						if (ret.sx!=0 || ret.sy!=0) if (forcedalready) Confirm.Annoy("Duplicate min scroll", "WARNING!", MessageBoxIcon.Warning);
						ret.sx = o.x;
						ret.sy = o.y;
						break;
					case "SCROLLMAX":
					case "SCROLLMAXI": {
							if (forcedalready) Confirm.Annoy("Duplicate max scroll","WARNING!",MessageBoxIcon.Warning);
							forcedalready = true;
							ret.ex = o.x;
							ret.ey = o.y;
						}
						break;
					default: {
							switch (o.kind) {
								case "Rect":
								case "TiledArea":
								case "StretchedArea":
								case "Zone":
									if (!forcedalready) {
										ret.ex = Math.Max(ret.ex,o.x + o.w);
										ret.ey = Math.Max(ret.ey,o.y + o.h);
									}
									break;
								default:
									ret.ex = Math.Max(ret.ex, o.x );
									ret.ey = Math.Max(ret.ey, o.y );
									break;
							}
						}
						break;						
				}
			}
			return ret;
		}

		~KthuraData() {
			Debug.WriteLine($"Saving Kthura Map: {file}");
			Save();
			Debug.WriteLine($"Disposing: {pathlessfile}");
		}

		static public void Switch(string f) {			
			if (f != "") Current = new KthuraData(f); else Current = null;
		}

	}
}