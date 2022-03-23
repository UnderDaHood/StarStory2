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
// Version: 22.03.22
// EndLic

using NSKthura;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TrickyUnits;
using UseJCR6;

namespace SS2MapData {
	class KthuraData {
		const string Author = "Jeroen P. Broks";
		const string Notes = "Part of the Star Story universe or the Phantasar universe. Copyrighted and owned by Jeroen P. Broks. This file may only be distributed with an official UNMODIFIED version of the game and not be extracted from the game";
		readonly string pathlessfile;
		public string file => $"{Config.KthuraDir}{pathlessfile}";
		public Kthura TheMap { private set; get; } = null;
		Dictionary<string, byte[]> Unknown = new Dictionary<string, byte[]>();
		public GINIE Layers = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		public GINIE Foes = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		public GINIE Treasures = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");
		//GINIE Layers => SS2MapData.Layers.Data; //GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor={Author}\n");

		static public KthuraData Current { private set; get; } = null;


		static KthuraData() {
			JCR6_zlib.Init();
			JCR6_lzma.Init();
		}

		KthuraData(string f) {
			pathlessfile = f;
			Debug.WriteLine($"Loading Kthura Map: {file}");
			Current = this;
			Load();
		}

		void Load() {
			GC.Collect(); // I must be sure all old data has been saved and disposed or data loss can occur due to conflicts.
			var J = JCR6.Dir(file);
			TheMap = Kthura.Load(J);
			foreach(var E in J.Entries.Keys) {
				switch (E) {
					case "DATA":
					case "OBJECTS":
						break; // Kthura can handle this by itself 
					case "LAYERS":
						Layers.FromBytes(J.JCR_B("LAYERS"));
						break;
					case "TREASURE":
						Treasures.FromBytes(J.JCR_B("TREASURE"));
						break;
					case "FOES":
						Treasures.FromBytes(J.JCR_B("FOES"));
						break;
					default:
						Debug.WriteLine($"Unknown data: {E}");
						Unknown[J.Entries[E].Entry] = J.JCR_B(E);
						break;
				}
			}
			Meta.Load();
		}

		void Save() {
			var J = new TJCRCreate(file);
			KthuraSave.Save(TheMap,J,"","zlib",Author,Notes);
			void SaveGINIE(GINIE G, string Entry) => J.AddBytes(G.ToBytes(), Entry, "zlib", Author, Notes);
			SaveGINIE(Layers, "Layers");
			SaveGINIE(Foes, "Foes");
			SaveGINIE(Treasures, "Treasure");
			foreach (var k in Unknown) J.AddBytes(k.Value, k.Key, "zlib", Author, Notes);
			J.Close();
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