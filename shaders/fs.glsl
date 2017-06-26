#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 intersectionPoint;		// the world location of the pixel you are drawing
uniform sampler2D pixels;		// texture sampler
uniform vec4 ambientColor;		// color that's added to every value of light
uniform vec3 cameraPos;			// position of the camera at the time of drawing this.

//uniform int lightAmount;
//uniform vec3 lightPos[lightAmount];
//uniform vec3 lightIntensity[lightAmount];
//uniform vec3 lightSpecular[lightAmount];

vec3 lightpos1 = vec3(0, -800, 0);
vec3 light1 = vec3(300000, 300000, 300000);		
vec3 lightSpec1 = vec3(1, 1, 1);		

// shader output
out vec4 outputColor;

// fragment shader
void main()
{

	//camera position is probably incorrect.
	

	vec3 L = lightpos1 - intersectionPoint.xyz;
	vec3 C = normalize(intersectionPoint.xyz - cameraPos); // supposed to be a normalized version of the vector from the camera to the intersection point, reflected in the normal of the surface of the reflection point
	
	float ndotl = dot(normalize(L), normal.xyz);
	ndotl = max(-ndotl, 0);
	vec3 mirroredL = -normalize(L - (2 * normal.xyz * dot(normal.xyz,L)));

	vec4 diffTexture = vec4(light1.x * texture( pixels, uv ).x, light1.y * texture( pixels, uv).y, light1.z * texture( pixels, uv ).z, 0);
	vec4 specularTexture = vec4(lightSpec1.x * texture( pixels, uv ).x, lightSpec1.y * texture( pixels, uv).y, lightSpec1.z * texture( pixels, uv ).z, 0);

	float specular = pow(dot(mirroredL,C),1000);

	vec4 cDiff = (ndotl * diffTexture) / (L.x * L.x + L.y * L.y + L.z * L.z);
	vec4 cSpecular = specular * specularTexture;
	vec4 cAmbient = vec4(ambientColor.x * texture( pixels, uv ).x, ambientColor.y * texture( pixels, uv).y, ambientColor.z * texture( pixels, uv ).z, 0);

	cDiff.x = min(cDiff.x, texture(pixels, uv).x);
	cDiff.y = min(cDiff.y, texture(pixels, uv).y);
	cDiff.z = min(cDiff.z, texture(pixels, uv).z);
	cSpecular.x = min(cSpecular.x, 1);
	cSpecular.y = min(cSpecular.y, 1);
	cSpecular.z = min(cSpecular.z, 1);

	vec4 color = cAmbient + cDiff;
	
	color.x = min(color.x, texture(pixels, uv).x);
	color.y = min(color.x, texture(pixels, uv).y);
	color.z = min(color.x, texture(pixels, uv).z);

    outputColor = color + cSpecular;
}