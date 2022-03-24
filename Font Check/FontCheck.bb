Data 3
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Unknown\Fonts\SysFont.jfbf
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Unknown\Fonts\Computerfont.15.jfbf
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Unknown\Fonts\Computerfont.20.jfbf
Data "E:\Projects\Applications\Apollo\Games\Star Story 2\src\Unknown\Fonts\Computerfont.30.jfbf"
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Unknown\Fonts\Computerfont.40.jfbf
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Unknown\Fonts\Computerfont.60.jfbf
Data "E:\Projects\Applications\Apollo\Games\Star Story 2\src\Tricky CC\Fonts\Ryanna.jfbf"
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Brian Kent\Fonts\techover.25.jfbf
Data "E:\Projects\Applications\Apollo\Games\Star Story 2\src\Brian Kent\Fonts\techover.40.jfbf"
;E:\Projects\Applications\Apollo\Games\Star Story 2\src\Brian Kent\Fonts\techover.50.jfbf


Dim Lines$(3)
Lines(1) = "The quick brown fox jumps over the lazy dog!"
Lines(2) = Upper(Lines(1))
Lines(3) = "0123456789"

AppTitle "Font Test"
Graphics 1000,500

Read num

For i=1 To num
	Read FontFile$
	DebugLog "Font #"+i+"/"+num+": "+FontFile
	;bt = WriteFile("Expand"+i+".bat")
	;CreateDir "DC"+i
	;WriteLine bt,"cd D"+i
	;WriteLine bt,Replace("njcr extract '"+FontFile+"' -output DC"+i,"'",Chr(34)) ;+" > d_output"+i+".txt"
	;WriteLine bt,"cd .."
	;CloseFile bt
	;ExecFile "Expand"+i+".bat"
	
	ClsColor Rand(0,50),Rand(0,50),Rand(0,50)
	Cls
	For l=1 To 3
		x=0
		y=l*40
		For ls=1 To Len(Lines(l))
			ac = Asc(Mid(Lines(l),ls,1))
			ACFile$ = "DC"+i+"\"+AC+".png"
			DebugLog "Line "+l+" pos "+pos+" char "+ac+" Line:"+Lines(l)+" File: "+ACFile
			ch = LoadImage(ACFile)
			If ch
				DrawImage ch,x,y
				x = x + ImageWidth(ch)
				FreeImage ch
			Else
				x = x + 32
			EndIf 
		Next
	Next
	Flip
	shot = CreateImage(1000,250)
	GrabImage shot,0,0
	SaveImage shot,"DC_OUT"+i+".bmp"
	FreeImage shot
	WaitKey 
Next
