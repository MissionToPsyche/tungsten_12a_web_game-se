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
- [ ] player can get stuck moving when inventory is openned
- [ ] Pause in front of caves disables entrance
- [ ] Player sprite child object, center player
      
- [ ] Rework upgrade menu to run on seperate script
- [ ] Display tool upgrade limits on upgrades ui
      
- [ ] move TempGameManager code to GameController
      
- [ ] Fix battery / health pickup warning
- [ ] getbutton input names
- [ ] Slight dialog box optimisation
      
- [ ] Implement UI scaling for various window sizes
      
- [ ] Fix Magnet level lighting

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
- [x] readme
- [ ] Use event to communicate player and ui for imager counter
- [ ] Reset tools on death before checkpoint
- [ ] Make cave entrances bigger
- [ ] Change cave teleport locations

