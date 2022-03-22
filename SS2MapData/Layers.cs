// Lic:
// Map Extra Data Editor
// Layers
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
using System.Windows.Controls;
using TrickyUnits;

namespace SS2MapData {
	class Layers {
		#region Statics
		public enum Type { TextBox, Checkbox};
		static Dictionary<object, Layers> _Register = new Dictionary<object, Layers>();
		//static readonly public GINIE Data = GINIE.FromSource($"[sys]\nCreated={DateTime.Now}\nAuthor=Jeroen P. Broks\n\n");
		static public GINIE Data {
			get {
				if (KthuraData.Current == null) return null;
				return KthuraData.Current.Layers;
			}
		}

		static public void Register(object gadget, string Field, Type GType = Type.TextBox) {
			_Register[gadget] = new Layers(gadget, Field, GType);
		}
		static public MainWindow MW = null;

		static public string Selected => MW.SelectedLayer;

		#endregion

		readonly object gadget;
		readonly string Field;
		readonly Type GType;

		public string this[string key] { 
			get {
				return Data[Selected, key];
			} 
			private set {
				if (Selected != "") Data[Selected, key] = value;
			}
		}

		static bool NoUpdate = false;
		public static void Receive() {
			NoUpdate = true;
			foreach(var g in _Register.Values) {
				switch (g.GType) {
					case Type.TextBox:
						((TextBox)g.gadget).Text = g[g.Field];
						break;
					case Type.Checkbox:
						((CheckBox)g.gadget).IsChecked = g[g.Field].Trim().ToUpper() == "TRUE";
						break;
					default:
						Debug.WriteLine($"I do not know how to receive data of type {g.GType} ({g.Field})");
						break;
				}
			}
			NoUpdate = false;
		}

		public static void Update(object g) {
			var d = _Register[g];
			if (NoUpdate) return;
			switch (d.GType) {
				case Type.TextBox:
					d[d.Field] = ((TextBox)g).Text;
					break;
				case Type.Checkbox:
					d[d.Field] = $"{((CheckBox)g).IsChecked}";
					break;
				default:
					Debug.WriteLine($"I do not know how to update data of type {d.GType} ({d.Field})");
					break;
			}
		}

		Layers(object _g,string _f,Type T) {
			gadget = _g;
			Field = _f;
			GType = T;
		}

		

	}
}