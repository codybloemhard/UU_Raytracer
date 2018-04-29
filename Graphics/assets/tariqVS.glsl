#version 330
in vec3 vPos;
in vec3 vNor;
uniform mat4 uMat;
uniform float uMaxHeight;

out vec3 pos;
out vec3 nor;
out float height;

void main()
{
	pos = vPos;
	gl_Position = uMat * vec4(pos, 1.0);	
	nor = normalize(vNor);
	height = uMaxHeight;
}