```
+                                                                                                   
+                                                                                                   
+                                                                         s/`    `:y                
+                                                                        .yh/:oo-/hs-               
+                                                                        `hy/-dy-:hy.               
+                                                                         +-./Mo  -o                
+                                                             `...........-..dys-..                 
+                                                `.........--..`   `        y/oo ``--..+++++`       
+                                          `oyssyshhsoooyyy+++oyy++++yy-   +d oo `ohoomMMMMMy       
+                                         /y:+y..yo:h:`sy.yo`:h:sy..ss-so`.doyso:y:/h+MMMMMMM-      
+                                       `yd+//sddy//omhh//+hhmo//yddy+/+hddy/yNmd+//oNNNNNNNNs      
+                                        ....::..+................../N-../MMmsys.............`      
+                                            -.  /                  :m...:MMysNo                    
+                                           /ssooy:                 :MNmmNMMd:so                    
+     ./ossso/.                             MMMMMMd                 :MMMMMMMh-so                    
+     `.+oso+-`                             dmmmmmy                 `+ssssNo:sNo                    
+        :y-                                `.....`                       N+o+yo                    
+    `---/M:                                                              Nh/`oo                    
+    +NNNNMN+                                                             N./smo                    
+    sMMMMMMo                                                             N-+sho                    
+    sMNNNMMdss.   `.....` `.....  .....` `.....`                         Nh: oo                    
+    sM--:MMMMMo   yNNNNNh-mNmmmm++mmmmmm.hmmmmmh                         N:ooho                    
+    sM---MMMMMs   mMMMMMN:MMMMMMosMMMMMM-mMMMMMm                         N`-smo                    
+    yMMMMMMMMMs   /yyyyy+`ssssss--syyyys`+yyyyy+                         Ny+.oo                    
+  `.yMMMMMMMMMs.` dMMMMMm-MMMMMMosMMMMMM.dMMMMMd   ``````                Nos/so                    
+  hNMMMMMMMMMMMNm dMMMMMm:NMMMMMosMMMMMN.mMMMMMm .smNNNNm:               N .sNo                    
+  :MMMMMMMMMMMMMM/+ooooo+:ooosss//ssssso:+ooooo+yNMMMMNy-                Nss:so                    
+   yMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNs-              ````Nsy/ss````                
+   .NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNo.               .dNNNMNNMMMNNNm:               
+    +MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMm+`                :hMMMMMMMMMMMMMMds.             
+    `dMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMm+`                  hMMMMMMMMMMMMMMMMM+             
+mmmmmNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNmmmmmmmmmmmmmmmmmmmmmMMMMMMMMMMMMMMMMNmmmmmmmmmmmmmm
+MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
+MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
```

# PortSimulation
## SlutUppgift "Port Administration"
Last Updated: 3/11/20 19:52

## Table of contents
* [About the program](#about-the-program)
* [About the Simulation](#about-the-Simulation)
* [Check out points](#check-out-points)
* [Setup](#setup)

## About the program
I started this program as a console project. Once the main logic was achieved I decided to move
towards a "Windows Presentation Foundation" version. It did present some challenges, but at the
same time, it simplified some others.

## About the Simulation
I have been thinking whether or not to use a Timer to move forward in time, like I did with the
"Tjuv och Polis" project, but finally I decided to use a simple button to click to move to the next
day. I believe that it will be simpler this way to have enough time to read the screen to
understand how the simulation is being handled.

Another decision was whether to keep constantly writing to disk, after each new day automatically,
or allow the user to save at will by giving him/her the option with a "Save" button. I took the
latter option. But I implemented the Save function as a "Save Status ()" method. So in all fairness
to get the "constantly save" functionality, it would be a simple matter of calling the method from
within the "Next Day" button.

## Check out points
* Each day comes to port a random number of boats. From 1 to a maximum of 15.
* The boats are placed on the docks as effectively as possible.
* One dock can guest two row boats.
* No boat is moved once placed.
* The places first available are the ones managed first.
* Implemented Save to file functionality.
* Implemented Load from file functionality.
* Boats that cannot be allocated at the docks are rejected and saved into a List.

## Setup
To run this project, install it locally runing from the command line:

```
$ git clone https://github.com/Julio-German-Gutierrez/PortSimulation.git
```
