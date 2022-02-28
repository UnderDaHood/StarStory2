using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickyUnits;

namespace StarStory2_Foe_Editor {
	static class Data {

		static Data() {
			Debug.WriteLine("Initizing 'Data' class");
			Dirry.InitAltDrives();
		}

		static public string Dir => Dirry.AD("Scyndi:Projects/Applications/Apollo/Games/The Fairy Tale Revamped");
	}
}
