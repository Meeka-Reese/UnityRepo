INCLUDE globals.ink
=== InitialChecks ===
{ InitialRunDone == true: ->PTCheck | -> Main }
=== PTCheck ===
{ PaintTab == true: ->NPCheck | ->SnowLeopard }

=== NPCheck ===
{ NoPaint == true: -> Ask | -> PaintAsk }
=== Main ===


...
Hey, sorry.. do you need something?
    *[Are you doing all good?]
        I'm... fine, How are you doing?
        
        
            **[What're you up to?]
                ->pt1
           **[The water's really pretty]
                I know! Waterfalls are the only thing that keep me going
                //change
                ->pt1
                
    *[What're you up to?]
        ->pt1
=== pt1 ===
I'm just kinda visiting some old parks. Doing the whole tour de old parks
    * [-You think this is really funny-]
        Hey, uh is everything all good? You've just been standing there doing a joker laugh for an hour.
            ** [-You keep laughing-]
                ...
                uh, well you keep at it. I'm glad you're having a good time.
                ->END
            ** [Oh sorry, uh anyhoo]
                No don't worry, I was just concerned.
                You just kept standing there like a statue, staring ominously and cackling
                ->pt2 
        
    * [-You have no idea what she's saying-]
        Hey, uh is everything all good? You've just been standing there silent for like 10 minutes.
        ** [-Scream and run away-]
            Okay, uh good talking with you
            ->END
        **[Oh sorry I think I had a mini stroke]
            Don't worry, I love sitting in the quiet by the creak. It's really peaceful.
            You should definitly get that checked out though. That's not normal please don't die.
            ->pt2
    
=== pt2 ===
I don't think I introduced myself. My name's Adeline
This is crazy and you can say no.
I used to come here with my ex a while back, and well, I guess I'm just having a hard time finding any peace.
I even tried painting some of the plants and birds but everything looks different.
->Ask

=== Ask ===
Could you paint me a picture of the purple Iris flowers out by the waterfall?
It's completely okay if this is too much to ask.
    *[-Accept Quest-]
    ~InitialRunDone = true 
 
    ~NoPaint = false
    { PaintTab == true: -> PaintAsk | -> SnowLeopard }
    *[-Don't Accept Quest-]
        Oh, okay well no worries either way. Let me know if you change your mind.
    ~NoPaint = true
    ~InitialRunDone = true 
        ->END
=== SnowLeopard ===
I think I remember the Snow Leopard over the bridge. Talking about loosing their Watercolor book.
I bet if you find it, you could just take it.
Just don't mention it to him. 
Wait can you even see him? Am I going crazy?
Anyways that is what it is.
->END
=== PaintAsk ===
Hey, did you already draw the Iris?
    *[I still need some more time]
        Okay, don't worry. 
        Let me know when you find it.
        ->END
    *[Yep it's all ready]
        Yay, okay which of your paintings are you most proud of?
        ~ImageChoice = true 
            **[Painting 1]
                ~ChosenPaintingNum = 1
                ->END
            **[Painting 2]
                ~ChosenPaintingNum = 2
                ->END
            **[Painting 3]
                ~ChosenPaintingNum = 3
                ->END
->END