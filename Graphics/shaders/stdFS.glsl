#version 330
in vec3 pos;
in vec3 nor;
in vec3 lightDir;

void main(){
	vec3 lightD = normalize(lightDir);
	vec3 lightC = vec3(1.0);
	float ambientStr = 0.1;	
	vec3 ambientC = lightC * ambientStr;
	float diff = max(dot(nor, lightD), 0.0);
	vec3 diffuseC = diff * lightC;
	
	vec3 result = (ambientC + diffuseC);
	outputColor = vec4(result.xyz, 1.0);
}