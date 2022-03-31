// Lic:
// SS2MapData
// Behavior Edit
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
// Version: 22.03.31
// EndLic

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Tricky_Apollo;
using TrickyUnits;
using UseJCR6;

namespace SS2MapData {
	static class Behavior {
		public static System.Windows.Controls.TextBox SourceBox;

		static public string source /*{
			get => SourceBox.Text;
			set { SourceBox.Text = value; }
		}*/{
			get => KthuraData.Current.BehaviorSource;
			//set { KthuraData.Current.BehaviorSource = value; }
		}

		public const string Basis = @"; Just make sure the absolute basis is present
; When finding more chunk commands with the same data, the AQS compiler will
; Just start merging, no problem, as long as you know what you are doing!

Chunk Load	
Chunk Update
Chunk PreDraw
Chunk PostDraw
";

		//static public bool box = true;

		static public void Compile(KthuraData KD,TJCRCreate J,string f,bool box) {
			var source = KD.BehaviorSource;
			var bstream = J.NewEntry("Behavior");
			var compiled = AQSCompiler.Compile(source,f);
			var qstream = new QuickStream(bstream.GetStream);
			var cors = source.Split('\n');
			var corb = new StringBuilder();
			foreach(var c in cors) {
				var ct = c.Trim();
				if (ct == "")
					corb.Append("\n");
				else if (ct[0] == ';' || qstr.Prefixed(ct.ToUpper(), "CHUNK") || qstr.Prefixed(ct.ToUpper(), "SCHUNK") || qstr.Prefixed(ct.ToUpper(), "CSCHUNK") || qstr.Prefixed(ct.ToUpper(), "INCLUDE"))
					corb.Append($"{ct}\n");
				else
					corb.Append($"\t{ct}\n");
			}
			Directory.CreateDirectory(qstr.ExtractDir(f));
			QuickStream.SaveString(f,$"{corb}");
			//QuickStream.SaveString(f, source);
			//Debug.WriteLine($"<src>\n{source}</src>\n<corb>{corb}\n</corb>");
			compiled.WriteOut(qstream);
			qstream.Close();
			bstream.Close();
			if (!compiled.Success) {
				try {
					if (box)
						Confirm.Annoy($"Compiling failed\n\n{compiled.Error.Melding}\n\nFile:{compiled.Error.File}\nLine: {compiled.Error.Regel}", $"Compiling {f} failed!", MessageBoxIcon.Error);
					else
						Debug.WriteLine($"nCompiling failed\n\n{ compiled.Error.Melding}\n\nFile: { compiled.Error.File}\nLine: { compiled.Error.Regel}");
				} catch (Exception huh) {
					Debug.WriteLine($".NET Error! Compilation error could not be shown >> {huh.Message}\n\nCompiling failed\n\n{ compiled.Error.Melding}\n\nFile: { compiled.Error.File}\nLine: { compiled.Error.Regel}");

				}
			}
		}

	}
}