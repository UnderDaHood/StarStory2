function pf(f,...)
	print( string.format(f,...) )
end

for foe=1,9 do
	pf("BossFoe[%d]=BOSS%d_Foe;",foe,foe)
	for skill=1,3 do
		pf("BossSkill[%d,%d]=BOSS%d_Skill%d;",foe,skill,foe,skill)
	end
end