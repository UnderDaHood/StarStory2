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
// Version: 22.03.22
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
			Layers.MW = this;
			Layers.Register(TxtLayTitle, "Title");
			Layers.Register(CheckNoEncounter, "NoEcounters", Layers.Type.Checkbox);
			Layers.Register(TxtLayAltArena, "AltArena");
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
    }
}