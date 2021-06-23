* Refactor cone of vision stats
    - make cone of vision for zombies and humans 360 when finding each other - only humans finding player needs an angle check

### Misc
* Death sound for people and sauce
* explain win condition in first levels

### Visual
* make tentacle not reach through obstacles
* Infecting animation
* Make selected human circle more visible
    - particle effects 
    - show glow even if unifectable, but make it red
* Tint human skin
* Fade in tentacles
* Grow in tentacles
* Aim reticle
* hit animation for many melee humans not working
* better hit animation for zombies
* no need to have separate feet for each character
* restart button
* On zombiefy rotate/play animation

### Movement
* cooldown after failed leap

### AI
* A host should not be infectable while they are in search mode, else you can dart behind their back and infect them
	* nuance: host should be infectable if they are seacrhing/attacking a zombie and not the player
* When human is infected (theres an isInfected method) - make them stunned
* You should lose anonymity when you kill someone/shoot
* You should lose anon when someone sees you enter host
* Add in attack state...maybe?
* Search state
* Some solution for AI's changing at diff times - human balance
* For zombie rand patrol make it look more natural and not go out of bounds/make if follow player (play test!)


### Tile map
* Add palette for each level type
* Normalize size
* https://forum.unity.com/threads/2d-tilemap-extras-2-0-released-for-unity-2021-1.1107524/

### Refactor
* Transition manager that holds all zombify/humanify
* Cleanup die method
* Add singleton that holds all animation strings


### Bugs
* null controller agent in Chase

