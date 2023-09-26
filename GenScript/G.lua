function pf(fmt,...)
	print(string.format(fmt,...))
end

for i=1,12 do
	pf("CHUNK ZA_G%d",i)
	pf("	call Score,%d000",i)
	pf("	call VitalBoss,\"Security\",\"\",\"Security\"")
	pf("	call kill,\"G%d\"",i)
	pf("	call kill,\"ZA_G%d\"",i)
end