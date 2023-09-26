// Lic:
// Star Story II
// Credits Generator
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

QCol.ColWrite(ConsoleColor.Gray,"");

const string JCRFile = "E:/Projects/Applications/Apollo/Games/Star Story 2/Debug/Star Story 2.jcr";
const string OutScript = "E:/Projects/Applications/Apollo/Games/Star Story 2/src/Tricky Script/Script/Flow/FlowCredits/CreditsData.neil";

void Write(params object[] values) { foreach (var o in values) { Console.Write(o.ToString()); Console.Write(" "); } }
void WriteLine(params object[] values) { foreach (var o in values) { Console.Write(o.ToString()); Console.Write(" "); } Console.WriteLine(); }



WriteLine("Set up Credits");
JCR6_zlib.Init();
var Config = GINIE.FromFile("Credits.ini");
var AC = new TAsk(Config);
Config.AutoSaveSource = "Credits.ini";
var Lijsten = new SortedDictionary<string, List<string>>();

void Credit(string category,string person) {
	if (person == "") return;
	if (AC.Yes($"Cat::{category}", person,$"Credit {person} for {category}")) {
		Console.ResetColor();
		Config.ListAddNew("Categories", "Categories", category);
		if (!Lijsten.ContainsKey(category)) Lijsten[category] = new List<string>();
		if (!Lijsten[category].Contains(person)) {
			Lijsten[category].Add(person);
			Lijsten[category].Sort();
			WriteLine("Credited", person, "for: ", category);
		}
	} else Console.ResetColor();
}

Write("Analysing:", JCRFile,"...");
var JCRRes = JCR6.Dir(JCRFile);
if (JCR6.JCATCH!=null) {
	QCol.Red("Error!\x7\n");
	WriteLine(JCR6.JCATCH.Message);
	return;
}
WriteLine("Ok");

foreach(var entry in JCRRes.Entries) {
	var A = entry.Key.Split('/');
	var author = entry.Value.Author.Trim();
	WriteLine(entry.Value.Entry, "by", author);
	switch (A[0]) {
		case "DATA": Credit("Databases", author); break;
		case "FONTS": Credit("Fonts",author); break;
		case "ID":
		case "SCRIPT":
		case "LIBS":
		case "README.MD":
			Credit("Code", "Jeroen P. Broks"); 
			Credit("Script", author); 
			Credit("Design", "Jeroen P. Broks");
			Credit("Development", "Jeroen P. Broks");
			Credit("Production", "Jeroen P. Broks");
			break;
		case "KTHURA":
			Credit("Map design", "Jeroen P. Broks");
			break;
		case "GFX":
			Credit("Graphics", author);
			break;
		case "AUDIO":
			Credit("Audio effects", author);
			break;
		case "MUSIC":
			Credit("Music", author);
			break;
		case "BOXTEXT":
		case "SCENARIO":
			Credit("Scenario", "Jeroen P. Broks");
			break;
		default:
			QCol.QuickError($"\x7I don't know what {A[0]} is!");
			return;
	}
}


var Output = new StringBuilder(@"
// Generated data

#USE 'Libs/LinkedList'

Class CreditLine
	Static Var Lines
	Static Int SY = 0
	Byte R
	Byte G
	Byte B
	Int Y
	Var Img
	String Caption

	Constructor(O,_R=255,_G=255,_B=255)
		Y = SY
		R = _R
		G = _G
		B = _B
		if Lua.type(O)=='string'
			Caption=O
			SY += 40
		Else
			Img=O
			SY += O.Height
		End
	End
End

Init
	CreditLine.Lines = new LinkedList()
	CreditLine.Lines.AddLast(new CreditLine( Image.LoadNew(""GFX/Intro/StarStory.png"",""STARSTORYLOGO"") ) )
	Creditline.Lines.AddLast(new CreditLine(""Episode II - The virus strikes back!"",255,180,0) )
".Replace('\'','"'));
foreach(var c in Lijsten) {
	Output.Append($"\tCreditLine.Lines.AddLast( new CreditLine(\"\",0,0,0) )\n");
	Output.Append($"\tCreditLine.Lines.AddLast( new CreditLine(\"{c.Key}:\",0,180,255) )\n");
	foreach (var a in c.Value) Output.Append($"\t\tCreditLine.Lines.AddLast( New CreditLine(\"{a}\") )\n");
}

Output.Append($"\tCreditLine.Lines.AddLast( new CreditLine(\"\",0,0,0) )\n");
Output.Append($"\tCreditLine.Lines.AddLast( new CreditLine(\"\",0,0,0) )\n");
Output.Append($"\tCreditLine.Lines.AddLast( new CreditLine(\"Copyright (c) 2022-{DateTime.Today.Year}\",255,180,0) )\n");
Output.Append($"\tCreditLine.Lines.AddLast( New CreditLine(\"Jeroen P. Broks\") )\n");
Output.Append($"End\n\n// Last Generated: {DateTime.Now}\n\n");
try {
	WriteLine("Saving: ", OutScript);
	QuickStream.SaveString(OutScript, Output);
} catch(Exception e) {
	QCol.QuickError(e.Message);
}