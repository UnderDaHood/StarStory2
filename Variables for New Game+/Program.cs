// Lic:
// Star Story II
// Collector Cycle Transfer Variables
// 
// 
// 
// (c) Jeroen P. Broks, 2023
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
// Version: 23.08.30
// EndLic

using System.Text;
using TrickyUnits;
using UseJCR6;

JCR6_zlib.Init();

const string SGDir = "H:/Home/SaveGame/Apollo/ApolloGameData/SS2TVSB/SaveGame";

const string OutPutScript = "E:/Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Script/Script/Use/lnk.uwn/NewGamePlus.neil";

var Output = new StringBuilder($"\n\n// Generated {DateTime.Now}\n\nVoid _SNGP()\n\tIf !Sys.Yes(\"Start a new game cycle?\")\n\t\tReturn\n\tend\n\n\n");
var Types = new Dictionary<string, string>(); Types["string"] = "\"\""; Types["int"] = "0"; Types["bool"] = "false";
var SGList = FileList.GetDir(SGDir);
var Config = GINIE.FromFile("../../New Game+.ini");
var Ask = new TAsk(Config);
var Gedaan = new List<string>();
Config.AutoSaveSource = "../../New Game+.ini";

if (SGList == null) {
	QCol.QuickError($"Reading dir {SGDir} apparently failed!");
	return;
}


foreach (var s in SGList) {
	if (s.ToUpper() != "PAGES.NEIL") {
		QCol.Doing("Analysing", s);
		var J = JCR6.Dir($"{SGDir}/{s}");
		if (J == null)
			QCol.QuickError($"Error loading {SGDir}/{s}! {JCR6.JERROR}");
		else {


			var VSRC = J.LoadString("DATA/VARS.INI");
			var VGIN = GINIE.FromSource(VSRC);
			foreach (var T in Types) {
				var EV = VGIN.Each(T.Key);
				if (EV == null)
					QCol.QuickError($"Nothing out there in type {T.Key}");
				else {
					foreach (var K in EV) {
						var TransTag = $"Transfer:{T.Key}";
						if (qstr.Prefixed(K, "AWARDED_") || qstr.Prefixed(K,"ANALYSED_"))
							Config[TransTag, K] = "YES";
						else if (qstr.Prefixed(K,"TRANS_") || qstr.Prefixed(K,"BEEN_") || qstr.Prefixed(K, "WELCOME_") || qstr.Prefixed(K, "EFU_KEY_"))
							Config[TransTag, K] = "NO";
						if (!Ask.Yes(TransTag, K, $"Transfer {T.Key} variable {K}")) {
							var Tag = $"{T}::{K}";
							if (!Gedaan.Contains(Tag)) {
								Output.Append($"\tgv.g{T.Key}[\"{K}\"] = {T.Value}\n");
								Gedaan.Add(Tag);
							}
						}
					}
				}
			}
		}
	}
}
Output.Append("\tlnk.party.party(\"Klahre\",\"Yorno\")\n");
Output.Append("\tlnk.Map.GoToMap(\"001_HyperloopStation\",\"GATE27\",\"Start_Klahre\")\n");
Output.Append("\tAny.Caps.Cycle++\n");
Output.Append("End\n\nReturn _SNGP");
QCol.Doing("Saving", OutPutScript);
QuickStream.SaveString(OutPutScript, Output);
QCol.Green("Ok\n");
Console.ForegroundColor = ConsoleColor.Gray;