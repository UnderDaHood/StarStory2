// Lic:
// ...
// ....
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
// Version: 22.05.16
// EndLic
using System.Text;
using TrickyUnits;

const string outdir = "E:/Projects/Applications/Apollo/Games/Star Story 2/Dev/Sudoku";

var l = new List<string>(new string[]{
	".3#4.7.2.8.1#5.9#6",
	".5.1#8.7.9.6.4#2.3",
	"#9.6.2.4#3.5#8.1.7",

	"#1.5.3.8.7.2#9.6#4",
	".4#7.9.1#6.3.2#8.5",
	".2.8#6.5.4.9#7.3#1",

	"#7#3.4.9#1#8.6#5.2",
	"#6#9.5.3#2#7#1.4#8",
	".8#2#1#6.5#4.3#7#9"});

var outp1 = new StringBuilder("Dim Nummers%(9,9)\n");
var outp2 = new StringBuilder("Dim Always%(9,9)\n");
var outp3 = new StringBuilder("");
var cempty = 0;

Console.WriteLine($"Parsing {l.Count} lines!");

for (var line = 0; line < l.Count; line++) {
	for (var pos = 0; pos < l[line].Length/2; pos++) {
		Console.Write($" {line}:{pos}\r");
		outp1.Append($"Nummers({pos},{line}) = {l[line][(pos * 2) + 1]}\n");
		outp2.Append($"Always({pos},{line}) = {l[line][pos] == '#'}\n");
		if(l[line][pos] != '#') { outp3.Append($"EmptyX({cempty})={pos}\n"); outp3.Append($"EmptyY({cempty++})={line}\n"); }
	}
}
Console.WriteLine();


var outp = $"; Generated {DateTime.Now}\n\n{outp1}\n\n{outp2}\n\nDim EmptyX%({cempty})\nDim EmptyY%({cempty})\n{outp3}\n";
//Console.WriteLine(outp);
QuickStream.SaveString($"{outdir}/Data.bb", outp);