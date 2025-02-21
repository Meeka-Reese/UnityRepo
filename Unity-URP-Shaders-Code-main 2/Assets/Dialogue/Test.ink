INCLUDE globals.ink
{ NoPaint == true: -> Ask | -> Main }
-> Main
=== Main ===
...
Hey, sorry.. do you need something?
    *[Are you doing all good?]
        I'm... fine, How are you doing?
            **[What're you up to?]
                ->pt1
           **[The water's really pretty]
                I know! Waterfalls are the only thing <br>
                that keep me going
                //change
                ->pt1
                
    *What're you up to?
        ->pt1
=== pt1 ===
I'm just kinda visiting some old parks. 
Doing the whole tour de old parks
    * [-You think this is really funny-]
        Hey, uh is everything all good? <br>
            You've just been standing there doing <br>
                a joker laugh for an hour.
            ** [-You keep laughing-]
                ...
                uh, well you keep at it. <br>
                    I'm glad you're having a good time.
                ->END
            ** [Oh sorry, uh anyhoo]
                No don't worry, I was just concerned.
                You just kept standing there like a statue <br>
                    staring ominously and cackling
                ->pt2 
        
    * [-You have no idea what she's saying-]
        Hey, uh is everything all good? <br>
            You've just been standing there silent <br>
                for like 10 minutes.
        ** [-Scream and run away-]
            Okay, uh good talking with you
            ->END
        **[Oh sorry I think I had a mini stroke]
            Don't worry, I love sitting in the quiet by the creak. <br>
                It's really peaceful.
            You should definitly get that checked out though <br>
                That's not normal please don't die.
            ->pt2
    
=== pt2 ===
I don't think I introduced myself. <br>
    My name's Adeline
This is crazy and you can say no.
I used to come here with my ex a while back <br>
    and well, I guess I'm just having a hard time <br>
        finding any peace.
I even tried painting some of the plants and birds <br>
    but everything looks different.
->Ask

=== Ask ===
Could you paint me a picture of all of the pretty flowers?
It's completely okay if this is too much to ask.
    *[-Accept Quest-]
    { PaintTab == true: -> PaintAsk | -> SnowLeopard }
    *[-Don't Accept Quest-]
        Oh, okay well no worries either way. <br>
        Let me know if you change your mind.
    { NoPaint == true }
        ->END
=== SnowLeopard ===
->END
=== PaintAsk ===
->END