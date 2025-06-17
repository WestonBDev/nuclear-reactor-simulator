# Nuclear Reactor Dynamics Simulator

[![Unity](https://img.shields.io/badge/Unity-6.0-black?style=flat-square&logo=unity)](https://unity.com/)
[![DOTS](https://img.shields.io/badge/DOTS-ECS%20Architecture-orange?style=flat-square)](https://unity.com/dots)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)

A real-time nuclear reactor physics simulator built in Unity 6 using DOTS (Data-Oriented Technology Stack) and Entity Component System (ECS) architecture with Burst-compiled systems. Reproduces the 01:23:40 power excursion from April 26, 1986.

## Technical Implementation

### Architecture Overview

| Component | Description |
|-----------|-------------|
| **Individual neutron simulation** | Neutron entities with real-time collision detection and state transitions |
| **ECS Systems Architecture** | 13+ Burst-compiled systems handling reactor physics in parallel |
| **Real-time reactor calculations** | Core temperature, pressure, void coefficient, and reactivity modeling |
| **Interactive control systems** | Manual/automatic control rod operation with 5-group rod banks |

## Nuclear Physics Modeling

### Core Physics Systems

**Neutron Dynamics**
- Fast and thermal neutron states with real-time collision detection
- Neutron multiplication through fission events
- Neutron absorption by control rods and xenon

**Uranium Fuel Dynamics**
- Three uranium states: inactive (0), reactive (1), and xenon-poisoned (2)
- Fission chain reactions spawning 3 neutrons per collision
- Random probability of continued reactivity vs. xenon production

**Xenon-135 Poisoning**
- Neutron-absorbing fission product accumulation
- Automatic xenon counting system
- Reactor poisoning effects on neutron multiplication

**Positive Void Coefficient**
- Steam formation reducing neutron moderation
- Void fraction calculations based on core temperature and pressure

**Control Rod Mechanics**
- Manual slider control for each group
- Automatic rod movement based on neutron levels (>40 neutrons triggers insertion)
- Emergency SCRAM function (immediate full insertion)

**Thermal Hydraulics**
- Core temperature calculation based on neutron flux
- Pressure dynamics from steam formation
- Coolant temperature modeling with cooling system effects

## Simulation Features

### Instrumentation and Monitoring

**Real-time Reactor Parameters**
- Neutron count display
- Core temperature monitoring
- Pressure gauge
- Reactivity meter
- Reactor period calculation
- Void fraction tracking
- Manual SCRAM button
- Automatic reactor shutdown conditions

### Control Systems

- **Manual Control**: Individual control rod group sliders
- **Automatic Control**: System-managed rod positioning based on neutron levels
- **Emergency SCRAM**: Immediate full rod insertion
- **Coolant System**: Water cooling with adjustable flow rates

## Technical Details

### ECS Systems
1. **UraniumSpawnerSystem** - Initial fuel rod grid generation
2. **NeutronSpawnerSystem** - Neutron entity creation with void coefficient effects
3. **NeutronMovingSystem** - Neutron movement and collision detection
4. **UraniumActivationSystem** - Random uranium activation for chain reactions
5. **NeutronCounterSystem** - Real-time neutron tracking
6. **XenonCounterSystem** - Xenon-135 accumulation monitoring
7. **RodMovementSystem** - Control rod positioning (manual/automatic)
8. **WaterCoolingSystem** - Coolant circulation simulation
9. **ColorChangeSystem** - Visual state indication for uranium
10. **NeutronDeletionSystem** - Neutron cleanup and lifecycle management
11. **WaterSpawnerSystem** - Coolant grid initialization
12. **RodSpawnerSystem** - Control rod placement
13. **ModeratorSpawnerSystem** - Moderator rod deployment

### Physics Implementation
- **Grid-based reactor core**: Configurable rows/columns for uranium placement
- **Collision detection**: Distance-based neutron-uranium interactions
- **State management**: Component data for all reactor elements
- **Performance optimization**: Burst compilation for all physics systems

## Inspiration

This project was inspired by the Higgsino Physics video: [Chernobyl Visually Explained](https://youtu.be/P3oKNE72EzU?si=kIGXqNy9mmr4gZ)
