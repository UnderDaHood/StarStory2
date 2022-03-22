// Lic:
// Map Extra Data Editor
// Configuration
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrickyUnits;

namespace SS2MapData {
	class Config {
		const string rawKthura = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Story/Kthura/";
		static public string KthuraDir => Dirry.AD(rawKthura);

		static Config() {
			Debug.WriteLine("Initiating Dirry Alt Drives!"); Dirry.InitAltDrives(); 
		}

		static public string[] KthuraMaps => FileList.GetDir(KthuraDir);

		static public ListBox MapList = null;

		static public string SelectedMap {
			get {
				//Debug.WriteLine($"MapList {MapList}");
				if (MapList == null) return "";
				//Debug.WriteLine($"MapList {MapList} / {MapList.SelectedItem}");
				if (MapList.SelectedItem == null) return "";
				// Debug.WriteLine($"{MapList.SelectedItem.ToString()}");
				return MapList.SelectedItem.ToString();
			}
		}
	}
}