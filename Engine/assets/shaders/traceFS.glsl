#version 330

in vec2 UV;
out vec4 outputColor;

void main()
{
	vec2 uv = UV - vec2(0.5);
	outputColor = vec4(abs(uv.x), abs(uv.y), 0.0, 1.0);
}