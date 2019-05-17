using System;
using UnityEngine;

namespace plugintutorial1
{
    public class ModuleMassAccelerator: PartModule
    {
        [KSPField(isPersistant = false, guiActive = false)]
        public string LoadingpointName;
        [KSPField(isPersistant = false, guiActive = false, guiName="Accelerator force", guiUnits = "KiloNewtons")]
       
        public float acceleratorForce; //in kilonewtons
        [KSPField(isPersistant = false, guiActive = true, guiName = "Accelerator force", guiUnits = "KiloNewtons")]

        public float acceleratorDisplayForce; //in kilonewtons
        [KSPField(isPersistant = true, guiActive = true, guiName = "Power Level", guiUnits ="percent")]
        [UI_FloatRange(stepIncrement = 0.5f, maxValue = 100f, minValue = 0f)]
        public float powerLevel = 100.0f;
        [KSPField(isPersistant = false, guiActive = false)]
        public float accelerationDuration;
        [KSPField(isPersistant = false, guiActive = false)]
        public float NetworkRange = 500.0f;
        

        [KSPField(isPersistant = false, guiActive = false)]
        public string armedAnimation;

        [KSPField(isPersistant = false, guiActive = false)]
        public float loadDistance = 2.0f; //in meters

        [KSPField(isPersistant = true, guiActive = true, guiName = "Status")]
        public string status;
       //  [KSPField(isPersistant = true, guiActive = false)]
        public bool armed = false; //when armed is set to true by the event the vessel inside the load point zone will be "loaded", brought to the center of the magnet, and oriented up
         
        [KSPField(isPersistant = false, guiActive = false)]
        public bool master;  

        [KSPField(isPersistant = false, guiActive = false)]
        public float resourceAmount;
        [KSPField(isPersistant = false, guiActive = false)]
        public string resourceName;
        //[KSPField(isPersistant = true, guiActive = false)]
        public Guid LaunchTarget = Guid.Empty;

        public Vessel vLaunchTarget;
        double firetime = 999;
        double disarmTimeout = 10;
        bool enableShutdowntimeout = false;
        bool firing = false;
        double countdownTime = 999;
        bool countdown = false;

        void armNetwork(Vessel v)
        {
            var massAccelerators = v.FindPartModulesImplementing<ModuleMassAccelerator>();
            print("Network found modules " + massAccelerators.Count.ToString());
            foreach (ModuleMassAccelerator a in massAccelerators)
            {
                print("module is master " + a.part.partName + a.master.ToString());
                if (a.master == false)
                    a.ArmAccelerator();
            }

        }
        void DisarmAcceleratorTimeout()
        {
            enableShutdowntimeout = true;
            disarmTimeout = 10;
        }
        void DisarmNetwork(Vessel v)
        {
            var massAccelerators = v.FindPartModulesImplementing<ModuleMassAccelerator>();
            foreach (ModuleMassAccelerator a in massAccelerators)
            {
                if (a.master == false)
                {
                    a.DisarmAcceleratorTimeout();
                }
            }
            

        }
        private void armNetwork()
        {
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                var subTransform = this.part.FindModelTransform("LoadingPoint");
                var testlocation = subTransform.position;
                var vesselLocation = v.GetWorldPos3D();
                if(Vector3.Distance(testlocation, vesselLocation) < NetworkRange)
                {
                    armNetwork(v);

                }
            }

        }
        private void disarmNetwork()
        {
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                var subTransform = this.part.FindModelTransform("LoadingPoint");
                var testlocation = subTransform.position;
                var vesselLocation = v.GetWorldPos3D();
                if (Vector3.Distance(testlocation, vesselLocation) < NetworkRange)
                {
                    DisarmNetwork(v);

                }
            }

        }
        private void PlayStartAnimation(Animation StartAnimation, string startAnimationName, int speed, bool instant)
        {
            if (startAnimationName != "")
            {
                if (speed < 0)
                {
                    StartAnimation[startAnimationName].time = StartAnimation[startAnimationName].length;
                }
                if (instant)
                    StartAnimation[startAnimationName].speed = 999999 * speed;
                StartAnimation[startAnimationName].speed = speed;
                StartAnimation.Play(startAnimationName);
            }
        }
        public void PlayAnimation(string name, bool rewind, bool instant, bool loop)
        {
            // note: assumes one ModuleAnimateGeneric (or derived version) for this part
            // if this isn't the case, needs fixing. That's cool, I called in the part.cfg


            {

                var anim = part.FindModelAnimators();

                foreach (Animation a in anim)
                {
                    // print("animation found " + a.name + " " + a.clip.name);
                    if (a.clip.name == name)
                    {
                        // print("animation playingxx " + a.name + " " + a.clip.name);
                        var xanim = a;
                        if (loop)
                            xanim.wrapMode = WrapMode.Loop;
                        else
                            xanim.wrapMode = WrapMode.Once;
                        PlayStartAnimation(xanim, name, (rewind) ? (-1) : (1), instant);
                    }
                }

            }


        }
        Vessel getLaunchTarget()
        {
            foreach(Vessel v in FlightGlobals.Vessels)
            {
                if(v.id == LaunchTarget)
                    return v;
            }
            return null;
        }
        Vessel getClosestVesselToLoadPoint(out double distance)
        {
           return getClosestVesselToLoadPoint(out distance, false);
        }
        Vessel getClosestVesselToLoadPoint(out double distance, bool debug)
        {
            var subTransform = this.part.FindModelTransform("LoadingPoint");
           if(debug)
            print(subTransform.name);
           var testlocation = subTransform.transform.position;

            var accelerateVector = this.part.transform.position - testlocation;
            if (debug)
            print("accelerate vector = " + accelerateVector.ToString());
            var location = subTransform.transform.position;
            if (debug)
            print("Getting closest vessel... locations " + testlocation.ToString() + " vesselloc " + location.ToString());
            Vessel closest = null;
            double closestDistance = 2300; //any further than this and it doesn't matter
            foreach(Vessel v in FlightGlobals.Vessels)
            {
               
                if (v.id == this.vessel.id)
                    continue;
                var targetLocation = v.GetWorldPos3D();
                double d = Vector3d.Distance(location, targetLocation);
                if (debug)
                    print("vessel test: " + v.name + " loc = " + targetLocation.ToString() + " distance = " + d.ToString());
                if(d < closestDistance)
                {
                    closest = v;
                    closestDistance = d;
                }


            }
              
            distance = closestDistance;
            return closest;
        }

        public override void OnActive()
        {
            
 	         base.OnActive();
        }
        void pullLaunchTargetToLaunchPosition(float dt)
        {
            var subTransform = this.part.FindModelTransform("LoadingPoint");
            var testlocation = subTransform.position;
            if (vLaunchTarget != null)
            {
                var pullVector = testlocation - vLaunchTarget.GetWorldPos3D();
              //  print("pull vector " + pullVector.ToString());
               // print("acceleration " + (pullVector * dt).ToString());
                double f = powerLevel * acceleratorForce / 100; 
                var spent = part.RequestResource(resourceName, dt * resourceAmount * f);
                if (spent == 0)
                {
                   
                    f = 0;
                   

                }
                   
                double m = vLaunchTarget.RevealMass();
                float a = (float)(f / m);
                double strength = 4*(pullVector.magnitude - loadDistance ) / (loadDistance) * a;
                pullVector.Normalize();
                pullVector = pullVector * a;
                vLaunchTarget.ChangeWorldVelocity(pullVector * dt);
                m = this.vessel.RevealMass();
                a = (float)(f / m);
                pullVector.Normalize();
                pullVector = pullVector * -a; //for every action there is an equal and opposite reaction
                //this.vessel.ChangeWorldVelocity(pullVector);
                //calculate resource draw based on the applied power

                
            }
        }
        public override void OnUpdate()
        {
              double dt = TimeWarp.deltaTime;
            //should be 0 = realtime

            if(!master)
            {
                Events["StartCountdown"].guiActive = false;
                Events["fire"].guiActive = false;
                Events["DisarmAccelerator"].guiActive = false;
                Events["ArmAccelerator"].guiActive = false;

            }
            acceleratorDisplayForce = powerLevel * acceleratorForce /100;
            if(TimeWarp.CurrentRateIndex == 0)
            {
                if (armed)
                {
                    if (firetime < accelerationDuration)
                    {
                        if (vLaunchTarget != null)
                        {
                            var subTransform = this.part.FindModelTransform("LoadingPoint");
                            var testlocation = subTransform.position;
                            var vespos = this.vessel.GetWorldPos3D();
                            var targetpos = vLaunchTarget.GetWorldPos3D();
                            print("vesselpos " + vespos.ToString());
                            print("targetvesselpos " + targetpos.ToString());
                            print("localtransform " + subTransform.localPosition.ToString());
                            print("globaltransform " + subTransform.position.ToString());
                            print("parttransform " + transform.position.ToString());
                            print("distance from ship transform to part transform" + Vector3.Distance(transform.position, vespos));

                            print("distance betwen them" + Vector3.Distance(transform.position, subTransform.position).ToString());
                            //var accelerateVector = this.part.transform.position - vLaunchTarget.GetWorldPos3D();
                            var accelerateVector = this.part.transform.position - testlocation;
                            print("accelerate vector is " + accelerateVector.ToString());
                            //f = m/a
                            //f is in Kn
                            //m is in Mt (1000 kg)
                            //a is m/s^2
                            //a scales by DT
                            //a = f/m, assuming M is not 0
                            

                            double f = powerLevel * acceleratorForce/100;
                            var spent = part.RequestResource(resourceName, dt * resourceAmount * f);
                            if (spent == 0)
                                f = 0;
                            double m = vLaunchTarget.RevealMass();
                            float a = (float)(f / m);
                            dt = Math.Min(dt, accelerationDuration); //for slow computers... don't apply more than the duration of acceleration
                            vLaunchTarget.ChangeWorldVelocity(accelerateVector * (a * (float)dt));
                            //for every action there is an equal and opposite reaction
                            m = this.vessel.RevealMass();
                            a = (float)(f / m);
                            vLaunchTarget.ChangeWorldVelocity(accelerateVector * (-a * (float)dt));
                           
                        }
                        firetime += dt;
                    }
                    else if (firing)
                    {
                        armed = false;
                        status = "idle";
                        firing = false;
                        DisarmAccelerator();
                    }

                    if (!firing )
                    {
                       if( !master)
                        pullLaunchTargetToLaunchPosition((float)dt);

                    }

                    if(countdown)
                    {
                        if(countdownTime < 0)
                        {
                            fire();
                            countdownTime = 999;
                            countdown = false;

                        }
                        else
                        {
                            status = string.Format("Time to launch: {0}", countdownTime);
                            countdownTime -= dt;
                        }


                    }
                    if (!master)
                    {
                        double dist = 0;
                        vLaunchTarget = getClosestVesselToLoadPoint(out dist);
                        if (vLaunchTarget && dist < loadDistance)
                        {
                            fire();
                        }
                        else
                            vLaunchTarget = null;
                        if(enableShutdowntimeout)
                        {
                            disarmTimeout -= dt;
                            if (disarmTimeout < 0)
                            {
                                enableShutdowntimeout = false;
                                DisarmAccelerator();
                            }
                        }
                    }
                }

            }




            base.OnUpdate();
        }

        [KSPEvent(guiName = "Arm Accelerator", guiActive=true)]
        void ArmAccelerator()
        {
           
            double cd = 0;
            vLaunchTarget = getClosestVesselToLoadPoint(out cd, true);
            print("finding closest target");
            if (master)
            {
                if (vLaunchTarget != null)
                    print("found vessel nearby " + vessel.name + " dist = " + cd + " targetdist = " + loadDistance);
                if (vLaunchTarget != null && cd < loadDistance)
                {
                    print("found closest vessel...  " + vLaunchTarget.name + " dist " + cd.ToString());
                    armed = true;
                    foreach (BaseEvent e in Events)
                        print(e.name);
                    // LaunchTarget = vLaunchTarget.id;
                    status = "armed";
                    Events["StartCountdown"].guiActive = true;
                    Events["fire"].guiActive = true;
                    Events["DisarmAccelerator"].guiActive = true;
                    Events["ArmAccelerator"].guiActive = false;
                   
                    PlayAnimation(armedAnimation, false, false, false);
                }
                else
                    vLaunchTarget = null;
            }
            else if (!master)
            {
                PlayAnimation(armedAnimation, false, false, false);
                armed = true;
                status = "armed";
            }
            

        }
        [KSPEvent(guiName = "Disarm Accelerator", guiActive = false)]
        void DisarmAccelerator()
        {
            armed = false;
            status = "Idle";
            
            Events["fire"].guiActive = false;
          Events["StartCountdown"].guiActive = false;
            Events["DisarmAccelerator"].guiActive = false;
            Events["ArmAccelerator"].guiActive = true;
            PlayAnimation(armedAnimation, true, false, false);
            disarmNetwork();

        }
         [KSPEvent(guiName = "Fire Accelerator", guiActive=false)]
        void fire()
         {
             armNetwork();
             firetime = 0;
             status = "firing";
             firing = true;
                 

         }
         [KSPEvent(guiName = "Start Countdown", guiActive = false)]
         void StartCountdown()
         {
             Events["StartCountdown"].guiActive = false;
             countdownTime = 10;
             countdown = true;
             


         }
        bool vesselHasEnoughResource(string name, float rc)
        {
            if (this.vessel == FlightGlobals.ActiveVessel)
            {
                //print("found vessel event!");
                //var resources = vessel.GetActiveResources();
                //for (int i = 0; i < resources.Count; i++)
                //{
                //    print("vessel has resources!");
                //    print(resources[i].info.name);
                //    print("im looking for " + resourceName);
                //    if (resources[i].info.name == resourceName)
                //    {
                //        print("Found the resouce!!");
                //        if (resources[i].amount >= resourceAmount)
                //        {
                //            return true;
                //        }
                //    }
                //}
            }
            return false;
        }


    }
    //this one is to be added via the module manager to EVERYTHING so that we can scale the parts correctly.
    public class VABScaleDetector:PartModule
    {
        public override void OnStart(PartModule.StartState state)
        {
            EditorLogic logic = EditorLogic.fetch;

             if (logic != null)
            {
                Debug.Log("found editor logic!");
                
                Debug.Log(logic.ship[0].scaleFactor);
                
                foreach(Part p in logic.ship.parts)
                {
                    var moduleList = p.FindModulesImplementing<StructuralIntegrityField>();
                    if(moduleList.Count != 0)
                    {
                        StructuralIntegrityField mod = moduleList[0];
                        if(mod.sizeState)
                        {
                            //don't rescale a strut.
                            if(this.part.FindModuleImplementing<CompoundParts.CModuleStrut>() == null ) 
                                this.part.rescaleFactor = .02f;
                            //this.part.partTransform.localScale = new Vector3(.02f, .02f, .02f);
                            this.part.srfAttachNode.position *= .02f;
                            foreach (AttachNode node in part.attachNodes)
                            {
                               // node.size = 1;
                                node.originalPosition /= 50.0f;
                                //node.position /= 50.0f;
                            }
                          
                             
                            var lightsOnModel = part.FindModelComponents<Light>();
                            if (lightsOnModel != null)
                                foreach (Light l in lightsOnModel)
                                    l.enabled = false;
                        }

                    }

                }



                //logic.ship;

            }
            base.OnStart(state);
        }

    }

    public class StructuralIntegrityField : PartModule
    {
        [KSPField(isPersistant = false, guiActive = false)]
        public int fieldStrength = 40000000;
        [KSPField(isPersistant = false, guiActive = false)]
        public float fieldRange = 2500;

        bool oneshotPrint = false;
        public override void OnStart(PartModule.StartState state)
        {
            EditorLogic logic = EditorLogic.fetch;
            
            if (logic != null)
            {
                Debug.Log("found editor logic!");
                
                Debug.Log(logic.ship[0].scaleFactor);
                
                //logic.ship;
               
            }
            base.OnStart(state);
        }
        public bool sizeState = false; //false means full size
        [KSPEvent(guiName = "Editing mode...", guiActive = false, guiActiveEditor= true)]
        public void scaleMe()
        {
            
            sizeState = !sizeState;
            EditorLogic logic = EditorLogic.fetch;
            if (logic != null)
            {
                if (this.part != logic.ship.parts[0])
                {
                    Debug.Log("Part must be root for editing mode.");
                    return;
                }
                Debug.Log("found editor logic!");
                
                Debug.Log(logic.ship[0].scaleFactor);
                if(sizeState)
                    part.partTransform.localScale = new Vector3(.02f, .02f, .02f);
                else
                    part.partTransform.localScale = new Vector3(1f, 1f, 1f);
                
                foreach(Part p in logic.ship.parts)
                {
                    if (sizeState)
                    {
                        if(vessel != null)
                        {

                            Debug.Log("found vessel!");
                            //vessel.vesselTransform.localScale *= .02f;

                        }
                       // p.transform.localScale *= .02f;
                        if (p == this.part)
                            continue;
                       
                       //todo: scale the attach nodes too
                        foreach (AttachNode node in p.attachNodes)
                        {
                            //node.size = 1;
                            node.position *= .02f;
                        }
                        var lightsOnModel = p.FindModelComponents<Light>();
                        if (lightsOnModel != null)
                            foreach (Light l in lightsOnModel)
                                l.enabled = false;
                               
                        p.srfAttachNode.position *= .02f;
                      
                        
                    }
                    else
                    {
                       // p.transform.localScale *= 100.0f;
                        Debug.Log(p.rescaleFactor);
                        if (p == this.part)
                        {
                           
                            continue;
                        }
                       // if (p.rescaleFactor != 1)
                          //  p.partTransform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
                       // else
                            //p.partTransform.localScale = new Vector3(1.0f,1.0f,1.0f);
                        p.rescaleFactor = 1;
                        p.srfAttachNode.position *= 50.0f;
                        foreach (AttachNode node in p.attachNodes)
                        {
                            Debug.Log(node.position);
                            node.position *= 50.0f;

                        }
                        logic.saveBtn.enabled = true;
                    }



                }
                //logic.ship;
               
            }
            

        }

        public override void OnActive()
        {
          
            var partlist = this.vessel.GetActiveParts();
            foreach (Part p in partlist)
            {
                print(p.name + " " + p.crashTolerance + " " + p.breakingForce + " " + p.breakingTorque);
                //print(p.collider.active);
                print(p.attachMethod);
                p.crashTolerance = fieldStrength;
                p.breakingForce = fieldStrength;
                p.breakingTorque = fieldStrength;
               // p.attachMethod = AttachNodeMethod.FIXED_JOINT;
                

            }
             var vesselList = FlightGlobals.Vessels;
              foreach(Vessel v in vesselList)
            {
                print(v.RevealName());
                print(v.distancePackThreshold);
                print(v.distanceUnpackThreshold);
                  

            }
            base.OnActive();
            oneshotPrint = true;
        }
        void setNoRailsArea()
        {

           
            var vesselList = FlightGlobals.Vessels;
            var myloc = vessel.GetWorldPos3D();
           // if(oneshotPrint)
           // {
            //    print(Vessel.unloadDistance);
            //    oneshotPrint = false;
            //}
           // Vessel.unloadDistance = 120000;
           // Vessel.loadDistance = 120000;
            foreach (Vessel v in vesselList)
            {
                // print(v.RevealName());
                //print(v.packed);
                //print(v.distancePackThreshold);
                // print(v.distanceUnpackThreshold);

                var theirloc = v.GetWorldPos3D();
                if (Vector3.Distance(myloc, theirloc) < fieldRange)
                {
                    v.distanceUnpackThreshold = fieldRange;

                }


            }
        }
        public override void OnFixedUpdate()
        {
            setNoRailsArea();
            base.OnFixedUpdate();
        }
       
       //every part attached to the vessel with this module gets a crash tolerance of fieldStrength, and breaking force/torque to match
        public override void OnUpdate()
        {
           

            setNoRailsArea();
            base.OnUpdate();
           
        }


    }
    //public class automatedConstruction: ExLP.ExWorkshop
    //{

    //    [KSPField(guiActive = false, isPersistant=true)]
    //    public float automatedProductivity = 20.0f;
    //    public override void OnUpdate()
    //    {
    //        base.OnUpdate();
    //        Productivity = automatedProductivity;
    //    }


    //}
}
