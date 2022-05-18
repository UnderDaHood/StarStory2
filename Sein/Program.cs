// Lic:
// Sein
// Program
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
// Version: 22.05.17
// EndLic

using TrickyUnits;
using UseJCR6;
using NSKthura;
using System.Diagnostics;

const string _kthura = "scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Story/Kthura";
string KthuraDir;

void Init() {
	MKL.Version("Star Story 2 - The Virus Strikes Back - Program.cs","22.05.17");
	MKL.Lic    ("Star Story 2 - The Virus Strikes Back - Program.cs","GNU General Public License 3");
	JCR6_zlib.Init();
	Dirry.InitAltDrives();
	KthuraDir = Dirry.AD(_kthura);
	Kthura.LoadUnknown = true;
	QCol.DoingTab = 15;
}

void Head() {
	QCol.Green($"Sein {MKL.Newest}\n");
	QCol.Yellow("Coded by: Jeroen P. Broks\n");
	QCol.Red($"(c) {MKL.CYear(2022)} Jeroen P. Broks\n\n");
}

void Run() {
	QCol.Doing("Analysing", _kthura);
	var Maps = FileList.GetDir(KthuraDir);
	Console.WriteLine();
	foreach (var Map in Maps) Workout(Map);
}

void Workout(string MFile) {
	try {
		bool modified = false;
		QCol.Doing("Analyzing", MFile);
		var KMap = Kthura.Load($"{KthuraDir}/{MFile}");
		if (!KMap.Unknown.ContainsKey("Sein")) {
			modified = true;
			KMap.Unknown["Sein"] = qstr.StringToBytes($"[*CREATION]\nCreated={DateTime.Now}\nAuthor=Jeroen P. Broks\n");
			QCol.Doing("Created", "Sein data");
		}
		var SeinSource = qstr.BytesToString(KMap.Unknown["Sein"]); 
		//QCol.White($"<SeinSource>\n{SeinSource}\n</SeinSource>\n"); // debug
		var Sein = GINIE.FromSource(SeinSource);
		foreach (var Lay in KMap.Layers) {
			QCol.Doing("Layer", Lay.Key);
			foreach (var o in Lay.Value.Objects) {
				if (o.kind == "Obstacle" && qstr.Prefixed(o.Texture.ToUpper(), "GFX/TEXTURES/MEDALS/SEIN/")) {
					QCol.Doing("Found", "Sein"); Console.Beep();
					if (!qstr.Prefixed(o.Tag, "SEIN")) {
						int i = 0;
						string mkTag;
						do { mkTag = $"SEIN_{qstr.md5($"::{MFile}::{Lay.Key}::{++i}")}"; } while (Lay.Value.HasTag(mkTag));
						o.Tag = mkTag;
					}
					QCol.Doing("Tag", o.Tag);
					if (!Sein.List("LAYERS", "Layers").Contains(Lay.Key)) { Sein.List("LAYERS", "Layers").Add(Lay.Key.Replace("#","[HASH]")); QCol.Doing("Listed", "Layer"); modified = true; }
					//if (!Sein.List("LAYERS_BACKUP", "Layers").Contains(Lay.Key)) { Sein.List("LAYERS_BACKUP", "Layers").Add(Lay.Key); QCol.Doing("Listed", "Layer (backup)"); modified = true; }
					if (!Sein.List(Lay.Key, "Seinen").Contains(o.Tag)) { Sein.List(Lay.Key, "Seinen").Add(o.Tag); QCol.Doing("Listed", "Sein"); modified = true; }
					var Cat = $"::SEIN::{Lay.Key}::{o.Tag}::";
					if (Sein.List(Cat, "Requirements").Count == 0) {
						modified = true;
						if (qstr.Prefixed(o.Tag, "SEIN_NBX_")) {
							QCol.Red("Explicit requiest not to add BlackBox to this one!");
						} else { 
							QCol.Green("Automatically added BlackBox as requirement");
							Sein.List(Cat, "Requirements").Add("&BeenInBlackBox");
						}
						QCol.Magenta("\n\nI need some requirements for this barrier to open.\n");
						string a = "";
						do {
							QCol.Yellow("> "); QCol.Cyan("");
							a = $"{Console.ReadLine()}";
							a = a.Trim();
							if (a != "") Sein.List(Cat, "Requirements").Add(a);
						} while (a != "");
					}
					if (Sein.List(Cat, "Barriers").Count == 0) {
						modified = true;
						QCol.Magenta("\n\nI need some barrier tags to open.\n");
						foreach (var ot in Lay.Value.Objects) {
							if (
								ot.Tag != "" &&
								ot.Tag != o.Tag && // We don't want the sein itself to be listed!
								(
								ot.kind == "Zone" ||
								ot.kind == "TiledArea" ||
								ot.kind == "StretchedArea" ||
								ot.kind == "Rect")) {
								QCol.Red("* ");
								QCol.Green($"{ot.kind}: {ot.Tag}\n");
							}
						}
						string a;
						do {
							QCol.Yellow("> "); QCol.Cyan("");
							a = $"{Console.ReadLine()}";
							a = a.Trim();
							if (a != "") Sein.List(Cat, "Barriers").Add(a);
						} while (a != "" || Sein.List(Cat, "Barriers").Count == 0);

					}
					if (Sein.List(Cat, "Labels").Count==0) {
						var Labeltjes = new TMap<string, int>();
						foreach (var ol in Lay.Value.Objects) {
							var lab = ol.Labels.Split(',');
							foreach (var l in lab) if (l!="") Labeltjes[l]++;
						}
						QCol.Magenta("\n\nI need some labels to show.\n");
						foreach (var l in Labeltjes.Keys) {
							QCol.Red("* ");
							QCol.Green(l);
							QCol.Cyan($" ({Labeltjes[l]} objects)\n");
						}
						string a;
						do {
							QCol.Yellow("> "); QCol.Cyan("");
							a = $"{Console.ReadLine()}";
							a = a.Trim();
							if (a != "") Sein.List(Cat, "Labels").Add(a);
						} while (a != "" || Sein.List(Cat, "Labels").Count == 0);
					}
				}
			}
		}


		if (modified) {
			QCol.Doing("Saving", MFile);
			var F = $"{KthuraDir}/{MFile}";
			Debug.WriteLine($"Saving: {F}");
			Sein["*CREATION","Modified"] = $"{DateTime.Now}";
			KMap.Unknown["Sein"]=qstr.StringToBytes(Sein.ToSource());
			KthuraSave.Save(KMap, F, "zlib","Jeroen P. Broks", "(c) Jeroen P. Broks - No extraction without author's prior written permission.");
			Debug.WriteLine($"JCR6 JERROR:\"{JCR6.JERROR}\"");
#if DEBUG
			foreach (var U in KMap.Unknown) Debug.WriteLine($"U: {U.Key}\t({U.Value.Length} bytes)");
#endif
		}
	} catch (Exception ex) {
		QCol.QuickError(ex.Message);
#if DEBUG
		QCol.Magenta($"{ex.StackTrace}\n");
#endif
	}
}

#region int main(int c, char * args)
Init();
Head();
Run();
#endregion