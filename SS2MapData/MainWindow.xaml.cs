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
// Version: 22.04.05
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

namespace SS2MapData {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		#region Init
		public MainWindow() {
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
			Layers.MW = this;
			Layers.Register(TxtLayTitle, "Title");
			Layers.Register(CheckNoEncounter, "NoEcounters", Layers.Type.Checkbox);
			Layers.Register(TxtLayAltArena, "AltArena");
			Layers.Register(Txt_ScrollMinX, "Scroll_MinX");
			Layers.Register(Txt_ScrollMinY, "Scroll_MinY");
			Layers.Register(Txt_ScrollMaxX, "Scroll_MaxX");
			Layers.Register(Txt_ScrollMaxY, "Scroll_MaxY");
			Layers.Register(Txt_ScrollCenX, "Scroll_CenX");
			Layers.Register(Txt_ScrollCenY, "Scroll_CenY");
			Behavior.SourceBox = Behavior_Source;
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
	}
}