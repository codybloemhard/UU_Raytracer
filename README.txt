RAYTRACER

Erwin Glazenburg
Egor Dmitriev
Cody Bloemhard		6231888

Bonus features:

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

*	AntiAliasing

* 	Textured and HDR Skybox

*	Triangles and .OBJ Models

*	Refraction

*	Stochastic Area Lights

