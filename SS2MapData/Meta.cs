// Lic:
// Map Extra Data Editor
// Meta
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
// Version: 22.05.28
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

	class Meta {
		public enum MetaAlt { Meta, Item, Foe, Sein};

		static Dictionary<TextBox, Meta> _Register = new Dictionary<TextBox, Meta>();
		static Dictionary<CheckBox, Meta> _CheckBoxRegister = new Dictionary<CheckBox, Meta>();
		
		static public Dictionary<TextBox,Meta> GetRegister => _Register;

		static public void Register(TextBox tb,string field,bool IntOnly=false) {
			_Register[tb] = new Meta(tb, field,IntOnly);
		}

		static public void Register(CheckBox tb,string field, bool defaulttrue = false) {
			_CheckBoxRegister[tb] = new Meta(tb, field, defaulttrue);
		}

		readonly public TextBox TextB = null;
		readonly public CheckBox ChkB = null;
		readonly public string Field;
		readonly public bool IntOnly;
		readonly public bool DefBoolValue=false;

		readonly public MetaAlt Alt = MetaAlt.Meta; 
		

		private Meta(TextBox tb,string fld,bool _IntOnly) {
			TextB = tb;
			Field = fld;
			IntOnly = _IntOnly;			
			if (qstr.Prefixed(fld.ToUpper(), "ITEM:")){
				Field = fld.Substring(5);
				Alt = MetaAlt.Item;
			}
			if (qstr.Prefixed(fld.ToUpper(), "FOE:")) {
				Field = fld.Substring(4);
				Alt = MetaAlt.Foe;
			}
			if (fld == "Sein!") Alt = MetaAlt.Sein;
			Debug.WriteLine($"Registered textbox for field {Field} as type {Alt}");
		}

		private Meta(CheckBox cb,string fld,bool defaulttrue = false) {
			ChkB = cb;
			Field = fld;
			DefBoolValue = defaulttrue;
		}

		static  bool NoUpdate = false;

		static public void Load() {
			if (KthuraData.Current == null) return;
			NoUpdate = true;
			foreach (var a in _Register.Values) {
				switch (a.Alt) {
					case MetaAlt.Meta:
						if (!KthuraData.Current.TheMap.MetaData.ContainsKey(a.Field)) KthuraData.Current.TheMap.MetaData[a.Field] = "";
						if (a.IntOnly) a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.TheMap.MetaData[a.Field])}"; else a.TextB.Text = KthuraData.Current.TheMap.MetaData[a.Field];
						break;
					case MetaAlt.Item: {
							var EI = MainWindow.MySelf.EditItem;
							if (EI != "") {
								if (KthuraData.Current.Treasures[$"ITEM:{EI}", a.Field] == "") KthuraData.Current.Treasures[$"ITEM:{EI}", a.Field] = "1";
								//if (a.IntOnly) a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.TheMap.MetaData[a.Field])}"; else a.TextB.Text = KthuraData.Current.TheMap.MetaData[a.Field];
								a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.Treasures[$"ITEM:{EI}", a.Field])}";
							} else { Debug.WriteLine($"I cannot retrieve field {a.Field}. There doesn't seem to be an item selected"); }
						}
						break;
					case MetaAlt.Foe: {
							var EI = MainWindow.MySelf.EditFoe;
							if (EI != "") {
								if (KthuraData.Current.Foes[$"Foe:{EI}", a.Field] == "") KthuraData.Current.Foes[$"Foe:{EI}", a.Field] = "1";
								//if (a.IntOnly) a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.TheMap.MetaData[a.Field])}"; else a.TextB.Text = KthuraData.Current.TheMap.MetaData[a.Field];
								a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.Foes[$"Foe:{EI}", a.Field])}";
							} else { Debug.WriteLine($"I cannot retrieve field {a.Field}. There doesn't seem to be an item selected"); }
						}
						break;
					case MetaAlt.Sein:
						/*if (!KthuraData.Current.TheMap.Unknown.ContainsKey("Sein")) {
							a.TextB.Text = "# No sein data present\n";
							string f = "";
							//foreach (var k in KthuraData.Current.Unknown.Keys) f += $"# I do have: '{k}'\n";
							a.TextB.Text += f;
						} else {
						*/
							a.TextB.Text = KthuraData.Current.Sein;
						//}
						break;
					default:
						Confirm.Annoy($"No alteration(TB) action known for {a.Alt}! ({a.Field}");
						break;
				}
			}
			foreach(var b in _CheckBoxRegister.Values) {
				switch (b.Alt) {
					case MetaAlt.Meta:
						if ((!KthuraData.Current.TheMap.MetaData.ContainsKey(b.Field)) || KthuraData.Current.TheMap.MetaData[b.Field].Trim() == "") KthuraData.Current.TheMap.MetaData[b.Field] = qstr.YesNo(b.DefBoolValue);
						b.ChkB.IsChecked = KthuraData.Current.TheMap.MetaData[b.Field].Trim().ToUpper() == "YES";
						break;
					default:
						Confirm.Annoy($"No alteration(CB) action known for {b.Alt}! ({b.Field}");
						break;
				}
			}
			NoUpdate = false;
		}

		static public void Update(TextBox tb) {
			if (!_Register.ContainsKey(tb)) {
				//Confirm.Annoy($"TextBox {tb.Name} could not be found in the register","Internal error",System.Windows.Forms.MessageBoxIcon.Error); 
				Debug.WriteLine($"Skipped {tb.Name} as it's not (yet) in the register");
				return; 
			}
			var e = _Register[tb];
			switch (e.Alt) {
				case MetaAlt.Meta:
					KthuraData.Current.TheMap.MetaData[e.Field] = tb.Text;
					break;
				case MetaAlt.Item: {
						var EI = MainWindow.MySelf.EditItem;
						if (EI != "") KthuraData.Current.Treasures[$"Item:{EI}", e.Field] = tb.Text;
					}
					break;
				case MetaAlt.Foe: {
						var EI = MainWindow.MySelf.EditFoe;
						if (EI != "") KthuraData.Current.Foes[$"Foe:{EI}", e.Field] = tb.Text;
					}
					break;
				case MetaAlt.Sein: {
						//KthuraData.Current.TheMap.Unknown["Sein"]=qstr.StringToBytes(tb.Text);
						KthuraData.Current.Sein = tb.Text;
					}
					break;
				default:
					Confirm.Annoy($"No (edit)alteration(TB) action known for {e.Alt}! ({e.Field}");
					break;

			}
		}

		static public void Update(CheckBox cb) {
			var e = _CheckBoxRegister[cb];
			if ((bool)cb.IsChecked)
				KthuraData.Current.TheMap.MetaData[e.Field] = "Yes";
			else
				KthuraData.Current.TheMap.MetaData[e.Field] = "No";
			Debug.WriteLine($"MetaData field '{e.Field}' set to '{KthuraData.Current.TheMap.MetaData[e.Field]}'");
		}


	}
}