<CsoundSynthesizer>
<CsOptions>
-odac
</CsOptions>
<CsInstruments>

sr = 44100
ksmps = 64
nchnls = 2
0dbfs = 1	
gktone = 7.00
gktone2 = 7.00
gipchndx = 0
gipchndx2 = 0
gaRvb init 0
gkrvbAm = 0
gkrvbAm chnget "Reverb"


instr 1
gipchndx random 0, 10
gktone table gipchndx,2
kcps = cpspch(gktone)
aOsc oscil .5, kcps
aOsc = aOsc * p4

vincr gaRvb, aOsc * gkrvbAm
outall aOsc
endin
instr 2
ioverin random 0, 6
ktonePlus table ioverin, 3
kcps = cpspch(gktone + ktonePlus)
aOsc oscil .5, kcps
aOsc = aOsc * p4

vincr gaRvb, aOsc * gkrvbAm
outall aOsc
endin
instr 3
gipchndx random 0, 10
endin
instr 4
gipchndx2 random 0, 10
gktone2 table gipchndx2,2
kcps = cpspch(gktone2)
aOsc oscil .5, kcps
aOsc = aOsc * p4

vincr gaRvb, aOsc * gkrvbAm
outall aOsc
endin
instr 5
ioverin random 0, 6
ktonePlus table ioverin, 3
kcps = cpspch(gktone2 + ktonePlus)
aOsc oscil .5, kcps
aOsc = aOsc * p4

vincr gaRvb, aOsc * gkrvbAm
outall aOsc
endin
instr 6
gipchndx2 random 0, 10
endin
instr Reverb
kRoomSize = 1
kHFDamp = .1
arvbL, arvbR freeverb gaRvb, gaRvb, kRoomSize, kHFDamp
fsig pvsanal arvbL, 1024, 256, 1024,1
	fshift pvshift 	fsig, 30, 0
	arvbL pvsynth fshift
fsig pvsanal arvbR, 1024, 256, 1024,1
	fshift pvshift 	fsig, 30, 0
	arvbR pvsynth fshift



arvbL delay arvbL, 1
outs arvbL, arvbR
clear gaRvb
endin 
</CsInstruments>
<CsScore>
f 2 0 64 -2 6.00 8.00 5.07 6.00 6.04 6.07 6.11 7.02 7.04 7.07
f 3 0 64 -2 1 1.5 2 3 3.5 4 
f0 z
i "Reverb" 0 10000

</CsScore>
</CsoundSynthesizer>




<bsbPanel>
 <label>Widgets</label>
 <objectName/>
 <x>100</x>
 <y>100</y>
 <width>320</width>
 <height>240</height>
 <visible>true</visible>
 <uuid/>
 <bgcolor mode="background">
  <r>240</r>
  <g>240</g>
  <b>240</b>
 </bgcolor>
</bsbPanel>
<bsbPresets>
</bsbPresets>
