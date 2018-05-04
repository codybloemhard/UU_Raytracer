#version 330
in vec3 vPos;
in vec3 vUv;//not used, engine thing

out vec2 UV;

void main()
{
	gl_Position = vec4(vPos, 1.0);
	UV = vUv.xy;
}