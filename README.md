# RBMK-4 Nuclear Reactor Dynamics

[![Unity](https://img.shields.io/badge/Unity-6.0-black?style=flat-square&logo=unity)](https://unity.com/)
[![DOTS](https://img.shields.io/badge/DOTS-ECS%20Architecture-orange?style=flat-square)](https://unity.com/dots)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)

A real-time nuclear reactor physics simulator recreating the Chernobyl Unit 4 accident sequence. Built in Unity 6 using DOTS (Data-Oriented Technology Stack) and Entity Component System (ECS) architecture with Burst-compiled systems. Reproduces the 01:23:40 power excursion from April 26, 1986.

## Technical Implementation

### Architecture Overview

| Component | Description |
|-----------|-------------|
| **Particle-level neutron simulation** | Individual neutron entities with real-time collision detection and state transitions |
| **ECS Systems Architecture** | 11+ Burst-compiled systems handling reactor physics in parallel |
| **Real-time reactor calculations** | Core temperature, pressure, void coefficient, and reactivity modeling |
| **Interactive control systems** | Manual/automatic control rod operation with 5-group rod banks |

## Nuclear Physics Modeling

### Core Physics Systems

**Neutron Kinetics**
- Fast/thermal neutron states with authentic moderation and absorption

**Uranium Fuel Dynamics**
- Fuel rod activation
- Fission chain reactions
- State transitions

**Xenon-135 Poisoning**
- Neutron-absorbing fission product accumulation
- Burnout modeling

**Positive Void Coefficient**
- Steam formation reducing neutron moderation

**Control Rod Mechanics**
- AZ-5 emergency shutdown system
- Insertion kinetics

**Thermal Hydraulics**
- Water heating
- Steam formation
- Pressure dynamics

## Simulation Features

### Instrumentation and Monitoring

**Professional Reactor Instrumentation**
- Real-time gauges
- Charts and parameter plotting

**Emergency Systems**
- Automatic SCRAM triggers
- Loss-of-coolant scenarios

**Performance Analysis**
- Operator response times
- Procedural compliance monitoring

### Control Systems

- **Manual Control**: Direct control rod operation
- **Automatic Control**: System-managed rod positioning
- **5-Group Rod Banks**: Categorized control rod management
- **AZ-5 Emergency System**: Emergency shutdown capability

## Inspiration

This project was inspired by the Higgsino Physics video: [Chernobyl Visually Explained](https://youtu.be/P3oKNE72EzU?si=kIGXqNy9mmr4gZ)
