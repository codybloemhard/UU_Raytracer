#version 330
in vec3 pos;
in vec3 nor;
in float height;

out vec4 outputColor;

void main()
{
	float h = (abs(pos.z) / height);
	
	vec3 result = vec3(h);
	outputColor = vec4(result.xyz, 1.0);
}