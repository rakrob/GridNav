Hello!

GridNav is a simple algorithm (sat underneath a visualizer) for the navigation of a 2D plane filled with randomized obstructions.

GridNav comes with default values already filled in to the settings boxes. Click 'Generate Grid' to see what the grid settings look like in the visualizer. Remember, the grids are randomized, so each time you click 'Generate Grid' the program will generate something different.

When the visualizer opens for the first time, you can see your grid, obstructions, and start and end points, as well as a pre-computed flight plan from start to finish. This flight plan contains no data about obstructions it may encounter, because the path hasn't been flown yet.

Click 'Iterate Flight' to simulate one flight through the grid. This will show you the flight plan the vehicle actually followed (dark blue), as well as what the new "best path" will look like (light blue dotted line). Click 'Iterate Flight' to run the simulation again.

If you get sick of clicking 'Iterate Flight', click 'Optimize Flight Path' to perform 1 iteration every .75 seconds. This will save your mouse finger some clicks.

Click 'Reset Flight' to erase all precompute data and set the simulation back to square one.

When the precomputed flight plan matches the optimized flight plan exactly, the simulation will end and the line will turn green. This is the optimized 2D flight path that GridNav has computed. It's probably not 100% optimal (right now), especially for grids with dense, closely packed obstacles, but it's a decent proof of concept.

Thanks,
Alex