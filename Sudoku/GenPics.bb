Include "Data.bb"

SeedRnd MilliSecs()
AppTitle = "Make Sudoku"
Graphics 40*9,40*9

Dim a(3)
a(1) = 4
a(2) = 2
a(3) = 9
Global bx=0
Dim Box(2)
For i=0 To 1
	Box(i) = LoadImage("Box"+i+".png")
	If Not Box(i)
		DebugLog "Failied to load: "+"Box"+i+".png"
	Else
		DebugLog "Box: "+i+"; "+Box(i)
	EndIf
Next
;Global font = LoadFont("conthrax-sb.ttf",36)
;SetFont Font
Dim F%(9)
For i=0 To 9
	F(i) = LoadImage("Fnt\"+Int(I+48)+".png") 
	DebugLog "Loaded letter "+i+" at "+F(i)
 	MidHandle F(i)
Next


Function Base()
	Cls
	bx=0
	For y=0 To 8
		If y Mod 3<> 0
				bx = Not bx
		EndIf
		For x=0 To 8
			If x Mod 3 = 0
				bx = Not bx
			EndIf
			DebugLog "Base: ("+x+","+y+") --> Box: "+bx+">"+Box(bx)

			DrawImage Box(bx),x*40,y*40
		Next
	Next
End Function


Dim dax(3) ; unfortunately this cannot be done locally in Blitz Basic			
Dim day(3)
Function Create(skill%,outn%)	
	Local Okay = False
	Local d
	Local dx,dy
	Base
	For i=1 To 3
		Repeat
			dax(i)=Rand(0,8)
			day(i)=Rand(0,8)
		Until (Not Always(dax(i),day(i))) And nummers(dax(i),day(i))=a(i)
		Select i
			Case 1
				Color 255,0,0
			Case 2
				Color 0,255,0
			Case 3
				Color 0,0,255
		End Select 
		If skill<>0 And skill<>4 Then Rect (dax(i)*40)+5,(day(i)*40)+5,30,30
	Next
		
	For y=0 To 8
		For x=0 To 8
			Okay = Always(x,y) Or skill=0
			Okay = okay Or Rand(1,((skill+1)^2))=1
			For i=1 To 3
				Okay = okay And (dax(i)<>x Or day(i)<>y) 
			Next
			Okay = Okay And skill<>4
			Okay = Okay Or skill=0
			If Okay	
				d = nummers(x,y)
				dx = ((x*40)+18) 
				dy = ((y*40)+18) 
				DebugLog d+"> ("+x+","+y+") -> ("+dx+","+dy+")" 
				;Text d,dx,dy ;,True,True
				DrawImage F(d),dx,dy
			EndIf
		Next
	Next
	If Outn		
		Local shot = CreateImage(40*9,40*9)
		Local outfile$ = "Classic\Puz_"+skill+"_"+Right("0"+outn,2)+".bmp" 
		If skill=0
			outfile = "Classic\Solution.png" 
		ElseIf skill=4
			outfile = "Classic\Empty.png" 
		EndIf
		DebugLog "Grapbbing onto "+shot
		GrabImage shot,0,0		
		DebugLog "Saving: "+outfile+" ("+shot+")" 
		SaveImage shot,outfile
		FreeImage shot
	EndIf
	Flip
	Delay 100
End Function 


SetBuffer BackBuffer()


;Create 1
;Flip
;WaitKey
For skill=1 To 3
	For keer=1 To 10
		Create skill,keer
	Next
Next
Create 0,1
Create 4,1
End
