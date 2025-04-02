<CsoundSynthesizer>
<CsOptions>
</CsOptions>
<CsInstruments>
; Adapted from Richard Boulanger's toots
sr = 44100
ksmps = 128
nchnls = 2
0dbfs = 1

garvbL init 0
garvbR init 0
gadlyL init 0
gadlyR init 0
gafbL  init 0
gafbR init 0

instr	10	;GUI
	ktrig	metro	100
	if (ktrig == 1)	then
		
		gkscale		invalue	"Scale"
	endif
endin
instr 1
	ktrigger	metro		2
	kspeed lfo 2, .1
	ktrigger2 metro 1 * abs(kspeed)
	ktrigger3 metro 20


	ktrigger4 metro 3 
	ksnareDur = .1
	ktrigger5 metro 1.5, .5
	kdur		randomh		.1,    2,       10		
	kBassdur = kdur
	krandom randomh 0,1,.2
	krandomfast randomh 0,1,3

	krandom2 randomh 0,1,10
	kftb = (krandom < .33 ? 2 : 3)
	kbass = (krandom < .5 ? 6.00 : 6.03)
	if ((krandom > .66) && !(krandom < .33)) then
		kftb = 4
		kbass = 5.05
		endif
		
	if ((krandomfast > .9)) then
	ktrigger4 metro 6
	kBassdur *= .5
	kbass += 1
	endif
	
	
	if ((krandomfast < .05)) then
		ktrigger5 metro 18
		ksnareDur = .1
		endif
	if (krandomfast > .05) then
		ktrigger5 metro 1.5
		ksnareDur = .2	

	endif
	
	
	klfo lfo .7, .2
	kspectral lfo 20, .1

	
;                                               i     start  dur     amp oct car f   rvbam  hpfreq spec   dly
	schedkwhen 	ktrigger,   0,        0,       2,      0,   kdur,    .6, 0, 1, kftb, .1,   300,kspectral, .4
	schedkwhen 	ktrigger2,  0,        0,       2,      0,   kdur * 2, .5, 2, 1, kftb, .1,   300,0,         .2
	schedkwhen 	ktrigger4,  0,        0,       3,      0,   kBassdur + klfo + .5, .5,kbass, 1
	schedkwhen 	ktrigger5,  0,        0,       4,      0,  ksnareDur, .5
	if (krandom2 > .7) then
		schedkwhen 	ktrigger3,  0,        0,       2,      0,   kdur * .05, 0, 4, 1, 4, .7, 4000, kspectral * 2,.2
	endif
	endin


instr 2
	iamp = ampdbfs(p4)  * .1  
	
	iscale = iamp * .1      ; scale the amp at initialization
	krvbadd lfo .5, .1
	krvbam = p8 + krvbadd
	ipchndx	random	0,13     
	ipch		table	ipchndx, p7+i(gkscale)
	aenv		expseg	.5, (p3), .001	
	kfmenv    linen 1, .01, .05, 0
	kping expseg .5, p3/10, .001
	kdly = p11
	

	a1 foscil iamp, cpspch(ipch + p5)  + (kping * 200), p6,1,1, 1
	a2 pluck iamp, cpspch(ipch + p5) + (kping * 200),cpspch(ipch + p5), 1, 3
	a3 pluck iamp, cpspch(ipch/2 + p5) + (kping * 500), cpspch(ipch/2 + p5), 1, 3
	a2 = (a2%(a3 *kping)) * 1.5
	afin = a2 + a1
	afin buthp afin, p9
	klfo random 0, 50
	gadlyL = gadlyL+gafbL
	gadlyR = gadlyR + gafbR
	
	afin = afin * aenv
	fsig pvsanal afin, 1024, 256, 1024,1
	fshift pvshift 	fsig, klfo * p10, 0
	aout pvsynth fshift
	a1,a2 pan2 aout, random(.1,.9)
	vincr garvbL + gadlyL, a1 *  krvbam  
	vincr garvbR + gadlyR, a2 * krvbam
	vincr gadlyR, a1 * kdly
	vincr gadlyL, a2 * kdly

	   
	outs a1, a2

endin

instr 3
iamp = ampdbfs(p4)  * .1  
	

kpch = p5
kcps = cpspch(kpch)
aenv expseg .5, p3, .001

kfilop lfo 2000, .05
kping expseg .5, p3/20, .001
kfilenv linen 500 + abs(kfilop),.05, p3/5, p3/5
klfo lfo .1, 3


afm foscili iamp, kcps + (kping * 400), p6, .1+kping,.1,2 


afm = afm * aenv * .5
afm = chebyshevpoly(afm, kping * 5, kcps,klfo) * .05
afm = chebyshevpoly(afm, klfo, kcps,klfo) * .1
afm = afm + abs(klfo)
afm clip afm, 2, .8
afm = afm * aenv *.3
afm butlp afm,kfilenv +(abs(kfilop) * .1)

outs afm,afm

endin


instr 4
iamp = ampdbfs(p4) * .1

kenv linen 1,0, p3, .1
kenv2 linen 1000,.01,p3/4,.01
kenv3 linen 20, .1, p3/4,.01
kenv4 linen 2, .01, p3/2, .1
kenv5 linen 15000, p3/2,p3/2,1
krand randomh 100, 2000, 50

anoise noise iamp, 0
afm foscil iamp, 100 + krand, 3 + kenv4,2,2,1
afm2 foscil iamp, 400 +  krand, 3 + kenv4,2,2,1
anoise *= afm * 100 * afm2
anoise += (afm / (abs(afm2) + 4))
anoise buthp anoise, 2000 + krand 
anoise *= kenv
anoise butlp anoise, 20000 - kenv5
anoiseL, anoiseR freeverb anoise, anoise, .5, .35, sr, 0
anoiseL alpass anoiseL, 5, .1
anoiseR alpass anoiseR, 5,.1

anoiseL += anoise * .8
anoiseR += anoise * .8

;delay send

vincr gadlyL, anoiseL * .2
vincr gadlyR, anoiseR * .2
outs anoiseL,anoiseR
endin





instr Reverb
krvbadd lfo .5, .1
ktime = .5 + abs(krvbadd)
denorm garvbL
areverb reverb garvbL, ktime
areverb2 reverb garvbR, ktime * 1.3
areverb2 delay areverb2, .1
outs areverb,areverb2
clear garvbL
clear garvbR
endin





instr Delay1 
    ifeedback = 0.3
    ideltime = 0.5   

   gadlyL = gadlyL + gafbL
   gadlyR = gadlyR + gafbR
   
	gadlyL buthp gadlyL, 500
	gadlyR buthp gadlyR, 500
	gadlyL butlp gadlyL, 5000
	gadlyR butlp gadlyR, 5000
   gadlyL delay gadlyL, ideltime + .5
   gafbL = (gafbL + gadlyL) * ifeedback
   gadlyR delay gadlyR, ideltime
   gafbR = (gafbR + gadlyR) * ifeedback
   
   
   

    outs gadlyL, gadlyR
    clear gadlyL, gadlyR
  
endin
instr Delay2
 ;multiple delay times, this one with comb
 	ifeedback = .2
    ideltime = 0.3 
    aosc = .005


   gadlyL = gadlyL + gafbL
   gadlyL delay gadlyL, ideltime
   gadlyR = gadlyR + gafbR
   gadlyR delay gadlyR, ideltime  + .3
   	gadlyL buthp gadlyL, 500
	gadlyR buthp gadlyR, 500
  
  
  gadlyL flanger gadlyL, aosc, .1
  gadlyR flanger gadlyR, aosc, .1

    gafbL = gadlyL * ifeedback
    gafbR = gadlyR * ifeedback
    
 
   

    outs gadlyL, gadlyR
    clear gadlyL, gadlyR
  
endin

 
</CsInstruments>
<CsScore>
; number  time   size  GEN  p1
f   1         0       4096	10	 1
f   2          0      64      -2 	7 	7.03 	7.05 	7.07 	7.10 	8	8.03 	8.05 	8.07 	8.10 	9			;PENTATONIC SCALE - 2 OCTAVES
f   3          0      64      -2 	7.03 	7.07 	7.10 	8 	8.03 	8.07 	8.09 	8.11 	9 	9.03	9.07 9.09 9.10	
f   4          0      64      -2 6.00 6.07 7.00 7.07 8.00 8.07 9 9.07 10 6.00 7.00 8.00 9.00
f   5          0      64      -2 6.05, 6.05, 7, 7.05, 7.09, 8.00, 8.03, 8.05, 8.09, 9.00, 9.03, 9.05, 9.09
;WHO	; use GEN10 to compute a sine wave

;ins strt dur  amp   freq      attack    release
i 10	 0	 3600		;GUI
i 1 0 3600
i "Reverb" 0 3600
i "Delay1" 0 3600
i "Delay2" 0 3600

e ; indicates the end of the score
</CsScore>
</CsoundSynthesizer>






















<bsbPanel>
 <label>Widgets</label>
 <objectName/>
 <x>514</x>
 <y>322</y>
 <width>413</width>
 <height>431</height>
 <visible>true</visible>
 <uuid/>
 <bgcolor mode="background">
  <r>7</r>
  <g>95</g>
  <b>162</b>
 </bgcolor>
 <bsbObject version="2" type="BSBLabel">
  <objectName/>
  <x>14</x>
  <y>12</y>
  <width>100</width>
  <height>29</height>
  <uuid>{a6fa6d02-14cc-4f0c-b2c3-5e3da73375d3}</uuid>
  <visible>true</visible>
  <midichan>0</midichan>
  <midicc>-3</midicc>
  <description/>
  <label>Toot 4</label>
  <alignment>left</alignment>
  <valignment>top</valignment>
  <font>DejaVu Sans</font>
  <fontsize>18</fontsize>
  <precision>3</precision>
  <color>
   <r>241</r>
   <g>241</g>
   <b>241</b>
  </color>
  <bgcolor mode="nobackground">
   <r>255</r>
   <g>255</g>
   <b>255</b>
  </bgcolor>
  <bordermode>noborder</bordermode>
  <borderradius>1</borderradius>
  <borderwidth>0</borderwidth>
 </bsbObject>
 <bsbObject version="2" type="BSBLabel">
  <objectName/>
  <x>14</x>
  <y>43</y>
  <width>365</width>
  <height>114</height>
  <uuid>{80a23d3c-a09d-4c5a-a02f-838a51099125}</uuid>
  <visible>true</visible>
  <midichan>0</midichan>
  <midicc>-3</midicc>
  <description/>
  <label>Next we'll animate the basic sound by mixing it with two slightly de-tuned copies of itself. We'll employ Csound's cpspch value converter which will allow us to specify the pitches by octave and pitch-class rather than by frequency, and we'll use the ampdbfs converter to specify loudness in dB FS rather than linearly.</label>
  <alignment>left</alignment>
  <valignment>top</valignment>
  <font>DejaVu Sans</font>
  <fontsize>12</fontsize>
  <precision>3</precision>
  <color>
   <r>241</r>
   <g>241</g>
   <b>241</b>
  </color>
  <bgcolor mode="nobackground">
   <r>255</r>
   <g>255</g>
   <b>255</b>
  </bgcolor>
  <bordermode>noborder</bordermode>
  <borderradius>1</borderradius>
  <borderwidth>0</borderwidth>
 </bsbObject>
 <bsbObject version="2" type="BSBScope">
  <objectName/>
  <x>50</x>
  <y>276</y>
  <width>292</width>
  <height>96</height>
  <uuid>{2075a23a-bf65-471c-a1c4-f675ed4b806c}</uuid>
  <visible>true</visible>
  <midichan>0</midichan>
  <midicc>-3</midicc>
  <description/>
  <value>-255.00000000</value>
  <type>scope</type>
  <zoomx>2.00000000</zoomx>
  <zoomy>1.00000000</zoomy>
  <dispx>1.00000000</dispx>
  <dispy>1.00000000</dispy>
  <mode>0.00000000</mode>
  <triggermode>NoTrigger</triggermode>
 </bsbObject>
 <bsbObject version="2" type="BSBLabel">
  <objectName/>
  <x>14</x>
  <y>162</y>
  <width>365</width>
  <height>102</height>
  <uuid>{94118d9a-d6aa-485c-81ac-b5148a74545b}</uuid>
  <visible>true</visible>
  <midichan>0</midichan>
  <midicc>-3</midicc>
  <description/>
  <label>Since we are adding the outputs of three oscillators, each with the same amplitude envelope, we'll scale the amplitude before we mix them. Both iscale and inote are arbitrary names to make the design a bit easier to read. Each is an i-rate variable, evaluated when the instrument is initialized.</label>
  <alignment>left</alignment>
  <valignment>top</valignment>
  <font>DejaVu Sans</font>
  <fontsize>12</fontsize>
  <precision>3</precision>
  <color>
   <r>241</r>
   <g>241</g>
   <b>241</b>
  </color>
  <bgcolor mode="nobackground">
   <r>255</r>
   <g>255</g>
   <b>255</b>
  </bgcolor>
  <bordermode>noborder</bordermode>
  <borderradius>1</borderradius>
  <borderwidth>0</borderwidth>
 </bsbObject>
</bsbPanel>
<bsbPresets>
</bsbPresets>
