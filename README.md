**RBMK-4 Nuclear Reactor Dynamics**

A real-time nuclear reactor physics simulator recreating the Chernobyl Unit 4 accident sequence.
Built in Unity 6 using DOTS (Data-Oriented Technology Stack) and Entity Component System (ECS) architecture with Burst-compiled systems.

**Technical Implementation:**
• **Particle-level neutron simulation** - Individual neutron entities with real-time collision detection and state transitions
• **ECS Systems Architecture** - 11+ Burst-compiled systems handling reactor physics in parallel  
• **Real-time reactor calculations** - Core temperature, pressure, void coefficient, and reactivity modeling
• **Interactive control systems** - Manual/automatic control rod operation with 5-group rod banks

**Nuclear Physics Modeling:**
• **Neutron kinetics** - Fast/thermal neutron states with authentic moderation and absorption
• **Uranium fuel dynamics** - Fuel rod activation, fission chain reactions, and state transitions  
• **Xenon-135 poisoning** - Neutron-absorbing fission product accumulation and burnout
• **Positive void coefficient** - Steam formation reducing neutron moderation
• **Control rod mechanics** - AZ-5 emergency shutdown system with insertion kinetics
• **Thermal hydraulics** - Water heating, steam formation, and pressure dynamics

**Simulation Features:**
• **Professional reactor instrumentation** - Real-time gauges, charts, and parameter plotting
• **Emergency condition detection** - Automatic SCRAM triggers and loss-of-coolant scenarios  
• **Performance tracking** - Operator response times and procedural compliance monitoring
• **Interactive scenario recreation** - Reproduces the 01:23:40 power excursion from April 26, 1986

**Inspired by Higgsino Physics video:** "[Chernobyl Visually Explained](https://youtu.be/P3oKNE72EzU?si=kIGXqNy9mmr4gZ)"