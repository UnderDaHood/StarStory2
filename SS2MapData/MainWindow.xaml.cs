// Lic:
// Map Extra Data Editor
// Main Windows and gadget scriptout
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
// Version: 22.06.10
// EndLic

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SS2MapData {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public static MainWindow MySelf { get; private set; }

		public Visibility MyVis(bool k) { if (k) return Visibility.Visible; else return Visibility.Hidden; }

		void AutoItemShow() {
			ItemSettingsGrid.Visibility = MyVis(ItemYes.SelectedItem != null);
			FoeSettingsGrid.Visibility = MyVis(FoeYes.SelectedItem != null);
		}

		TextBox[] BossFoe = new TextBox[10];
		TextBox[,] BossSkill = new TextBox[10, 4];
		#region Init
		public MainWindow() {
			MySelf = this;
			InitializeComponent();
			GridLayer.Visibility = Visibility.Hidden;
			GetMaps();
			TabVisible();
			Config.MapList = ListMaps;
			Meta.Register(BoxTitle, "Title");
			Meta.Register(BoxMapLevel, "MapLevel", true);
			Meta.Register(BoxArena, "Arena");
			Meta.Register(BoxMusic, "Music");
			Meta.Register(ChkBoxApolloQuickScript, "UseAQS", true);
			Meta.Register(ChkBoxNeilMapScript, "UseNeil");
			Meta.Register(ChkBoxUpdate, "CallbackUpdate");
			Meta.Register(ChkBoxPreDraw, "CallbackPreDraw");
			Meta.Register(ChkBoxPostDraw, "CallbackPostDraw");
			Meta.Register(AltCombatMusic, "AltCombatMusic");
			Layers.MW = this;
			Layers.Register(TxtLayTitle, "Title");
			Layers.Register(CheckNoEncounter, "NoEncounters", Layers.Type.Checkbox);
			Layers.Register(TxtLayAltArena, "AltArena");
			Layers.Register(Txt_ScrollMinX, "Scroll_MinX");
			Layers.Register(Txt_ScrollMinY, "Scroll_MinY");
			Layers.Register(Txt_ScrollMaxX, "Scroll_MaxX");
			Layers.Register(Txt_ScrollMaxY, "Scroll_MaxY");
			Layers.Register(Txt_ScrollCenX, "Scroll_CenX");
			Layers.Register(Txt_ScrollCenY, "Scroll_CenY");
			Behavior.SourceBox = Behavior_Source;
			CDoor.Lijst = ListBox_Doors;
			//CDoor.Register(Door_Tag, "!Tag");
			CDoor.Register(Door_Collection, "Collection");
			CDoor.Register(Door_MoveX, "MoveX");
			CDoor.Register(Door_MoveY, "MoveY");
			CDoor.Register(Door_Frames, "Frames");
			CDoor.Register(Door_Open, "Open");
			CDoor.Register(Door_AudioOpen, "AudioOpen");
			CDoor.Register(Door_AudioClose, "AudioClose");
			DoorGrid();

			// Item linkups
			Meta.Register(MinCycle1, "ITEM:MinCycle1");
			Meta.Register(MinCycle2, "ITEM:MinCycle2");
			Meta.Register(MinCycle3, "ITEM:MinCycle3");
			Meta.Register(TreasureRate1, "ITEM:Rate1");
			Meta.Register(TreasureRate2, "ITEM:Rate2");
			Meta.Register(TreasureRate3, "ITEM:Rate3");

			// Foes
			Meta.Register(FoeMinCycle1, "FOE:MinCycle1");
			Meta.Register(FoeMinCycle2, "FOE:MinCycle2");
			Meta.Register(FoeMinCycle3, "FOE:MinCycle3");
			Meta.Register(FoeOMinLevel1, "FOE:MinLevel1");
			Meta.Register(FoeOMinLevel2, "FOE:Minlevel2");
			Meta.Register(FoeOMinLevel3, "FOE:Minlevel3");
			Meta.Register(FoeOMaxLevel1, "FOE:MaxLevel1");
			Meta.Register(FoeOMaxLevel2, "FOE:Maxlevel2");
			Meta.Register(FoeOMaxLevel3, "FOE:Maxlevel3");
			Meta.Register(FoeRate1, "FOE:Rate1");
			Meta.Register(FoeRate2, "FOE:Rate2");
			Meta.Register(FoeRate3, "FOE:Rate3");

			Meta.Register(FoeAMinLevel1, "Foe_MinLevel1");
			Meta.Register(FoeAMinLevel2, "Foe_Minlevel2");
			Meta.Register(FoeAMinLevel3, "Foe_Minlevel3");
			Meta.Register(FoeAMaxLevel1, "Foe_MaxLevel1");
			Meta.Register(FoeAMaxLevel2, "Foe_Maxlevel2");
			Meta.Register(FoeAMaxLevel3, "Foe_Maxlevel3");
			FoeSettingsGrid.Visibility = MyVis(false);

			// Boss
			Grid_Boss.Visibility = Visibility.Hidden;
			KthuraData.GadgetBossList = BossList;
			BossFoe[1] = BOSS1_Foe;
			BossSkill[1, 1] = BOSS1_Skill1;
			BossSkill[1, 2] = BOSS1_Skill2;
			BossSkill[1, 3] = BOSS1_Skill3;
			BossFoe[2] = BOSS2_Foe;
			BossSkill[2, 1] = BOSS2_Skill1;
			BossSkill[2, 2] = BOSS2_Skill2;
			BossSkill[2, 3] = BOSS2_Skill3;
			BossFoe[3] = BOSS3_Foe;
			BossSkill[3, 1] = BOSS3_Skill1;
			BossSkill[3, 2] = BOSS3_Skill2;
			BossSkill[3, 3] = BOSS3_Skill3;
			BossFoe[4] = BOSS4_Foe;
			BossSkill[4, 1] = BOSS4_Skill1;
			BossSkill[4, 2] = BOSS4_Skill2;
			BossSkill[4, 3] = BOSS4_Skill3;
			BossFoe[5] = BOSS5_Foe;
			BossSkill[5, 1] = BOSS5_Skill1;
			BossSkill[5, 2] = BOSS5_Skill2;
			BossSkill[5, 3] = BOSS5_Skill3;
			BossFoe[6] = BOSS6_Foe;
			BossSkill[6, 1] = BOSS6_Skill1;
			BossSkill[6, 2] = BOSS6_Skill2;
			BossSkill[6, 3] = BOSS6_Skill3;
			BossFoe[7] = BOSS7_Foe;
			BossSkill[7, 1] = BOSS7_Skill1;
			BossSkill[7, 2] = BOSS7_Skill2;
			BossSkill[7, 3] = BOSS7_Skill3;
			BossFoe[8] = BOSS8_Foe;
			BossSkill[8, 1] = BOSS8_Skill1;
			BossSkill[8, 2] = BOSS8_Skill2;
			BossSkill[8, 3] = BOSS8_Skill3;
			BossFoe[9] = BOSS9_Foe;
			BossSkill[9, 1] = BOSS9_Skill1;
			BossSkill[9, 2] = BOSS9_Skill2;
			BossSkill[9, 3] = BOSS9_Skill3;
			// There really should be an easier way to do this!

			Meta.Register(SeinSource, "Sein!");
		}
		#endregion

		void GetLayers() {
			ListLayers.Items.Clear();
			if (KthuraData.Current == null) return;
			foreach (var L in KthuraData.Current.TheMap.Layers.Keys) ListLayers.Items.Add(L);
		}


		void GetMaps() {
			Debug.WriteLine("Renewing MapList");
			ListMaps.Items.Clear();
			foreach (var f in Config.KthuraMaps) ListMaps.Items.Add(f);
		}

		void TabVisible() {
			if (ListMaps.SelectedItem == null)
				MainTab.Visibility = Visibility.Hidden;
			else
				MainTab.Visibility = Visibility.Visible;
		}

		private void ListMaps_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			TabVisible();
			BoxMap.Text = Config.SelectedMap;
			KthuraData.Switch(Config.SelectedMap);
			ListBox_Doors.Visibility = Visibility.Hidden;
			GridDoor.Visibility = Visibility.Hidden;
			GetLayers();
		}

		private void MetaChanged(object sender, TextChangedEventArgs e) => Meta.Update((TextBox)sender);

		public string SelectedLayer {
			get {
				if (ListLayers.SelectedItem == null) return "";
				return ListLayers.SelectedItem.ToString();
			}
		}

		private void ListLayers_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ListLayers.SelectedItem == null) {
				GridLayer.Visibility = Visibility.Hidden;
				return;
			}
			GridLayer.Visibility = Visibility.Visible;
			TxtLayer.Text = SelectedLayer;
			Layers.Receive();
		}

		private void LayUpdate(object sender, TextChangedEventArgs e) {
			Layers.Update(sender);
		}

		private void LayUpdate(object sender, RoutedEventArgs e) {
			Layers.Update(sender);
		}

		private void Behavior_Source_TextChanged(object sender, TextChangedEventArgs e) {
			//Behavior.source = Behavior_Source.Text;
			if (KthuraData.Current != null)
				KthuraData.Current.BehaviorSource = Behavior_Source.Text;
		}

		private void CompileAndSave_Click(object sender, RoutedEventArgs e) {
			KthuraData.Current.Save(true);
		}

		private void ScanScrollBoundaries_Click(object sender, RoutedEventArgs e) {
			var res = KthuraData.Current.ScanScrollBoundaries();
			var cx = res.sx + ((res.ex - res.sx) / 2);
			var cy = res.sy + ((res.ey - res.sy) / 2);
			Txt_ScrollMinX.Text = res.sx.ToString();
			Txt_ScrollMinY.Text = res.sy.ToString();
			Txt_ScrollMaxX.Text = res.ex.ToString();
			Txt_ScrollMaxY.Text = res.ey.ToString();
			Txt_ScrollCenX.Text = cx.ToString();
			Txt_ScrollCenY.Text = cy.ToString();
		}

		private void MetaCheckBox_Checked(object sender, RoutedEventArgs e) {
			Meta.Update((CheckBox)sender);
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			KthuraData.Current.Door.Scan();
			ListBox_Doors.Visibility = Visibility.Visible;
		}

		public void DoorGrid() {
			if (ListBox_Doors.SelectedItem != null) {
				GridDoor.Visibility = Visibility.Visible;
				var ID = qstr.Split(ListBox_Doors.SelectedItem.ToString(), "::");
				Door_Layer.Text = ID[0].Substring(7).Trim();
				Door_Tag.Text = ID[1];
				KthuraData.Current.Door.Sync(ListBox_Doors.SelectedItem.ToString());
			} else
				GridDoor.Visibility = Visibility.Hidden;
		}

		private void ListBox_Doors_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			DoorGrid();
		}

		private void Door_TB_TextChanged(object sender, TextChangedEventArgs e) {
			KthuraData.Current.Door.Update((TextBox)sender, ListBox_Doors.SelectedItem.ToString());
		}

		private void Doors_DestroyAll_Click(object sender, RoutedEventArgs e) {
			if (Confirm.Yes("This will destroy all existing doors data in order to scan anew.\nSome vital data can be destroyed in this process.\n\nAre you sure?")) {
				KthuraData.Current.Door.Data.Clear();
				Button_Click(sender, e);
			}
		}

		private void ItemYes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			Meta.Load();
			AutoItemShow();
		}

		public void UpdateItems() {
			ItemYes.Items.Clear();
			ItemNo.Items.Clear();
			foreach (var i in KthuraData.ItemFiles) {
				if (qstr.Prefixed(i, "ITM_") || qstr.Prefixed(i, "JWL_")) {
					var item = qstr.StripExt(i).Trim().ToUpper();
#if DEBUG
					Debug.WriteLine($"Do we have a {item}? {KthuraData.Current.HasItem(item)}");
#endif
					if (KthuraData.Current.HasItem(item))
						ItemYes.Items.Add(item);
					else
						ItemNo.Items.Add(item);
				}
			}
			AutoItemShow();
		}



		public void UpdateFoes() {
			FoeYes.Items.Clear();
			FoeNo.Items.Clear();
			foreach (var i in KthuraData.FoeFiles) {
				if (i.ToUpper() != "README.MD") {
					Debug.WriteLine($"Foe '{i}' available {KthuraData.Current.HasFoe(i)} in '{KthuraData.Current}'");
					if (KthuraData.Current.HasFoe(i))
						FoeYes.Items.Add(i);
					else
						FoeNo.Items.Add(i);
				}
			}
		}

		private void AddItem_Click(object sender, RoutedEventArgs e) {
			var SelItem = ItemNo.SelectedItem; if (SelItem == null) { Debug.WriteLine("Nothing to add"); return; }
			var Item = $"{SelItem}";
			Debug.WriteLine($"Adding item: {Item}");
			KthuraData.Current.HasItem(Item, true);
			UpdateItems();
		}

		private void RemItem_Click(object sender, RoutedEventArgs e) {
			var SelItem = ItemYes.SelectedItem; if (SelItem == null) return;
			var Item = $"{SelItem}";
			KthuraData.Current.HasItem(Item, false);
			UpdateItems();
		}

		public string EditItem {
			get {
				var SelItem = ItemYes.SelectedItem; if (SelItem == null) return "";
				return SelItem.ToString();
			}
		}

		public string EditFoe {
			get {
				var SelFoe = FoeYes.SelectedItem; if (SelFoe == null) return "";
				return SelFoe.ToString();
			}
		}

		Dictionary<bool, TextBox[]> LevelEnable;
		private void OwnLevelCheck(object sender = null, RoutedEventArgs e = null) {
			var SelItem = FoeYes.SelectedItem;

			if (LevelEnable == null) {
				LevelEnable = new Dictionary<bool, TextBox[]>();
				LevelEnable[true] = new TextBox[] { FoeOMaxLevel1, FoeOMaxLevel2, FoeOMaxLevel3, FoeOMinLevel1, FoeOMinLevel2, FoeOMinLevel3 };
				LevelEnable[false] = new TextBox[] { FoeAMaxLevel1, FoeAMaxLevel2, FoeAMaxLevel3, FoeAMinLevel1, FoeAMinLevel2, FoeAMinLevel3 };
			}
			var ch = FoeOwnlevel.IsChecked;
			foreach (var item in LevelEnable) {
				foreach (var texb in item.Value) {
					texb.IsEnabled = ch == item.Key;
				}
			}
			if (SelItem != null) {
				KthuraData.Current.Foes[$"Foe:{SelItem}", "UseOwnLevels"] = $"{ch}";
			}
		}

		private void AddFoe_Click(object sender, RoutedEventArgs e) {
			var SelItem = FoeNo.SelectedItem;
			if (SelItem != null) {
				KthuraData.Current.Foes.ListAddNew("Main", "List", $"{SelItem}".ToUpper());
			}
			UpdateFoes();
		}

		private void RemFoe_Click(object sender, RoutedEventArgs e) {
			var SelItem = FoeYes.SelectedItem;
			if (SelItem != null) {
				KthuraData.Current.Foes.ListRemove("Main", "List", $"{SelItem}".ToUpper());
			}
			UpdateFoes();
			AutoEnableFoes();
		}

		public void AutoEnableFoes() {
			var SelItem = FoeYes.SelectedItem;
			var Goed = SelItem != null;
			/*
			foreach (var tb in Meta.GetRegister.Values) {
				if (tb.Alt == Meta.MetaAlt.Foe) {
					tb.TextB.IsEnabled = Goed;
					tb.TextB.Text=KthuraData.Current.Foes[$"Foe:{SelItem}", tb.Field];
				}
			}
			*/
			FoeSettingsGrid.Visibility = MyVis(Goed);
		}

		private void BossList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var BS = KthuraData.Current.CurBoss;
			if (BS == null) { Grid_Boss.Visibility = Visibility.Hidden; return; }
			Grid_Boss.Visibility = Visibility.Visible;
			Boss_Arena.Text = BS.Arena;
			Boss_Tune.Text = BS.Music;
			for (int idx = 1; idx <= 9; ++idx) {
				BossFoe[idx].Text = BS[idx];
				for (int skill = 1; skill <= 3; ++skill) BossSkill[idx, skill].Text = $"{BS[idx, skill]}";
			}
		}

		private void ChangeArena(object sender, TextChangedEventArgs e) { KthuraData.Current.CurBoss.Arena = Boss_Arena.Text; }
		private void Boss_Tune_TextChanged(object sender, TextChangedEventArgs e) { KthuraData.Current.CurBoss.Music = Boss_Tune.Text; }

		private void BossFoeChange(object sender, TextChangedEventArgs e) {
			var BS = KthuraData.Current.CurBoss;
			var TB = (TextBox)sender;
			if (BS == null) return;
			for (var i = 1; i < 10; i++) {
				if (BossFoe[i] == TB) { BS[i] = TB.Text; break; }
			}
		}

		private void BossSkillChange(object sender, TextChangedEventArgs e) {
			var BS = KthuraData.Current.CurBoss;
			var TB = (TextBox)sender;
			if (BS == null) return;
			for (var idx = 1; idx < 10; idx++) {
				for (var skill = 1; skill <= 3; skill++) {
					if (BossSkill[idx, skill] == TB) { BS[idx, skill] = qstr.ToInt(TB.Text); break; }
				}
			}
		}

		private void NewVitalAdd_Click(object sender, RoutedEventArgs e) {
			KthuraData.Current.AddVital(NewVital.Text);
			NewVital.Text = "";
		}
	}
}