// Lic:
// **********************************************
// 
// Dev/Foes/Editor/Data.cs
// (c) Jeroen Broks, 2022, All Rights Reserved.
// 
// This file contains material that is related
// to a storyline that is which is strictly
// copyrighted to Jeroen Broks.
// 
// This file may only be used in an unmodified
// form with an unmodified version of the
// software this file belongs to.
// 
// You may use this file for your study to see
// how I solved certain things in the creation
// of this project to see if you find valuable
// leads for the creation of your own.
// 
// Mostly this file comes along with a project
// that is for most part released under an
// open source license and that means that if
// you use that code with this file removed
// from it, you can use it under that license.
// Please check out the other files to find out
// which license applies.
// This file comes 'as-is' and in no possible
// way the author can be held responsible of
// any form of damages that may occur due to
// the usage of this file
// 
// 
// **********************************************
// 
// version: 22.03.01
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
	static class Data {


		#region Base Record
		public struct SS2FRec {
			public readonly GINIE Data;
			public bool Modified;
			internal SS2FRec(string f) {
				var mf = $"{Dir}/{f}";
				Data = GINIE.FromFile(mf);
				Data.AutoSaveSource = mf;
				Modified = false;
			}
		}
		static SortedDictionary<string, SS2FRec> Records = new SortedDictionary<string, SS2FRec>();

		static SS2FRec GetRec(string f) {
			string uf = f.ToUpper();
			if (!Records.ContainsKey(uf)) {
				Debug.WriteLine($"Getting data for not yet loaded record: {uf}!");
				Records[uf] = new SS2FRec(f);
			}
			return Records[uf];
		}
		#endregion

		struct RegRec {
			readonly public string Category;
			readonly public string VarName;
			internal RegRec(string VN) { Category = "Meta"; VarName = VN; }
			internal RegRec(string CT, string VN) { Category = CT; VarName = VN; }
		}
		static Dictionary<TextBox, RegRec> AutoTexBox = new Dictionary<TextBox, RegRec>();
		static public void RegTextBox(TextBox TB,string VN) { AutoTexBox[TB] = new RegRec(VN); }
		static public void RegTextBox(TextBox TB, string CT, string VN) { AutoTexBox[TB] = new RegRec(CT,VN); }

		static bool DontChange = false;
		static public void ActTextBox(TextBox TB) {
			try {
				if (Foe == "" || DontChange) return;
				var T = AutoTexBox[TB];
				var R = GetRec(Foe);
				R.Data[T.Category, T.VarName] = TB.Text;
			} catch(Exception e) {
				Confirm.Annoy($"An error occurred!\n\n{e.Message}\n");
			}
		}

		static public void UpdateTextBoxes() {
			DontChange = true;
			foreach(var IT in AutoTexBox) {
				IT.Key.IsEnabled = Foe != "";
				if (Foe != "") {
					var R = GetRec(Foe);
					IT.Key.Text = R.Data[IT.Value.Category, IT.Value.VarName];
				}
			}
		}

		static ListBox FoeList;
		static public string Foe {
			get {
				if (FoeList.SelectedItem == null) return "";
				return FoeList.SelectedItem.ToString();
			}
		}


		static Data() {
			Debug.WriteLine("Initizing 'Data' class");
			Dirry.InitAltDrives();
		}

		static public string Dir => Dirry.AD("Scyndi:Projects/Applications/Apollo/Games/Star Story 2/Dev/Foes/Data");
		static public void RegFoeList(ListBox FL) { FoeList = FL; }


	}
}