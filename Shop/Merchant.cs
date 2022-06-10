// Lic:
// Merchant data generator
// Like the name implies
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
// Version: 22.06.07
// EndLic

namespace Shop {
	public class Merchant {
		static Merchant() {
			MKL.Version("Star Story 2 - The Virus Strikes Back - Merchant.cs","22.06.07");
			MKL.Lic    ("Star Story 2 - The Virus Strikes Back - Merchant.cs","GNU General Public License 3");
			RecordLess.Clear();
			RecordData.Clear();
			bool InRec = false;
			string Rec = "";

			foreach (var _ln in QuickStream.LoadLines(MyData)) {
				var ln = _ln.Trim();
				if (ln.ToUpper() == "[RECORDS]") { InRec = true; Rec = ""; } else if (InRec && qstr.Prefixed(ln, "Rec: ")) {
					Rec = qstr.Right(ln, ln.Length - 5).Trim().ToUpper();
					if (!RecordData.ContainsKey(Rec)) { RecordData[Rec] = new StringMap(); }
				} else if (InRec && Rec != "") {
					if (ln.Length > 0 && ln[0] != '#') {
						var p = ln.IndexOf('=');
						var k = ln.Substring(0, p).Trim();
						var v = ln.Substring(p + 1).Trim();
						RecordData[Rec][k] = v;
					}
				} else if (InRec && (ln.Length == 0 || ln[0] == '#')) {
					// Nothing... Just make sure this keeps the 'else' clean
				} else {
					//Console.WriteLine($"InRec:{InRec}; Rec:\"{Rec}\""); // Debug only
					RecordLess.Append($"{ln}\n");
				}
			}
			//Console.WriteLine($"<DEBUG>\n{RecordLess}\n</DEBUG>"); // Debug only
		}

		public const string __mydata = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/Dev/Data/SS2 - Items Actions Abilities.mydata";
		public static string MyData => Dirry.AD(__mydata);

		public const string __outdir = "Scyndi:Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Script/Script/Use/Merchant.uwn";
		public static string OutDir => Dirry.AD(__outdir);
		public string OutFile => $"{OutDir}/{Tag}.neil";

		public static readonly Dictionary<string, Merchant> Map = new Dictionary<string, Merchant>();
		static StringBuilder RecordLess = new StringBuilder();
		static SortedDictionary<string, StringMap> RecordData = new SortedDictionary<string, StringMap>();
		static TMap<string, bool> MModified = new TMap<string, bool>();
		bool Modified { get => MModified[Tag]; set => MModified[Tag] = value; }

		readonly public string Tag;
		readonly public string Name;
		readonly public string Currency;
		bool OnSaleByDefault(string rec) => RecordData[rec][$"{Tag}_ForSaleByDefault"].ToUpper() == "TRUE";
		bool NeverForSale(string rec) {
			try {
				if (!RecordData.ContainsKey(rec)) throw new Exception($"??? Key '{rec}' not present");
				return RecordData[rec][$"{Tag}_NeverForSale"].ToUpper() == "TRUE";
			} catch (Exception e) {
				QCol.QuickError($"Something went wrong when retreiving record {rec} in the NeverForSale database!\n\n");
				QCol.Magenta($"\n\n{e.Message}\n\n");
				return false;
			}
		}
		string Category(string rec) => RecordData[rec][$"{Tag}_Category"];
		public Merchant(string _t, string currency = "Aurina", string _n = "") {
			Tag = _t;
			Name = _n; if (Name == "") Name = $"{_t}'s shop";
			Currency = currency;
			Map[_t] = this;
		}
		public void Run() {
			var Script = new StringBuilder($"\n\n// Generated: {DateTime.Now}\n\nVar _RET = New TMerchant(\"{Tag}\", \"{Name}\", \"{Currency}\")\n\nInit\n");
			foreach (var R in RecordData) {
				int p = R.Key.IndexOf('_');
				var f = R.Key.Substring(0, p);
#if DEBUG
				QCol.Doing(R.Key, "Processing");
#endif
				switch (f) {
					case "JWL":
						if (Category(R.Key) != "Jewels") { R.Value[$"{Tag}_Category"] = "Jewels"; Modified = true; QCol.Doing(R.Key, "Categorized as 'Jewel'"); }
						break;
					case "ITM":
						while (Category(R.Key) == "") {
							QCol.QuickError($"No category set for {R.Key}");
							QCol.Doing("Category", "", "");
							RecordData[R.Key][$"{Tag}_Category"] = Console.ReadLine().Trim();
							Modified = true;
						}
						break;
					default:
						try {
							if (!NeverForSale(R.Key)) {
								R.Value[$"{Tag}_NeverForSale"] = "TRUE";
								QCol.Doing(R.Key, "This is not an item. So it may NEVER be on sale");
								Modified = true;
							}
						} catch (Exception e) {
							QCol.QuickError($"Something went wrong when retreiving record {R.Key} in the NeverForSale database!\n\n");
							QCol.Magenta($"\n\n{e.Message}\n\n");
						}

						break;
				}
				if (!NeverForSale(R.Key)) {
					Script.Append($"\t_RET.C{Category(R.Key)}.AddLast(New TStockItem(\"{R.Key}\", \"{Tag}\", {OnSaleByDefault(R.Key)}))\n");
				}
			}
			Script.Append("End\n\nReturn _RET\n");
			if (!Directory.Exists(OutDir)) {
				QCol.Doing("Creating dir", __outdir);
				Directory.CreateDirectory(OutDir);
			}
			QCol.Doing("Saving", OutFile);
			QuickStream.SaveString(OutFile, Script);
		}

		static void Save() {
			var a = new StringBuilder();
			a.Append(RecordLess.ToString().Trim());
			a.Append($"\n\n[RECORDS]\n\n# This data is generated. Editing this is dangerous!\n# Last generated by the Shop Generator for Star Story II on {DateTime.Now}\n\n\n");
			foreach (var r in RecordData) {
				a.Append($"\n\n\nRec: {r.Key}\n");
				foreach (var k in r.Value.Keys) a.Append($"\t{k} = {r.Value[k]}\n");
			}
			a.Append($"\n\n# Well, that does it! See ya!\n# Time is money, friend!");
			QuickStream.SaveString(MyData, a);
		}

		static public void SavedIfModified() {
			bool any = false;
			foreach (var k in MModified.Keys) {
				var b = MModified[k];
				any = any || b;
				if (b) QCol.Doing("Modified by", k);
			}
			if (any) {
#if DEBUG
				QCol.Doing("Backing up", MyData);
				File.Copy(Merchant.MyData, $"{Merchant.MyData}.Back_{DateTime.Now.ToString().Replace('/', '-').Replace(':', '.')}.mydata");
#endif
				QCol.Doing("Saving", MyData);
				Save();

			}
		}
	}
}