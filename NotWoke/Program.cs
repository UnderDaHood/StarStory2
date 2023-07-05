// Lic:
// Not Woke
// Deals with Trans without being woke
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
// Version: 23.07.05
// EndLic

#region Using
using UseJCR6;
using TrickyUnits;
using NSKthura;
using System.Text;
#endregion

int Count = 0;
string KthuraDir() => Dirry.AD("Scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Story/Kthura");
string[] Maps;
string DataDir() => Dirry.AD(@"Scyndi:Projects\Applications\Apollo\Games\Star Story 2\dev\NotWoke");
string ScottyFile() => Dirry.AD(@"Scyndi:Projects\Applications\Apollo\Games\Star Story 2\src\Tricky Script\Data\General\Scotty.ini");
GINIE OwnData;
GINIE Scotty;

KthuraDraw.DrawDriver = new KthuraDrawFake();

string NCount(KthuraLayer L,string pref) {
	var s = "";
	do { s = $"TRANS_{pref}__{Roman.ToRoman(Count++)}"; } while (L.HasTag(s));
	OwnData["Sys", "Count"] = $"{Count}";
	return s;
}

void Init() {
	MKL.Version("Star Story 2 - The Virus Strikes Back - Program.cs","23.07.05");
	MKL.Lic    ("Star Story 2 - The Virus Strikes Back - Program.cs","GNU General Public License 3");
	JCR6_zlib.Init();
	Dirry.InitAltDrives();
	var ODF = $"{DataDir()}/NotWoke.ini";
	OwnData = GINIE.FromFile(ODF);
	OwnData.AutoSaveSource = ODF;
	OwnData["ALG", "ID"] = "Not woke";
	if (OwnData["ALG", "CREATED"] == "") OwnData["ALG","CREATED"]=$"{DateTime.Now}";
	OwnData["ALG", "LASTUSED"] = $"{DateTime.Now}";
	Count = qstr.ToInt(OwnData["Sys", "Count"]);
	Kthura.LoadUnknown = true;
	Scotty = GINIE.FromFile(ScottyFile());
	Scotty.AutoSaveSource = ScottyFile();
	if (Scotty["ALGEMEEN", "AANGEMAAKT"] == "") Scotty["ALGEMEEN", "AANGEMAAKT"] = $"{DateTime.Now}";
	Scotty["ALGEMEEN", "LAATSTBIJGEWERKT"] = $"{DateTime.Now}";
}

void Head() {
	QCol.Yellow("Not Woke ");
	QCol.Magenta(MKL.Newest);
	QCol.Yellow("\nCoded by: Jeroen P. Broks");
	Console.WriteLine();
}

void GetMaps() {
	QCol.Doing("Reading", KthuraDir());
	Maps = FileList.GetDir(KthuraDir());
}

string XAsk(GINIE OD,string cat,string key,string question,string defaultvalue = "") {
	while (OD[cat, key] == "") {
		if (defaultvalue != "") QCol.Magenta($"[{defaultvalue}] ");
		QCol.Yellow(question);
		QCol.Cyan(" ");
		var Answer = Console.ReadLine();
		if (Answer == "")
			OD[cat, key] = defaultvalue;
		else
			OD[cat, key] = Answer;
	}
	return OD[cat, key];
}
string Ask(string cat, string key, string question, string defaultvalue = "") => XAsk(OwnData, cat, key, question, defaultvalue);

void AdaptMap(string fname) {
	QCol.Doing("Map", fname);	
	var kth = Kthura.Load($"{KthuraDir()}/{fname}");
	var Loc = GINIE.FromSource($"[SYS]\nCreated={DateTime.Now}\n\n");
	if (kth.Unknown.ContainsKey("Trans")) Loc = GINIE.FromSource(kth.Unknown["Trans"]);
	foreach(var lay in kth.Layers) {
		QCol.Doing("==>", lay.Key);
		foreach(var obj in lay.Value.Objects) {
			//QCol.Doing("Tex", obj.Texture); // Tex check
			switch(obj.Texture.ToUpper()) {
				case "GFX/TRANS/GENERAL.PNG":
					if (obj.Tag == "") {
						var T = NCount(lay.Value, "GENERAL");
						QCol.Doing("Tagging:", T);
						obj.Tag = T;
					}
					var ktgtag=$"{fname}_{lay.Key}__{obj.Tag}";
					var TLoc = $"Lokatie:{ktgtag}";
					if (fname.ToUpper() != "015_EFU_HQ_2")
						Scotty.ListAddNew("Kaarten", "Lijst", fname);
					Scotty["Kaarten",fname] = Ask(ktgtag, "Map", $"{ktgtag} is part of map: ", kth.MetaData["Title"]);
					Scotty[TLoc, "Naam"] = Ask(ktgtag, "Loc", $"{ktgtag} as specific name to name it by in the menu: ");
					Scotty[TLoc, "Kthura"] = fname;
					if (fname.ToUpper() == "015_EFU_HQ_2") {
						Scotty[TLoc, "Lijst"] = "014_EFU_HQ_1";
						QCol.Doing("Relocate", "HQ Part 1 and 2 merge!\x7");
					} else
						Scotty[TLoc, "Lijst"] = fname;
					//Scotty.ListAddNew($"Kaart:{fname}", "Lokaties", ktgtag);
					Scotty.ListAddNew($"Kaart:{Scotty[TLoc, "Lijst"]}", "Lokaties", ktgtag);
					Scotty[TLoc, "Laag"] = lay.Key;
					Scotty[TLoc, "Object"] = obj.Tag;

					XAsk(Scotty, TLoc, "OpenVanafAanvang", "This is transporter open from the start?", "NO");
					Ask(ktgtag, "Tut", $"{ktgtag} has any hint tied to it? ", "NONE");
					Loc[lay.Key, obj.Tag] = OwnData[ktgtag, "Tut"];
					kth.Unknown["Trans"] = Encoding.ASCII.GetBytes(Loc.ToSource());
					QCol.Doing("TODO", "General trans");
					break;
				case "GFX/TRANS/RECOVER.PNG":
					if (obj.Tag == "") {
						var T = NCount(lay.Value, "RECOVER");
						QCol.Doing("Tagging", T);
						obj.Tag = T;
					}
					
					break;
				case "GFX/TRANS/RETURNONLY.PNG":
					if (obj.Tag == "") {
						var T = NCount(lay.Value, "RETURNONLY");
						QCol.Doing("Tagging:", T);
						obj.Tag = T;
					}
					break;
			}
		}
	}
	KthuraSave.Save(kth, $"{KthuraDir()}/{fname}","zlib","Jeroen P. Broks","Property of and copyrighted by Jeroen P. Broks. Do NOT extract without prior written permission");
}

#region int Main(int c, char* args) 
Init();
Head();
GetMaps();
foreach (var map in Maps) AdaptMap(map);
Console.ResetColor();
#endregion