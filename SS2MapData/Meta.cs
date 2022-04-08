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
// Version: 22.04.05
// EndLic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrickyUnits;

namespace SS2MapData {
	class Meta {
		static Dictionary<TextBox, Meta> _Register = new Dictionary<TextBox, Meta>();
		static Dictionary<CheckBox, Meta> _CheckBoxRegister = new Dictionary<CheckBox, Meta>();

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

		private Meta(TextBox tb,string fld,bool _IntOnly) {
			TextB = tb;
			Field = fld;
			IntOnly = _IntOnly;
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
			foreach(var a in _Register.Values) {
				if (!KthuraData.Current.TheMap.MetaData.ContainsKey(a.Field)) KthuraData.Current.TheMap.MetaData[a.Field] = "";
				if (a.IntOnly) a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.TheMap.MetaData[a.Field])}"; else a.TextB.Text = KthuraData.Current.TheMap.MetaData[a.Field];
			}
			foreach(var b in _CheckBoxRegister.Values) {
				if ((!KthuraData.Current.TheMap.MetaData.ContainsKey(b.Field)) || KthuraData.Current.TheMap.MetaData[b.Field].Trim()=="") KthuraData.Current.TheMap.MetaData[b.Field] = qstr.YesNo(b.DefBoolValue);
				b.ChkB.IsChecked = KthuraData.Current.TheMap.MetaData[b.Field].Trim().ToUpper() == "YES";
			}
			NoUpdate = false;
		}

		static public void Update(TextBox tb) {
			var e = _Register[tb];
			KthuraData.Current.TheMap.MetaData[e.Field] = tb.Text;
		}

		static public void Update(CheckBox cb) {
			var e = _CheckBoxRegister[cb];
			if ((bool)cb.IsChecked)
				KthuraData.Current.TheMap.MetaData[e.Field] = "Yes";
			else
				KthuraData.Current.TheMap.MetaData[e.Field] = "No";
		}


	}
}