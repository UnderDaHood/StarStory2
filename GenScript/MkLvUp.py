"""   -- Start License block
  MkLvUp.py
  Version: 16.07.26
  Copyright (C) 2015, 2016 Jeroen Petrus Broks
  
  ===========================
  This file is part of a project related to the Phantasar Chronicles or another
  series or saga which is property of Jeroen P. Broks.
  This means that it may contain references to a story-line plus characters
  which are property of Jeroen Broks. These references may only be distributed
  along with an unmodified version of the game. 
  
  As soon as you remove or replace ALL references to the storyline or character
  references, or any termology specifically set up for the Phantasar universe,
  or any other univers a story of Jeroen P. Broks is set up for,
  the restrictions of this file are removed and will automatically become
  zLib licensed (see below).
  
  Please note that doing so counts as a modification and must be marked as such
  in accordance to the zLib license.
  ===========================
  zLib license terms:
  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.
  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:
  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.
       -- End License block   """

#!/usr/bin/python



from math import *


#heroes =    ('Wendicka','UniCrystal','Crystal','Briggs' ,'ExHuRU' ,'Yirl'   ,'Foxy'   ,'Xenobi' ,'Rolf',    'Johnson')
#herocolor = ('#ff0000' ,'#00ffaa',   '#00ffaa','#ffaa00','#ffaa00','#aa00ff','#ff7700','#00aaff','#ff5500', '#888888')

heroes    = ("Klahre","Yorno","Doctor","Ashley")
herocolor = ("#00b4ff","#00ff00","#ff00ff","#ffff00")


def herocolf(h):
	for ak in range(0,10):
		if heroes[ak]==h: return herocolor[ak]



#Irravonia's original stats
"""function setupchar.Irravonia()
char.Irravonia = 
  {
  ["Name"] = "$IRRAVONIA",
  ["Strength"] = 10,
  ["Defense"] = 9,
  ["Intelligence"] = 15,
  ["Resistance"] = 14,
  ["Accuracy"] = 10,
  ["Evasion"] = 5,
  ["Agility"] = 8,
  ["Experience"] = Math.Rand(10,20),
  ["Level"] = 1,
  ["HP"] = {35,35},
  ["Abilities"] = {"Flame","Breeze","Heal","LittleRock"},
  ["AblTime"] = {10,10,10,10},
  ["SkillNames"] = {"Wand Waving Skills","Fire Magic","Wind Magic","Water Magic","Earth Magic"},
  ["SkillLevels"] = {1,1,1,1,1},
  ["SkillExperience"] = {0,0,0,0,0}, 
  ["SkillFP"] = {0,1,1,1,1}  
  }
end"""

#Basic Level 1 stats

statlv1 = {
"""
   'Wendicka'  : {'Strength': 15, 'Defense': 10, 'Will': 10, 'Resistance': 10, 'Agility':  7, 'Accuracy': 12, 'Evasion':  3, 'HP': 50, 'AP':15},
   'UniCrystal': {'Strength': 15, 'Defense': 10, 'Will': 10, 'Resistance': 10, 'Agility':  7, 'Accuracy': 12, 'Evasion':  3, 'HP': 50, 'AP':0 },
   "Crystal"   : {'Strength': 30, 'Defense':  4, 'Will':  5, 'Resistance': 10, 'Agility' : 1, 'Accuracy': 15, 'Evasion':  3, 'HP': 55, 'AP':0 },
   "Briggs"    : {'Strength': 15, 'Defense': 10, 'Will': 10, 'Resistance': 10, 'Agility':  7, 'Accuracy': 12, 'Evasion':  3, 'HP': 50, 'AP':0 },
   'ExHuRU'    : {'Strength': 40, 'Defense': 40, 'Will':  0, 'Resistance':  0, 'Agility':  1, 'Accuracy':  1, 'Evasion':  0, 'HP': 90, 'AP':0 },
   'Yirl'      : {'Strength': 20, 'Defense': 15, 'Will':  7, 'Resistance':  6, 'Agility':  5, 'Accuracy': 30, 'Evasion':  5, 'HP': 60, 'AP':5 },
   'Foxy'      : {'Strength': 10, 'Defense':  0, 'Will': 12, 'Resistance': 15, 'Agility': 10, 'Accuracy': 90, 'Evasion': 25, 'HP': 20, 'AP':10},
   'Xenobi'    : {'Strength': 30, 'Defense':  5, 'Will': 20, 'Resistance': 25, 'Agility':  9, 'Accuracy': 90, 'Evasion': 25, 'HP': 40, 'AP':20},
   'Rolf'      : {'Strength': 40, 'Defense': 40, 'Will':  0, 'Resistance':  0, 'Agility':  1, 'Accuracy':  1, 'Evasion':  0, 'HP': 90, 'AP':0 },
   'Johnson'   : {'Strength': 40, 'Defense': 40, 'Will':  0, 'Resistance':  0, 'Agility':-10, 'Accuracy':  1, 'Evasion':  0, 'HP': 90, 'AP':0 },
   """:{},
   "Klahre":     {'Power':30, 'Defense': 30, 'Will':30, 'Resistance': 30, 'Accuracy':30, 'Evasion':30, 'HP':30, 'Critical': 1,"Speed":30 },
   "Yorno":      {'Power':20, 'Defense': 10, 'Will':50, 'Resistance': 45, 'Accuracy':20, 'Evasion':10, 'HP':10, 'Critical': 0,"Speed":10 },
   "Doctor":     {'Power': 0, 'Defense':  4, 'Will':90, 'Resistance': 90, 'Accuracy': 1, 'Evasion': 1, 'HP': 1, 'Critical': 0,"Speed":2 },
   "Ashley":     {'Power':40, 'Defense': 40, 'Will':10, 'Resistance':  5, 'Accuracy':80, 'Evasion':12, 'HP':60, 'Critical': 4,"Speed":1 },
}   

#Basic Level 100 stats
statlv100 = {
"""
   'Wendicka'  : {'Strength':450, 'Defense':180, 'Will':295, 'Resistance':160, 'Agility': 70, 'Accuracy': 40, 'Evasion': 20, 'HP':3600 , 'AP':480},
   'UniCrystal': {'Strength':440, 'Defense': 80, 'Will':200, 'Resistance':100, 'Agility': 55, 'Accuracy': 80, 'Evasion': 22, 'HP':3400 , 'AP':0}, # Uniform Crystal is just as strong as Wendicka, as she has not yet become a Cyborg.4
   'Crystal'   : {'Strength':500, 'Defense':100, 'Will':180, 'Resistance':100, 'Agility':100, 'Accuracy':100, 'Evasion': 18, 'HP':4000 , 'AP':0},
   'Briggs'    : {'Strength':455, 'Defense':190, 'Will':295, 'Resistance':170, 'Agility': 80, 'Accuracy': 50, 'Evasion': 25, 'HP':4000 , 'AP':0},
   'ExHuRU'    : {'Strength':700, 'Defense':222, 'Will':190, 'Resistance': 60, 'Agility': 55, 'Accuracy': 80, 'Evasion':  6, 'HP':6000 , 'AP':340},
   'Yirl'      : {'Strength':370, 'Defense':150, 'Will':270, 'Resistance':120, 'Agility': 65, 'Accuracy':130, 'Evasion': 40, 'HP':4500 , 'AP':380},
   'Foxy'      : {'Strength':350, 'Defense':100, 'Will':230, 'Resistance':150, 'Agility':140, 'Accuracy':900, 'Evasion': 80, 'HP':2000 , 'AP':400},
   'Xenobi'    : {'Strength':410, 'Defense':112, 'Will':300, 'Resistance':200, 'Agility':110, 'Accuracy':500, 'Evasion': 65, 'HP':2500 , 'AP':888},
   'Rolf'      : {'Strength':460, 'Defense':195, 'Will':220, 'Resistance': 70, 'Agility': 60, 'Accuracy': 85, 'Evasion': 16, 'HP':4500 , 'AP':340},
   'Johnson'   : {'Strength':455, 'Defense':185, 'Will':240, 'Resistance':100, 'Agility': 61, 'Accuracy': 90, 'Evasion': 18, 'HP':3950 , 'AP':360},
"""   : {},
   "Klahre":     {'Power':300, 'Defense': 300, 'Will':300, 'Resistance': 300, 'Accuracy':300, 'Evasion':300, 'HP':3000, 'Critical': 3,  "Speed":300  },
   "Yorno":      {'Power':190, 'Defense': 200, 'Will':500, 'Resistance': 450, 'Accuracy':200, 'Evasion': 75, 'HP':2000, 'Critical': 1,  "Speed":310  },
   "Doctor":     {'Power':  0, 'Defense':  40, 'Will':900, 'Resistance': 600, 'Accuracy':  1, 'Evasion': 10, 'HP':4000, 'Critical': 0,  "Speed":200  },
   "Ashley":     {'Power':999, 'Defense': 800, 'Will':100, 'Resistance': 100, 'Accuracy':999, 'Evasion':200, 'HP':5000, 'Critical': 10, "Speed":190 },


}
   
   
#stats = ('Strength','Defense','Will','Resistance','Agility','Accuracy','Evasion','HP',"AP")
stats = ("Power","Defense","Will","Resistance","Speed","Accuracy","Evasion","HP")



bth = open('MkLvUpTable.html','w')
bth.write('<pre style="color:#ee8833; background-color:#000000">')   
#Now let's test out if how the damage would be if a character hits himself/herself and the others.
for ch in heroes:
	print('\nTest: %s'%ch)
	for ch2 in heroes:
		Str = statlv1[ch ]['Power']
		Def = statlv1[ch2]['Defense']/2
		MinDmg = (Str+1)-(Def+1)
		MaxDmg = (Str+(Str/2))-(Def+(Def/5))
		print(     'Lv   1> When %s hits %s, minimal damage is %i and maximal damage is %i'  %(ch,ch2,MinDmg,MaxDmg))
		bth.write('Lv   1> When %s hits %s, minimal damage is %i and maximal damage is %i\n'%(ch,ch2,MinDmg,MaxDmg))
		Str = statlv100[ch ]['Power']
		Def = statlv100[ch2]['Defense']/2
		MinDmg = (Str+1)-(Def+1)
		MaxDmg = (Str+(Str/2))-(Def+(Def/5))
		print(     'Lv 100> When %s hits %s, minimal damage is %i and maximal damage is %i'  %(ch,ch2,MinDmg,MaxDmg))
		bth.write('Lv 100> When %s hits %s, minimal damage is %i and maximal damage is %i\n'%(ch,ch2,MinDmg,MaxDmg))
		Str = statlv1[ch ]['Will']
		Def = statlv1[ch2]['Resistance']/2
		MinDmg = (Str+1)-(Def+1)
		MaxDmg = (Str+(Str/2))-(Def+(Def/5))	
		print(     'Lv   1> When %s hits %s with a spell, minimal damage is %i and maximal damage is %i'  %(ch,ch2,MinDmg,MaxDmg))
		bth.write('Lv   1> When %s hits %s with a spell, minimal damage is %i and maximal damage is %i\n'%(ch,ch2,MinDmg,MaxDmg))
		Str = statlv100[ch ]['Will']
		Def = statlv100[ch2]['Resistance']/2
		MinDmg = (Str+1)-(Def+1)
		MaxDmg = (Str+(Str/2))-(Def+(Def/5))
		print(     'Lv 100> When %s hits %s with a spell, minimal damage is %i and maximal damage is %i'  %(ch,ch2,MinDmg,MaxDmg))
		bth.write('Lv 100> When %s hits %s with a spell, minimal damage is %i and maximal damage is %i\n'%(ch,ch2,MinDmg,MaxDmg))
		print( '')
		bth.write('\n')
	bth.write('</pre><p>\n\n')
	
#Let's now get into te basic values, everybody.....
print('\n\n\nWriting tables!')
bth.write('<table>')
for ch in heroes:
	print ('Processing: %s'%ch)
	#btd = open('../../../JCR/Tricky Story/Data/LvStats/%s'%ch,'w')
	#btd = open("../../JCR/demo/data/Tricky Story/Data/CharStats/%s"%ch,'w')
	btd='niks'
	bth.write('\n<tr valign=top><td colspan=10 style="background-color: %s; font-family:Arial; font-size:20pt">%s</td></tr>\n'%(herocolf(ch),ch))
	#btd.write('REM Generated by MkLvUp (Python script by Jeroen P. Broks)\n\n')
	bth.write('<tr valign=top><td>Level</td>')
	for st in stats: bth.write('<td>%s</td>'%st)
#    bth.write('</tr>\n</tr>')
#    for st in stats:
#        btd.write('STAT_%s'%st)	    
#	for lv in range(1,251):
#	    bth.write('<tr valign=top><td>%i</td>'%lv)
    #for lv in range(1,10001):
	for lv in range(1,100001):
		bth.write('<tr valign=top><td>%i</td>'%lv)
		lvs = "%s"%lv
		"""
     if len(lvs)>2 and lvs[-2:]=="00":
         btd.close()
         nf = floor(lv/100)
         btd = open('../../../JCR/Tricky Story/Data/LvStats/%s/%i'%(ch,nf),'w')	 	 
     """    
		if lv % 500==1:
			if lv!=1:
				btd.close()
			btd = open("../../src/Tricky Story/Data/Characters/%s/lvdata/%d"%(ch,floor(lv/500)),'w')
			btd.write('REM Generated by MkLvUp (Python script by Jeroen P. Broks)\n\n')
		btd.write('LEVEL %i\n'%lv)
		for st in stats:
			prc = 0
			st1 = statlv1[ch][st]
			st2 = statlv100[ch][st]
			chg = (st2-st1)/float(100)
			val = st1 + int(chg*lv)
			#prc = float(st1)/float(st2)
			#val = st1 + (st1*prc)*lv
			if lv==1:   val=st1
			if lv==100: val=st2
			bth.write(' <td align=right>%i</td>\n'%val)
			btd.write('STAT.%s %i\n'%(st,val))
			#btd.write('REM STAT.%s %i    -- percent = %f; lv = %i;  st1 = %i;  st2 = %i\n'%(st,val,prc,lv,st1,st2))	    
			bth.write('</tr>\n')
btd.close()
bth.close()
    
