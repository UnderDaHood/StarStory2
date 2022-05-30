// Lic:
// Kist
// Main Program
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
// Version: 22.05.29
// EndLic

using System.Diagnostics;
using TrickyUnits;
using UseJCR6;
using NSKthura;

const string _Chest = "GFX/TREASURE/TREASURECHEST.PNG";
const string _KthuraDir = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Story/Kthura";
int errors = 0;
string KthuraDir() => Dirry.AD(_KthuraDir);
string KthuraFP(string mp)=>$"{KthuraDir()}/{mp}";
string numfile = qstr.ExtractDir(MKL.MyExe) + "/Kist.num";
int GetNum() {
	if (!File.Exists(numfile)) QuickStream.SaveString(numfile, "-1");
	return qstr.ToInt(QuickStream.LoadString(numfile));
}
	
int num = GetNum(); Debug.WriteLine($"StartNum = {num}");
Kthura.LoadUnknown = true;

void Init() {
	Dirry.InitAltDrives();
	JCR6_zlib.Init();
	MKL.Version("Star Story 2 - The Virus Strikes Back - Program.cs","22.05.29");
	MKL.Lic    ("Star Story 2 - The Virus Strikes Back - Program.cs","GNU General Public License 3");
}

void Head() {
	QCol.Green("Kist\n");
	QCol.Doing("Version", MKL.Newest);
	QCol.Magenta($"(c) {MKL.CYear(2022)} Jeroen P. Broks\n\n");
	QCol.White($"{MKL.All()}\n\n\n");
}

string MKTag(KthuraLayer L) {
	string Tag;
	do { Tag = $"CHEST_{Roman.ToRoman(++num)}"; } while (L.HasTag(Tag));
	QuickStream.SaveString(numfile, $"{num}");
	return Tag;
}

void WorkMap(string m) {
	var modified = false;
	QCol.Doing("Loading", m);
	var KM = Kthura.Load(KthuraFP(m));
	if (!KM.Unknown.ContainsKey("Treasures")) KM.Unknown["Treasures"] = qstr.StringToBytes($"[SYS]\nAUTHOR=Jeroen P. Broks\nCREATED={DateTime.Now}\nCreatedBy=Kist\n");
	if (!KM.Unknown.ContainsKey("Sein")) { QCol.QuickError($"ERROR #{++errors}: Run Sein on this map first!"); return; }
	var TRDat = GINIE.FromSource(KM.Unknown["Treasures"]);
	var SNDat = GINIE.FromSource(KM.Unknown["Sein"]);
	foreach(var l in KM.Layers) {
		QCol.Doing("=> Layer", l.Key);
		foreach (var o in l.Value.Objects) {
			if (o.Texture.ToUpper() == _Chest) {
				QCol.Doing("==> Chest found", $"({o.x},{o.y}) {o.Tag}");
				if (o.Tag == "") { o.Tag = MKTag(l.Value); QCol.Doing("===> AutoTagging", o.Tag); modified = true; }
				//Console.WriteLine($"Sein for {o.Tag}. Treasure Data has Sein Data: {TRDat.HasList(o.Tag, "Sein")}; There are seinen on map: {SNDat.HasList(l.Key, "Seinen")}"); // Debug only
				//if (!TRDat.HasList(o.Tag, "Sein") && SNDat.HasList(l.Key, "Seinen")) {
				if (TRDat[o.Tag,"SeinSet"]!="DONE" && SNDat.HasList(l.Key, "Seinen")) {
					TRDat[o.Tag, "SeinSet"] = "DONE";
					TRDat.List(o.Tag, "Sein"); // Make sure the list is created
					QCol.Yellow($"Er zijn {SNDat.List(l.Key, "Seinen").Count} sein(en) gevonden!\n");
					foreach (var s in SNDat.List(l.Key, "Seinen")) {
						QCol.Red(">> "); QCol.Green($"{s}\n");
					}
					QCol.White("Bind to which?\n");
					string a;
					do {
						QCol.Yellow("BIND> "); QCol.Cyan("");
						a = Console.ReadLine().Trim();
						if (a.Length > 0) TRDat.ListAddNew(o.Tag, "Sein", a);
					} while (a.Length > 0);
					modified = true;
				}
				while (TRDat[o.Tag, "ITEM"] == "") {
					QCol.Yellow("And what item should be put in? "); QCol.Cyan("");
					TRDat[o.Tag, "ITEM"] = Console.ReadLine().Trim().ToUpper();
					modified = true;
				}
			}
		}
	}
	if (modified) {
		QCol.Doing("Saving", m);
		KM.Unknown["Treasures"] = qstr.StringToBytes(TRDat.ToSource());
		KthuraSave.Save(KM, KthuraFP(m), "zlib", "Jeroen P. Broks", "Property of Jeroen P. Broks. Don't extract from this project!");
	}
}

QCol.DoingTab *= 2;
Init();
Head();
QCol.Doing("Analysing", _KthuraDir);
var Maps = FileList.GetDir(KthuraDir());
QCol.Doing("Found", $"{Maps.Length} maps");
foreach (var Map in Maps) WorkMap(Map);