// Lic:
// Sewer Generator
// Well, a simple program that does what the name implies
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
// Version: 22.05.21
// EndLic
#define AllesKlaar

using NSKthura;
using UseJCR6;
using TrickyUnits;

const int
	w = 5,
	h = 5,
	mw = 3,
	mh = 3;


const string
	ori = @"E:\Projects\Applications\Apollo\Games\Star Story 2\src\Tricky Story\Kthura\006_Sewers",
#if AllesKlaar
	tar = @"E:\Projects\Applications\Apollo\Games\Star Story 2\src\Tricky Story\Kthura\006_Sewers";
#else
	tar = @"E:\Projects\Applications\Apollo\Games\Star Story 2\src\Tricky Story\Kthura\006_Sewers_Kopie";
#endif

Kthura
	TheMap;

KthuraLayer L(int x,int y) {
	var SL = $"AX{x}Y{y}";
	if (!TheMap.Layers.ContainsKey(SL)) { TheMap.CreateLayer(SL); QCol.Doing("Created", SL); }
	return TheMap.Layers[SL];
}



#region MAIN
JCR6_zlib.Init();
Kthura.LoadUnknown = true;

QCol.DoingTab = 20;

QCol.Doing("Loading", ori);
TheMap = Kthura.Load(ori);
for (int y = 1; y <= h; y++) {
	for (int x = 1; x <= w; x++) {
		var SL = $"AX{x}Y{y}";
		var Lay = L(x, y);
		if (Lay.LabelMap.ContainsKey("FROM_TEMPLATE")){
			QCol.Doing("Cleaning", SL);
			Lay.RemapLabels();
			var Kill = Lay.LabelMap["FROM_TEMPLATE"].ToArray();
			foreach (var victim in Kill) Lay.Objects.Remove(victim);
		}
		QCol.Doing("Cloning to", SL);
		foreach(var obj in TheMap.Layers["TEMPLATE"].Objects) {
			bool MagIk = true;
			switch (obj.Tag) {
				case "BN":
					MagIk = !((y > 1) || (x == mw));
					break;
				case "BS":
					MagIk = !((y < h) || (x == mw));
					break;
				case "BW":
					MagIk = !((x > 1));// || (y == mh));
					break;
				case "BE":
					MagIk = !((x < w));// || (y == mh));
					break;					
			}
			if (MagIk) {
				var taro = Lay.Clone(obj);
				taro.Labels = "FROM_TEMPLATE";
				switch (obj.Tag) {
					case "GN":
						if (y == 1) 
						taro.Tag = $"GO_#002_Start";
						else
							taro.Tag = $"GO_AX{x}Y{y - 1}_StartS";
						break;
					case "GS":
						if (y == h)
							taro.Tag = $"GO_#001_Einde";
						else
							taro.Tag = $"GO_AX{x}Y{y + 1}_StartN";
						break;
					case "GW":
						taro.Tag = $"GO_AX{x - 1}Y{y}_StartE";
						break;
					case "GE":
						taro.Tag = $"GO_AX{x + 1}Y{y}_StartW";
						break;
				}
			}

		}
	}
}
QCol.Doing("Saving", tar);
KthuraSave.Save(TheMap, tar, "Store", "Jeroen P. Broks", "Don't use without permission");
QCol.Cyan("Ok\n\n");
Console.ResetColor();
#endregion