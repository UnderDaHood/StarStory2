// Lic:
// Foe Editor
// Items Actions Abilities
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
	class IAA {

		public static ListBox AI_Do = null;
		public static ListBox AI_Dont = null;
		public static ListBox Item_Do = null;
		public static ListBox Item_Dont = null;

		public const string _IAADir = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Script/Data/IAA/";
		public static string IAADir => Dirry.AD(_IAADir);

		public static SortedDictionary<string, IAA> Register { get; private set; } = new SortedDictionary<string, IAA>();

		public class AField {
			internal string Category;
			internal string Field;
			internal ListBox DoList;
			internal TextBox TB;
        }
		readonly static public Dictionary<TextBox, AField> AIFields = new Dictionary<TextBox, AField>();

		static public void AIReg(TextBox TB, string Field) => AIReg(TB, "Action", Field, AI_Do);
		static public void ItemReg(TextBox TB, string Field) => AIReg(TB, "Item", Field, Item_Do);
		static public void AIReg(TextBox TB, string Category,string Field, ListBox DoList) {
				var AF = new AField();
			AF.Field = Field;
			AF.TB = TB;
			AF.Category = Category;
			AF.DoList = DoList;
			AIFields[TB] = AF;
        }

		static public void AIAutoEnable() {
			var sel = new Dictionary<ListBox, bool>();
			sel[AI_Do] = AI_Do.SelectedItem != null;
			sel[Item_Do] = Item_Do.SelectedItem != null;

			foreach (var T in AIFields)
				//if (sel)
				//	T.Visibility = System.Windows.Visibility.Visible;
				//else
				//	T.Visibility = System.Windows.Visibility.Hidden;
				T.Key.IsEnabled = sel[T.Value.DoList];
        }

		public static void UpdateAIBoxes(string f) {
			var R = Data.GetRec(f);
			AI_Do.Items.Clear();
			AI_Dont.Items.Clear();
			Item_Do.Items.Clear();
			Item_Dont.Items.Clear();
			foreach(var k in Register) {
				if (R.Data.List("AI", "Actions").Contains(k.Value.ListAs))
					AI_Do.Items.Add(k.Value.ListAs);
				else
					AI_Dont.Items.Add(k.Value.ListAs);
				if (qstr.Prefixed(k.Value.ListAs, "ITM_") || qstr.Prefixed(k.Value.ListAs, "JWL")) {
					if (R.Data.List("Items", "List").Contains(k.Value.ListAs))
						Item_Do.Items.Add(k.Value.ListAs);
					else
						Item_Dont.Items.Add(k.Value.ListAs);
				}
			}
			AIAutoEnable();
		}

		static IAA() {
			Dirry.InitAltDrives();
			Debug.WriteLine($"Reading: {_IAADir}");
			foreach (var s in FileList.GetDir(IAADir)) 
				if (qstr.StripAll(s).ToUpper()!= ("MyData_ClassFile_IAA").ToUpper())
					Register[s] = new IAA(s);
		}


		readonly string gotname;
		public string File => $"{IAADir}gotname";
		public string ListAs => qstr.StripExt(gotname);

		public bool IsItem => qstr.Prefixed(gotname.ToUpper(), "ITM_") || qstr.Prefixed(gotname.ToUpper(), "JWL_");

		IAA(string f) { gotname = f; }
	}
}