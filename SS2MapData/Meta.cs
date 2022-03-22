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
// Version: 22.03.22
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

		static public void Register(TextBox tb,string field,bool IntOnly=false) {
			_Register[tb] = new Meta(tb, field,IntOnly);
		}

		readonly public TextBox TextB;
		readonly public string Field;
		readonly public bool IntOnly;

		private Meta(TextBox tb,string fld,bool _IntOnly) {
			TextB = tb;
			Field = fld;
			IntOnly = _IntOnly;
		}

		static  bool NoUpdate = false;

		static public void Load() {
			if (KthuraData.Current == null) return;
			NoUpdate = true;
			foreach(var a in _Register.Values) {
				if (!KthuraData.Current.TheMap.MetaData.ContainsKey(a.Field)) KthuraData.Current.TheMap.MetaData[a.Field] = "";
				if (a.IntOnly) a.TextB.Text = $"{qstr.ToInt(KthuraData.Current.TheMap.MetaData[a.Field])}"; else a.TextB.Text = KthuraData.Current.TheMap.MetaData[a.Field];
			}
			NoUpdate = false;
		}

		static public void Update(TextBox tb) {
			var e = _Register[tb];
			KthuraData.Current.TheMap.MetaData[e.Field] = tb.Text;
		}


	}
}