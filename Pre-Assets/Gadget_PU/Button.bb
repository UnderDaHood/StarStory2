; Init
AppTitle "Gadget Power Ups"
Graphics 300,100 
SetBuffer BackBuffer()

; Declarations and defintions of globals
;Global fnt = LoadFont("font/dodger3_1laserital.otf",20) DebugLog "Font loaded at "+fnt
;SetFont fnt


Function Max(a,b)
	If A>b
		Return A
	Else
		Return B
	EndIf
End Function


; Font
Dim LChars(256)
Dim IChars(256,30,30)
Dim WChars(256)
Dim HChars(256)

Function GetLetter(GCh%)
	Local x
	Local y
	Local P
	Local d$
	If GCH=32
		WChars(gch) = 10
		HChars(gch) = 10
		Return
	EndIf
	If Not LChars(GCh)	
		DebugLog "Loading char: "+GCh
		Local Img = LoadImage("Chars\"+GCh+".png")
		Local IB  = ImageBuffer(img)
		DebugLog "Img: "+Img+"; Buf: "+IB
		WChars(gch)=ImageWidth(Img)
		HChars(gch)=ImageHeight(Img)
		DebugLog "Size "+WChars(gch)+"x"+HChars(GCh)
		LockBuffer IB
		For y=0 To hChars(GCh)-1
			D=""
			For x=0 To wChars(GCh)-1
				P = ReadPixelFast(x,y,IB) And $ffffff
				If P D = D + "X" Else D = D + " "
				IChars(GCh,x,y)=P
				;DebugLog "("+X+","+Y+") >> "+P
			Next
			DebugLog Right("0"+y,2)+"> '"+D+"'"
		Next
		UnlockBuffer IB
		FreeImage Img		
		DebugLog "Freed: "+Img
		LChars(GCh)=True
	EndIf
End Function

Function TW%(T$)
	Local ret%
	For i=1 To Len(T)
		Local wch = Asc(Mid(T,i,1))
		GetLetter wch
		ret = ret + WChars(wch)
	Next
	Return ret
End Function

Function TH(T$)
	Local ret%
	For i=1 To Len(T)
		Local wch = Asc(Mid(T,i,1))
		GetLetter wch
		ret = max(ret,HChars(wch))		
	Next
	Return ret
End Function
	

Function DT(T$,X,Y)
	; Declarations ;
	Local CX=X
	Local CY=Y
	Local PX
	Local PY	
	; Text
	For i=1 To Len(T)
		Local ch = Asc(Mid(T,i,1))
		GetLetter ch
		For PX=0 To WChars(ch)
			For PY=0 To HChars(ch)
;				DebugLog "Pix("+PX+","+PY+") > "+IChars(ch,px,pY)+" ("+Ch+")"
				If IChars(Ch,px,pY) 
					Plot PX+CX,PY+CY
;					DebugLog "Plot ("+PX+","+PY+")"
				EndIf
			Next
		Next
		CX = CX + WChars(ch)	
	Next
	
End Function




; Functions
Function But(T$,R,G,B)
	Cls
	DebugLog "Creating button: "+T
	Color R,G,B
	Rect 0,0,TW(T)+10,TH(T)+10
	Color R/3,G/3,B/3
	Rect 2,2,TW(T)+6,TH(T)+6
	Local gI = CreateImage(TW(T)+10,TH(T)+10)
	GrabImage gi,0,0
	If Upper(T)<>"CANCEL" And Upper(T)<>"OKAY"
		SaveImage gi,"out\"+T+"_False.bmp"
	EndIf
	Color R,G,B
	;Text 5,5,t
	DT t,3,3
	GrabImage gi,0,0
	SaveImage gi,"out\"+T+"_True.bmp"
	FreeImage gi
	Flip
End Function


Function Main()
	But "Spread",255,255,0
	But "2x Power",255,0,0
	For i=1 To 30
		But "+"+Right("0"+i,2)+" cards",0,180,255
	Next		
	But "Okay",0,255,0
	But "Cancel",255,0,0
	;WaitKey
End Function

Main
End