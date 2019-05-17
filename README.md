TO INSTALL:

(1) Drag contents of GameData folder (currently, only "InterstellarMassAccelerator" folder) into GameData folder in KSP install

(2)  You're done!



TO USE:

- Start with one "Master" Mass Driver unit (the ones with the name "Netherdyne X## Mass Accelerator") and then attach multiple "Network Units" to the TOP of the "Master" unit (that is, to the side of the Mass Driver with the RED band- this is the direction they will fire in).  The Network units will be "slaved" to the master part, and will fire sequentially as a payload passes through the central tube.

- MAKE SURE the Mass Driver tube has some sort of control-unit, such as a probe core or cockpit attached to it so as to allow it to operate (my favorite unit for this purpose is a MechJeb AR202 case- which can be attached radially to the Mass Driver stack to provide control).  Also don't forget to include antennas on the Mass Driver stack if you are playing with CommNet enabled, as you will need to control the Mass Driver stack just like any other vessel.

- Attach LOTS of batteries, solar panels, launch clamps, fuel cells, RTG's, or other sources of electrical power to the Mass Drivers.  They are VERY EC-hungry, and will not function or provide lower-than-expected force if they run out of ElectricCharge during the launch.  The best way to determine how EC a given payload will require is by trial-and-error until you get a sense of it, as each payload will require different amounts of power even for the same ring-stack and power level setting, as heavier payloads pass through the ring more slowly and thus require the ring to fire for longer than lighter payloads (longer payloads will also activate more rings at once, so each ring will fire for longer during the launch- also increasing EC consumption, but leading to a higher exit-velocity.)

- Attach a payload to the bottom of the Mass Driver tube with a decoupler, or otherwise position a payload by the end of the tube where the BLACK band is closest to the craft.

- The ideal payload for a Mass Driver ring is LONG AND THIN like a needle, as this allows you to safely use lower-diameter Mass Drivers, which weigh less, and activates more rings at any given time as the payload passes through the Mass Driver tube (meaning two payloads of equal mass but different length should attain different exit velocities).  More slender payloads also tend to experience less aerodynamic drag once they finally exit the Mass Driver tube, are taken off rails and exposed to drag for the first time (the inner cavity of the Mass Driver tube is assumed to be evacuated of all air, with plasma-windows on each end, much like the real life StarTram design this mod is based on).

- After the payload is positioned below the Mass Driver, and has been SEPERATED as a distinct vessel if it was originally pary of the same craft as the Mass Drivers, "Arm" the Mass Driver stack through the right-click context menu on the master part of the stack (which you should ALWAYS locate on the LOADING END of a tube with Network Units, never somewhere in the middle or on the firing-end).

- Select to either "Fire" the Mass Driver stack instantly, or "Start Countdown" to initiate a 10-second countdown after which the Mass Driver will automatically fire regardless of whether you still have it actively selected (giving you time to switch back to your payload-vessel).

- If you followed these instructions carefully, and thus built and positioned your Mass Driver stack and payload correctly, your payload should now fire, potentially with VERY HIGH velocity, through the stack in the direction of the BLACK bands on the rings.  The speed at which your payload will exit the Mass Driver stack is determined solely by the ratio of its length to its mass.



TIPS:

- Beware that aerodynamic drag can quickly incinerate vessels accelerated to very high speeds at low altitudes via mass drivers- so make sure to use heat-resistant parts on the nose of your payload or limit the power-levels on the Mass Drivers so that the exit velocity is within safe limits based on atmospheric drag.  It is not unheard of for parts of a vessel with low heat-tolerance to explode the moment the vessel exits the Mass Driver tube (and is exposed to drag) due to aerodynamic overheating.

- Drag can also cause an aerodynamically-unstable payload to VERY quicky flip around after exiting the Mass Driver tube, hitting its nose on the firing-end of the Mass Driver and causing some players to mistakenly think their payload never made it out of the Mass Driver in the first place.  Placing most of your mass in the nose of your payload, using sharp nosecones and interstage adapters, and making use of fins at the base of less stable payloads are all *HIGHLY* recommended.

- Mass Drivers work on more locations than just Kerbin- they CAN AND SHOULD be flown to (or built on-site out of local materials using other mods such as Extraplanetary Launchpads) locations like the Mun, where you can position vessels you want to launch back to orbit below them- they even work at an angle relative to the horizon as they place vessels on rails while the tube of Mass Drivers is firing.  For a demonstration of use of an earlier (over-powered) version of this mod's Mass Drivers on the Mun, see Scott Manley's video on the subject on YouTube:

https://youtu.be/XeaTQzXpR7M


- The longer a vessel is, the more rings can accelerate it simultaneously.  The heavier the vessel is, the less velocity the Mass Drivers will impart to your payload as they exert a CONSTANT FORCE on the payload as it passes through the ring.  Longer rings impart more velocity to a payload, but the force exerted by a stack of mass drivers is UNRELATED to the diameter of the rings (although wider-diameter rockets will necessarily weigh more relative to their length, and thus attain lower speeds).  Individual larger-diameter Mass Driver parts are also longer, however, and will thus each impart more force to the payload: however a tube of the same length of larger-diameter rings will necessarily contain fewer parts.

- Be warned that due to the physics of electromagnetic forces, larger-diameter Mass Drivers have to carry a larger current to make up for the increased diameter of the inner cavity, and thus are much heavier for the force they provide.  This is balanced by the utility of being able to safely launch larger-diameter parts, and, in KSP, of being able to construct a tube of the same length with a lower part-count.  Otherwise, it is always better to rely on lower-diameter Mass Driver rings (and possible multiple launches or lighter payloads) wherever possible as they cost and weigh less, and are easier to move to locations other than the Launchpad.

- If you Mass Drivers are not working as expected, *MAKE SURE YOU HAVE ENOUGH ELECTRICAL POWER*.  One of the most common reasons for launch-failures and "bugs" using the Mass Drivers, or lower-than-expected exit velocity, is that the Mass Drivers run out of ElectricCharge partway through the launch, causing them to randomly shut down in the middle of the payload passing through the tube (NOT GOOD, as it will take the vessel off rails) or impart less than their maximum force to the payload.



KNOWN ISSUES:

- The "Master" Mass Accelerator parts will fire in the REVERSE of the normal/correct direction when used without any attached "Network units".  That is, they will fire in the direction of the BLACK band on the Mass Driver instead of the RED band.  To rotate a lone Mass Driver part to fire UPWARDS from the Launchpad, rotate the part 180 degrees so the BLACK band faces up.  However when a Mass Driver has ATTACHED NETWORK UNITS it will fire in the correct direction, towards the RED band instead of the BLACK band.  To fire a tube of Mass Drivers UPWARDS from the Launchpad, stack them with the RED band on top- that is, do NOT rotate them from their default rotation.