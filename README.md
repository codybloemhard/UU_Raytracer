# RAY TRACER

Erwin Glazenburg 	6224474
Egor Dmitriev 		6100120
Cody Bloemhard		6231888

## Bonus features:
*	Spotlight:
De spotlight is gewoon een point light maar de AngleEnergy is nu afhankelijk
van de hoek, bij de spotlight returned ie altijd 1f. Deze AngleEnergy wordt
gewoon vermedigvuldigd met de licht sterkte als er een licht position wordt
gesampled.
*	Stochastic Glossy Reflections:
Deze werken als normale reflecties maar we nemen het gemmiddelde van meerdere
rays. Elke ray heeft een random offset zodat er licht wordt gereflecteerd
vanuit meerdere hoeken. De hoeveelheid samples die worden genomen licht aan
je quality en supersampling preset.
* Polygons en meshes en Accelleration structure:
We hebben flexibele polygons met een arbritrair aantal vertices. Deze kunnen 
gebruikt worden om een mesh te maken, die je kan laden vanuit een .obj file.
Een mesh gebruikt AABB boxes om intersections te versnellen, door polygons in
steeds kleinere boxes te verdelen.
*	AntiAliasing
* 	Textured and HDR Skybox
*	Triangles and .OBJ Models
*	Refraction
*	Stochastic Area Lights
Voor de area lights worden er random punten gemaakt binnen een volume of oppervlakte.
Alle punten worden gechecked en er wordt een gemiddelde genomen. Er is een stratisfied
sampling method die veel betere resultaten geeft met minder noise.
* 	Multi threaded rendering

## Controls
* Use **Z, X, C, V to switch between scenes**
* Keys W,A,S,D move the camera horzontally. Q and E move the camera vertically.
* Use arrow keys to rotate the camera.
* Keys 1,2,3,4,5,6 Change the render preset. If your pc is bad or you want
 to move through the scene smoothly use 1 and if you want a beautiful render 
 use 6. 6 also has the nicest softshadows.
* Keys Y, U, I, O P Change the render resolution. Most of these do 
2 passes. One upscaled and second on full size with anti alias. Y render
 image at 1/8th resolution then half then full. I renders first half then 2xAA.
 O renders first half res then 4xAA. P renders 4xAA standard
