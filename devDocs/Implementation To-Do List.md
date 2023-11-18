# TODO ✓

## Unclaimed
- [ ] Battery image expands with capacity so percentage does not lower with capacity gains
  - [ ] Each bar could represent 25 capacity and maybe the ToolStates could be capacities of 50, 100, 150?  Maybe more?
- [ ] Implement new Magnetometer


### Bryant

- [x] Thrusters need sound
  - [x] Move away from T key bound

### Dhalia

- [ ] Art

### James

- [ ] Pause during falls and mid jump
- [ ] Pause in front of caves disables entrance
- [ ] Respawn button to correct scene
- [ ] Rework element system/upgrade menu to run on seperate sript
- [ ] Display tool upgrade limits on upgrades ui
- [ ] player can get stuck moving when inventory is openned
- [ ] move TmepGameManager code to GameController
- [ ] Fix battery / health pickup warning
- [ ] button input names
- [ ] Implement UI scaling for various window sizes
- [ ] Slight dialog box optimisation

### Joshua

- [x] Alternative transition implementation without as much repetitive code.
  - [x] remove excess scripts and compartmentalize the transition section from the playerManagement script
- [x] Transitions from caves back to main
- [x] Gamestate script
- [ ] Update elements to team names
- [x] Developer console with benchmark display
- [ ] ToolStates implemented into ToolManager abstract script to provide more refinement in allocating resource for modification of tools.  Each tool can have varying states which can be modified via the Modify() function.
- [x] Relocate battPercentText variable to UIController.cs and reconfigure communication event as necessary
  - [ ] Make sure to assign via code rather than inspector

### Matt
- [x] Create doc branch
- [ ] readme
- [ ] Use event to communicate player and ui for imager counter
- [ ] Reset tools on death before checkpoint

