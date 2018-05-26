#version 330
in vec3 vPos;
in vec3 vUv;//not used, engine thing

out vec2 v_texCoord;

void main()
{
	gl_Position = vec4(vPos, 1.0);
	v_texCoord = vUv.xy;
}