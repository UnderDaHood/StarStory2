Const HeroDir$="E:\Projects\Applications\Apollo\Games\Star Story 2\Dev\Pre-Assets\Sprites\"
Const FontDir$="E:\Projects\Applications\Apollo\Games\Star Story 2\Dev\Pre-Assets\fonts\Ryanna\"
Const W = 64
Const H = 64
Global MX = W/2
Global MY = H/2
Global Boss = LoadImage("Boss.png")

Function Max(A,B)
	If A>B
		Return A
	Else
		Return B
	EndIf
End Function

Function G#()
	Return Floor( (W+H) / 4)
End Function

Function Afstand%(X%,Y%)
	Local RHZ1 = Abs( Floor(W/2)-X )
	Local RHZ2 = Abs( Floor(H/2)-Y )
	Local HYP  = (RHZ1^2)+(RHZ2^2)
	Return Floor(Sqr(HYP))
End Function

Function AfstandN(X,Y)
	Return max(0,G()-Afstand(X,Y))
End Function

Function ABreukCol(X%,Y%,C)	
	Return Floor(AfstandN(X,Y)/G()*C)
End Function


Function Back(R%,G%,B%)	
	Cls
	For X=1 To W
		For Y=1 To H
			;DebugLog "Gen("+X+","+Y+") -> BCol "+ABreukCol(X,Y,255)+";  Dist: "+Afstand(X,Y)
			Color ABreukCol(X,Y,R),ABreukCol(X,Y,G),ABreukCol(X,Y,B)
			Plot X,Y
		Next
	Next
End Function

Global Img = CreateImage(W,H)
SetBuffer ImageBuffer(img)

; Templates
DebugLog "Format "+W+"x"+H+";  G="+G()+";" 

Back 0,255,0
SaveImage Img,"Out/TemplatePlayer.bmp"
Back 255,0,0
SaveImage Img,"Out/TemplateEnemy.bmp"
Back 0,0,255
SaveImage Img,"Out/TemplateExtra.bmp"
Back 180,0,255
SaveImage Img,"Out/TemplateJoker.bmp"

Back 80,80,80
SaveImage Img,"Out/Back.bmp" 



; Heroes
Function Hero(H$)
	DebugLog "Generating hero: "+H
	Local HI = LoadImage(HeroDir+H+"\"+H+" - Front.png")	
	Back 0,255,0
	DrawImage HI,MX-(ImageWidth(HI)/2),MY-(ImageHeight(HI)/2)
	SaveImage Img,"Out/Hero_"+H+".png"
	FreeImage HI
End Function
Hero "Klahre" 
Hero "Yorno"
;Hero "Doctor"
;Hero "Ashley"
	
	


; Foes
Function Foe(F)
	DebugLog "Generating Foe: "+F+" ("+Chr(F)+")"
	Local HI = LoadImage(FontDir+F+".png")	
	Back 255,0,0
	DrawImage HI,MX-(ImageWidth(HI)/2),MY-(ImageHeight(HI)/2)
	SaveImage Img,"Out/Foe_"+Chr(F)+".png"
	DrawImage Boss,0,0
	SaveImage Img,"Out/Boss_"+Chr(F)+".png"
	FreeImage HI
End Function

For i=65 To 90
	Foe i	
Next
	

End

