// Lic:
// Star Story II - Foe Editor
// Main Window linkups (XAML)
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
// Version: 22.05.03
// EndLic

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrickyUnits;

namespace StarStory2_Foe_Editor {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			IAA.AI_Do = AblDo;
			IAA.AI_Dont = AblDont;
			IAA.Item_Do = ItmDo;
			IAA.Item_Dont = ItmDont;
			GetFoeList();
			Data.RegFoeList(FoeList);
			MainTab.Visibility = Visibility.Hidden;
			Data.RegTextBox(TextFoeName, "Name");
			Data.RegTextBox(TextFoeDescription, "Description");
			Data.RegTextBox(TextVocalTag, "VocalTag");
			Data.RegTextBox(TextImage, "NormSprite");
			Data.RegTextBox(TextImageNeg, "NegaSprite");
			Data.RegCheckBox(ChkIsBoss, "Boss");
			Data.RegCheckBox(Humanoid, "Humanoid");
			new Stats("Power", S_MinPower, S_MaxPower, S_Sk1Power, S_Sk2Power, S_Sk3Power);
			new Stats("Defense", S_MinDefense, S_MaxDefense, S_Sk1Defense, S_Sk2Defense, S_Sk3Defense);
			new Stats("Will", S_MinWill, S_MaxWill, S_Sk1Will, S_Sk2Will, S_Sk3Will);
			new Stats("Resistance", S_MinResistance, S_MaxResistance, S_Sk1Resistance, S_Sk2Resistance, S_Sk3Resistance);
			new Stats("Accuracy", S_MinAccuracy, S_MaxAccuracy, S_Sk1Accuracy, S_Sk2Accuracy, S_Sk3Accuracy);
			new Stats("Evasion", S_MinEvasion, S_MaxEvasion, S_Sk1Evasion, S_Sk2Evasion, S_Sk3Evasion);
			new Stats("Speed", S_MinSpeed, S_MaxSpeed, S_Sk1Speed, S_Sk2Speed, S_Sk3Speed);
			new Stats("HP", S_MinHP, S_MaxHP, S_Sk1HP, S_Sk2HP, S_Sk3HP);
			new Stats("Level", S_MinLevel, S_MaxLevel, S_Sk1Level, S_Sk2Level, S_Sk3Level);
			Data.RegTextBox(S_Sk1Luck, "Luck", "SK1");
			Data.RegTextBox(S_Sk2Luck, "Luck", "SK2");
			Data.RegTextBox(S_Sk3Luck, "Luck", "SK3");

			Data.RegTextBox(AIScript, "AI", "Script");
			IAA.AIReg(AblMinLevel1, "MinLevel1");
			IAA.AIReg(AblMinLevel2, "MinLevel2");
			IAA.AIReg(AblMinLevel3, "MinLevel3");
			IAA.AIReg(AblRate1, "Rate1");
			IAA.AIReg(AblRate2, "Rate2");
			IAA.AIReg(AblRate3, "Rate3");
			IAA.AIReg(AblTarget1, "Target1");
			IAA.AIReg(AblTarget2, "Target2");
			IAA.AIReg(AblTarget3, "Target3");

			Data.RegTextBox(ResStatusBio, "ResistStatus", "Bio");
			Data.RegTextBox(ResStatusWill, "ResistStatus", "Will");
			Data.RegTextBox(ResStatusStamina, "ResistStatus", "Stamina");

			EleRes.Register("Flame", Flame0, Flame1, Flame2, Flame3, Flame4, Flame5, Flame6);
			EleRes.Register("Wind", Wind0, Wind1, Wind2, Wind3, Wind4, Wind5, Wind6);
			EleRes.Register("Water", Water0, Water1, Water2, Water3, Water4, Water5, Water6);
			EleRes.Register("Earth", Earth0, Earth1, Earth2, Earth3, Earth4, Earth5, Earth6);

			EleRes.Register("Frost", Frost0, Frost1, Frost2, Frost3, Frost4, Frost5, Frost6);
			EleRes.Register("Thunder", Thunder0, Thunder1, Thunder2, Thunder3, Thunder4, Thunder5, Thunder6);
			EleRes.Register("Light", Light0, Light1, Light2, Light3, Light4, Light5, Light6);
			EleRes.Register("Darkness", Darkness0, Darkness1, Darkness2, Darkness3, Darkness4, Darkness5, Darkness6);

			EleRes.Register("Healing", Healing0, Healing1, Healing2, Healing3, Healing4, Healing5, Healing6);
			EleRes.Register("DarkHealing", DarkHealing0, DarkHealing1, DarkHealing2, DarkHealing3, DarkHealing4, DarkHealing5, DarkHealing6);

			
			IAA.ItemReg(ItemDropMinLevel1, "DropMinLevel1");
			IAA.ItemReg(ItemDropMinLevel2, "DropMinLevel2");
			IAA.ItemReg(ItemDropMinLevel3, "DropMinLevel3");
			IAA.ItemReg(ItemDropRate1, "DropRate1");
			IAA.ItemReg(ItemDropRate2, "DropRate2");
			IAA.ItemReg(ItemDropRate3, "DropRate3");
			IAA.ItemReg(ItemStealMinLevel1, "StealMinLevel1");
			IAA.ItemReg(ItemStealMinLevel2, "StealMinLevel2");
			IAA.ItemReg(ItemStealMinLevel3, "StealMinLevel3");
			IAA.ItemReg(ItemStealRate1, "StealRate1");
			IAA.ItemReg(ItemStealRate2, "StealRate2");
			IAA.ItemReg(ItemStealRate3, "StealRate3");


		}

		void GetFoeList() {
			Debug.WriteLine("Renewing Foe List");
			FoeList.Items.Clear();
			var L = FileList.GetTree(Data.Dir);
			foreach (var f in L) {
				if (f.ToUpper() != "README.MD") FoeList.Items.Add(f);
			}
			Data.UpdateTextBoxes();
			MainTab.Visibility = Visibility.Hidden;
		}

		private void NewFoeFileName_TextChanged(object sender, TextChangedEventArgs e) {
			var txt = NewFoeFileName.Text;
			NewFoe.IsEnabled = txt != "";
		}

		private void RegTexBoxChange(object sender, TextChangedEventArgs e) => Data.ActTextBox((TextBox)sender);

		private void NewFoe_Click(object sender, RoutedEventArgs e) {
			var txt = NewFoeFileName.Text;
			var fname = $"{Data.Dir}/{txt}";
			var fdir = qstr.ExtractDir(fname);
			if (txt == "") return;
			if (qstr.ExtractDir(txt) == "") {
				Confirm.Annoy("No directory!");
				return;
			}
			if (File.Exists(fname)) {
				Confirm.Annoy($"File {fname} already exists!");
				return;
			}
			if (!Directory.Exists(fdir)) {
				if (Confirm.Yes($"Do you want me to create the directory '{fdir}' for '{txt}'?"))
					Directory.CreateDirectory(fdir);
				else
					return;
			}
			NewFoeFileName.Text = "";
			QuickStream.SaveString(fname, $"[alg]\nCreationTime={DateTime.Now}\nCreationFile={fname}\n[meta]\nName={qstr.StripDir(txt)}\nVocalTag={txt}\nNormSprite=GFX/Combat/Foes/{txt}.png\nNegaSprite=GFX/Combat/Foes/{txt}.Negative.png\n[luck]\nsk1=Random\nsk2=Random\nsk3=Random\n[ai]\nscript=Default\n");
			GetFoeList();
		}

		private void FoeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (FoeList.SelectedItem == null) {
				MainTab.Visibility = Visibility.Hidden;
				return;
			}
			MainTab.Visibility = Visibility.Visible;
			TextFileName.Text = Data.Foe;
			Data.UpdateTextBoxes();
			IAA.UpdateAIBoxes(Data.Foe);
			EleRes.Update(Data.Foe);
		}

		private void AddAbl_Click(object sender, RoutedEventArgs e) {
			var SelItem = AblDont.SelectedItem;
			if (SelItem != null) {
				Debug.WriteLine("Add action");
				Debug.WriteLine($"= {SelItem}");
				var D = Data.GetRec(Data.Foe);
				Debug.WriteLine("= Adding to action list");
				D.Data.ListAdd("AI", "Actions", SelItem.ToString());
				Debug.WriteLine("= Updating listboxes");
				IAA.UpdateAIBoxes(Data.Foe);
				Debug.WriteLine("= Done");
			}
		}

		private void RemAbl_Click(object sender, RoutedEventArgs e) {
			var SelItem = AblDo.SelectedItem;
			if (SelItem != null) {
				var D = Data.GetRec(Data.Foe);
				D.Data.List("AI", "ACTIONS").Remove($"{SelItem}");
			}
			IAA.UpdateAIBoxes(Data.Foe);
		}

		private void AblDo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			IAA.AIAutoEnable();
			var Lst = (ListBox)sender;
			var SelItem = Lst.SelectedItem;
			//var SelItemS = $"{SelItem}";
			var D = Data.GetRec(Data.Foe);
			if (SelItem != null) {
				foreach(var v in IAA.AIFields.Values) {
					if (Lst == v.DoList) {
						var Cat = $"{v.Category}:{SelItem}";
						if (D.Data[Cat, v.Field] == "") {
							D.Data[Cat, v.Field] = "1";
							if (qstr.Prefixed(v.Field.ToLower(), "target")) D.Data[Cat, v.Field] = "Random";
							Debug.WriteLine($"Data[\"{Cat}\",\"{v.Field}\"] set to default value> {D.Data[Cat, v.Field]}");
						}
						v.TB.Text = D.Data[Cat, v.Field];
					}
				}
			}
		}

		private void AITexBoxChange(object sender, TextChangedEventArgs e) {
			try {
				var DoList = IAA.AIFields[(TextBox)sender];
				var SelItem = DoList.DoList.SelectedItem; if (SelItem == null) return;
				var TB = (TextBox)sender;
				var D = Data.GetRec(Data.Foe);
				var Fld = IAA.AIFields[TB];
				var Cat = $"{Fld.Category}:{SelItem}";
				D.Data[Cat, Fld.Field] = TB.Text;
			} catch (Exception ex) {
				Debug.WriteLine($"AITextBoxChange(<{sender}>,<{e}>)Something went wrong: {ex.Message}");
			}
		}

		private void Ele_Checked(object sender, RoutedEventArgs e) {
			EleRes.SetRes(Data.Foe, (RadioButton)sender);
		}

		private void AddItem(object sender, RoutedEventArgs e) {
			var SelItem = ItmDont.SelectedItem;
			if (SelItem != null) {
				Debug.WriteLine("Add action");
				Debug.WriteLine($"= {SelItem}");
				var D = Data.GetRec(Data.Foe);
				Debug.WriteLine("= Adding to action list");
				D.Data.ListAdd("Items", "List", SelItem.ToString());
				Debug.WriteLine("= Updating listboxes");
				IAA.UpdateAIBoxes(Data.Foe);
				Debug.WriteLine("= Done");
			}
		}

		private void RemoveItem(object sender, RoutedEventArgs e) {
			var SelItem = ItmDo.SelectedItem;
			if (SelItem != null) {
				var D = Data.GetRec(Data.Foe);
				D.Data.List("ITEMS", "LIST").Remove($"{SelItem}");
			}
			IAA.UpdateAIBoxes(Data.Foe);
		}

		private void ItemDo_SelectionChanged(object sender, SelectionChangedEventArgs e) => AblDo_SelectionChanged(sender, e);

		private void RegChkChange(object sender, RoutedEventArgs e) => Data.ActCheckBox((CheckBox)sender);
	}
}