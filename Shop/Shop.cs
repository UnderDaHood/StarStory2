// Lic:
// Merchant Data Generator
// It does what the name implies
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
// Version: 22.06.06
// EndLic
global using TrickyUnits;
global using Shop;
global using System.Text;


MKL.Version("Star Story 2 - The Virus Strikes Back - Shop.cs","22.06.06");
MKL.Lic    ("Star Story 2 - The Virus Strikes Back - Shop.cs","GNU General Public License 3");

Dirry.InitAltDrives();
new Merchant("Heijn");
// TODO: Phantasar Merchant

MKL.AllWidth = 120;
QCol.DoingTab = 25;
QCol.Yellow("Shop ");
QCol.Cyan(MKL.Newest);
QCol.Magenta($"\n Copyright Jeroen P. Broks {MKL.CYear(2022)}\n\n");
QCol.White(MKL.All());
Console.WriteLine();
QCol.Doing("Database", Merchant.MyData);
Console.WriteLine();
foreach(var mer in Merchant.Map) {
	QCol.Doing("Processing", mer.Key);
	QCol.Doing("Name", mer.Value.Name);
	QCol.Doing("Currency", mer.Value.Currency);
	mer.Value.Run();
}
Console.WriteLine();
Merchant.SavedIfModified();