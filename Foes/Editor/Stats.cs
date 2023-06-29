// Lic:
// For Editor
// Stats
// 
// 
// 
// (c) Jeroen P. Broks, 2022, 2023
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
// Version: 23.01.23
// EndLic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarStory2_Foe_Editor {
	class Stats {

			/*
		public class WStat {
			readonly public string k;
			readonly public Stats s;
			internal WStat(string _k,Stats _s) { k = _k;s = _s; }
		}
		readonly public static Dictionary<TextBox, WStat> Register = new Dictionary<TextBox, WStat>();
		public readonly string Stat;
		public readonly TextBox
			Min,
			Max,
			Sk1,
			Sk2,
			Sk3;
			*/

		static public List<TextBox> Register = new List<TextBox>();


        public Stats(string s, TextBox _min, TextBox _max, TextBox _1, TextBox _2, TextBox _3) {
            /*
        Min = _min;
        Max = _max;
        Sk1 = _1;
        Sk2 = _2;
        Sk3 = _3;
        Register[Min] = new WStat("Min", this);
        Register[Max] = new WStat("Max", this);
        Register[Sk1] = new WStat("Sk1", this);
        Register[Sk2] = new WStat("Sk2", this);
        Register[Sk3] = new WStat("Sk3", this);
            */
            Data.RegTextBox(_min, "STAT.MIN", s);
            Data.RegTextBox(_max, "STAT.MAX", s);
            Data.RegTextBox(_1, "STAT.SK1", s);
            Data.RegTextBox(_2, "STAT.SK2", s);
            Data.RegTextBox(_3, "STAT.SK3", s);

            Register.Add(_min);
            Register.Add(_max);
            Register.Add(_1);
            Register.Add(_2);
            Register.Add(_3);
        }
    }
}