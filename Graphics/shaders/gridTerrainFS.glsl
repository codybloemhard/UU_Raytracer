#version 330
in vec3 pos;
in vec3 nor;
in vec3 lightDir;
in float height;

out vec4 outputColor;
//source: https://thebookofshaders.com/11/
float rand(vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898,78.233))) * 43758.5453123);
}
//source: https://thebookofshaders.com/11/
float noise(in vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);
    float a = rand(i);
    float b = rand(i + vec2(1.0, 0.0));
    float c = rand(i + vec2(0.0, 1.0));
    float d = rand(i + vec2(1.0, 1.0));
    vec2 u = f*f*(3.0-2.0*f);
    return mix(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

void main()
{
	//Calculate colour of terrain
	float h = (abs(pos.z) / height);
	vec3 zee = vec3(0.1, 0.3, 0.9);
	vec3 zand = vec3(0.7, 0.7, 0.4);
	vec3 gras = vec3(0.1, 0.7, 0.1);
	vec3 grond = vec3(0.5, 0.3, 0.3);
	vec3 sneeuw = vec3(0.8, 0.9, 0.9);
 
	float eps = 0.0001;
	float bg = 0.00;
	float w0 = 0.05;
	float w1 = 0.10;
	float w2 = 0.35;
	float w3 = 0.70;
	float nd = 1.00;
 
	vec3 m0 = zee;
	vec3 m1 = mix(zee, zand, smoothstep(w0, w1 - (w1 - w0) * 0.5, h));
	vec3 m2 = mix(zand, gras, smoothstep(w1, w2 - (w2 - w1) * 0.5, h));
	vec3 m3 = mix(gras, grond, smoothstep(w2, w3 - (w3 - w2) * 0.5, h));
	vec3 m4 = mix(grond, sneeuw, smoothstep(w3, nd - (nd - w3) * 0.5, h));
 
	vec3 z0 = smoothstep(bg - eps, bg + eps, h) * smoothstep(w0 + eps, w0 - eps, h) * m0;
	vec3 z1 = smoothstep(w0 - eps, w0 + eps, h) * smoothstep(w1 + eps, w1 - eps, h) * m1;
	vec3 z2 = smoothstep(w1 - eps, w1 + eps, h) * smoothstep(w2 + eps, w2 - eps, h) * m2;
	vec3 z3 = smoothstep(w2 - eps, w2 + eps, h) * smoothstep(w3 + eps, w3 - eps, h) * m3;
	vec3 z4 = smoothstep(w3 - eps, w3 + eps, h) * smoothstep(nd + eps, nd - eps, h) * m4;
 
	float nStr = 0.3;
	float n0 = noise(vec2(pos.x, pos.y) / 5);
	float n1 = noise(vec2(pos.x, pos.y) / 1);
	vec3 n = vec3(n0 * 0.7 + n1 * 0.3);
	vec3 c = z0 + z1 + z2 + z3 + z4;
	c *= (1.0 - nStr) + (n * nStr);
	
	//Calculate lighting
	vec3 lightD = normalize(lightDir);
	vec3 lightC = vec3(1.0);
	float ambientStr = 0.1;	
	vec3 ambientC = lightC * ambientStr;
	float diff = max(dot(nor, lightD), 0.0);
	vec3 diffuseC = diff * lightC;
	
	vec3 result = (ambientC + diffuseC) * c;
	outputColor = vec4(result.xyz, 1.0);
}