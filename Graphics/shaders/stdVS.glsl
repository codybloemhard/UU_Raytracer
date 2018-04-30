#version 330
in vec3 vPos;
in vec3 vNor;
uniform mat4 uModelM;
uniform mat4 uWorldM;
uniform mat4 uViewM;
uniform vec3 uLightDir;

out vec3 pos;
out vec3 nor;
out vec3 lightDir;

void main()
{
	pos = (vec4(vPos, 1.0) * uWorldM * uModelM).xyz;
	gl_Position = uViewM * uWorldM * uModelM * vec4(pos, 1.0);
	nor = normalize(vNor);
	lightDir = uLightDir;
}