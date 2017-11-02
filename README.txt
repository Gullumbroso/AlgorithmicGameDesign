Gilad Lumbroso 204781694
Inbal Bruker 206096927

DrunkBrain:
The drunk spaceship follows the nearest spaceship and tries to shoot it, but due to the amount of alcohol in its blood it aims with a random mistake. Also, when the drunk spaceship hiccups randomly, the shield is raised.

ShieldAndShootBrain:
As implied by its name, this spaceship constantly check the area for threats. It raises the shield if danger is nearby, whether a shot or a hostile spaceship. Its second priority is to follow closest ships and shoot them when they are in range.
The issue we tried to solve is what to do when there is a spaceship with a similar aggressive strategy, since they will collide in each other again and again. We thought that trying to change course of the spaceship will not be a good idea, since turning the back on an aggressive spaceship is a recipe for disaster. So our solution was to play with the values of the shield cool down timer in order to find the best formula for the timing of turning the shield on and off.


