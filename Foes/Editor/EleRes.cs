// Lic:
// Foe Editor
// EleRes
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrickyUnits;

namespace StarStory2_Foe_Editor {
	class EleRes {
		static Dictionary<RadioButton, EleRes> RadioRegist = new Dictionary<RadioButton, EleRes>();

		readonly RadioButton[] Array;
		string Element;
		byte index;
		RadioButton Radio => Array[index];

		private EleRes(string E,byte i,RadioButton[] A) {
			Element = E;
			index = i;
			Array = A;
		}

		public override string ToString() {
			return $"<EleRes:{base.ToString()}>: {Element}:{index}";
		}

		public static void Register(string Element, RadioButton Fatal, RadioButton UltraWeak, RadioButton Weak, RadioButton Nothing, RadioButton Resist, RadioButton Immune, RadioButton Absorb) {
			var _Array = new RadioButton[] { Fatal, UltraWeak, Weak, Nothing, Resist, Immune, Absorb };
			for(byte i = 0; i < 7; ++i) {
				RadioRegist[_Array[i]] = new EleRes(Element, i, _Array);
				Debug.WriteLine($"Registered button #{i} for {Element}");
			}
		}

		public static void Update(string f) {
			var Foe = Data.GetRec(f);
			foreach (var A in RadioRegist.Values) {
				if (Foe.Data["ResistElement", A.Element] == "") {
					switch (A.Element) {
						case "Healing":
							Foe.Data["ResistElement", A.Element] = "6";
							break;
						case "DarkHealing":
							Foe.Data["ResistElement", A.Element] = "0";
							break;
						default:
							Foe.Data["ResistElement", A.Element] = "3";
							break;
					}
				}
				if (qstr.ToInt(Foe.Data["ResistElement", A.Element]) == A.index) A.Radio.IsChecked = true;
			}
		}

		public static void SetRes(string f,RadioButton R) {
			try {
				var Foe = Data.GetRec(f);
				var ER = RadioRegist[R];
				Foe.Data["ResistElement", ER.Element] = ER.index.ToString();
			} catch (Exception E) {
				Debug.WriteLine($"SetRes(\"{f}\",<{R}>): Something went wrong: {E.Message}\n{E.StackTrace} ");
				int c = 0;
				foreach (var IE in RadioRegist) Debug.WriteLine($"{++c}:{IE.Key}>{IE.Value}");
				Debug.WriteLine($"Gadgets registered {c}");
			}
		}
		
	}
}