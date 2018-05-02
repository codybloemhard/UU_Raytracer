#version 330
in vec3 vPos;
in vec3 vNor;
uniform mat4 uModelM;
uniform mat4 uModelM;

uniform float uMaxHeight;
uniform vec3 uLightDir;

out vec3 pos;
out vec3 nor;
out vec3 lightDir;
out float height;

void main()
{
	pos = vPos;
	gl_Position = uMat * vec4(pos, 1.0);	
	nor = normalize(vNor);
	height = uMaxHeight;
	lightDir = uLightDir;
}