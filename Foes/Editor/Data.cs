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
// version: 22.04.25
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

		public static SS2FRec GetRec(string f) {
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
		static Dictionary<CheckBox, RegRec> AutoChkBox = new Dictionary<CheckBox, RegRec>();
		static public void RegTextBox(TextBox TB,string VN) { AutoTexBox[TB] = new RegRec(VN); }
		static public void RegTextBox(TextBox TB, string CT, string VN) { AutoTexBox[TB] = new RegRec(CT,VN); }
		static public void RegCheckBox(CheckBox CB, string CT, string VN) { AutoChkBox[CB] = new RegRec(CT, VN); }
		static public void RegCheckBox(CheckBox CB, string VN) { AutoChkBox[CB] = new RegRec(VN); } 

		static bool DontChange = false;
		static public void ActTextBox(TextBox TB) {
			try {
				if (Foe == "" || DontChange) return;
				var T = AutoTexBox[TB];
				var R = GetRec(Foe);
				var txt = TB.Text.Replace("\r",""); // Only Unix based line endings.
				if (TB.AcceptsReturn) {
					for (byte i = 255; i > 0; i--) 
						if ((i<35 && i!=32) || i>122)
						txt = txt.Replace($"{(char)i}", $"<<<<{i}>>>>");
				}
				R.Data[T.Category, T.VarName] = txt;
				R.Modified = true;
			} catch(Exception e) {
				Confirm.Annoy($"An error occurred!\n\n{e.Message}\n\n\nFoe: {Foe}");
			}
		}

		static public void ActCheckBox(CheckBox TB)
		{
			try
			{
				if (Foe == "" || DontChange) return;
				var T = AutoChkBox[TB];
				var R = GetRec(Foe);
				//var txt = TB.Text.Replace("\r", ""); // Only Unix based line endings.
				R.Data[T.Category, T.VarName] = TB.IsChecked.ToString().ToUpper();
				R.Modified = true;
			}
			catch (Exception e)
			{
				Confirm.Annoy($"An error occurred!\n\n{e.Message}\n\n\nFoe: {Foe}");
			}
		}

		static public void UpdateTextBoxes() {
			DontChange = true;
			foreach(var IT in AutoTexBox) {
				IT.Key.IsEnabled = Foe != "";
				if (Foe != "") {
					var R = GetRec(Foe);
					var txt = R.Data[IT.Value.Category, IT.Value.VarName];
					if (IT.Key.AcceptsReturn) {
						for (byte i = 255; i > 0; i--) txt = txt.Replace($"<<<<{i}>>>>", $"{(char)i}");
					}
					IT.Key.Text = txt;
				}
			}

			foreach (var IT in AutoChkBox) {
				IT.Key.IsEnabled = Foe != "";
				if (Foe != "") 	{
					var R = GetRec(Foe);
					var txt = R.Data[IT.Value.Category, IT.Value.VarName];
					IT.Key.IsChecked = txt.Trim().ToUpper() == "TRUE";
				}
			}

			/*
			foreach(var IT in Stats.Register) {
				var R = GetRec(Foe);
				IT.Key.Text = $"{R.Data[$"STAT.{IT.Value.k}", $"{IT.Value.s.Stat}"]}";
			}
			*/
			DontChange = false;
		}


		static ListBox FoeList;
		static public string Foe {
			get {
				if (FoeList==null || FoeList.SelectedItem == null) return "";
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