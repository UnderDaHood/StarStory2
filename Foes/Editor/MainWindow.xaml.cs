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
// Version: 22.03.01
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
			GetFoeList();
			Data.RegFoeList(FoeList);
			MainTab.Visibility = Visibility.Hidden;
			Data.RegTextBox(TextFoeName, "Name");
			Data.RegTextBox(TextFoeDescription, "Description");
			Data.RegTextBox(TextVocalTag, "VocalTag");
		}

		void GetFoeList() {
			Debug.WriteLine("Renewing Foe List");
			FoeList.Items.Clear();
			var L = FileList.GetTree(Data.Dir);
			foreach(var f in L) {
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
			if (qstr.ExtractDir(txt)=="") {
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
			QuickStream.SaveString(fname, $"[alg]\nCreationTime={DateTime.Now}\nCreationFile={fname}\n[meta]\nName={qstr.StripDir(txt)}\nVocalTag={txt}\nNormSprite=GFX/Combat/Foe/{txt}.png\nNegaSprite=NormSprite=GFX/Combat/Foe/{txt}.Negative.png");
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
		}

	  
	}
}