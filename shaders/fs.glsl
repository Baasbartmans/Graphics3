#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 intersectionPoint;		// the world location of the pixel you are drawing
uniform sampler2D pixels;		// texture sampler
uniform vec4 ambientColor;		// color that's added to every value of light
uniform vec3 cameraPos;			// position of the camera at the time of drawing this.
vec3 lightpos1 = vec3(0, -16, 0);
float lightintensity1 = 50;		//supposed to become a vector3

// shader output
out vec4 outputColor;

// fragment shader
void main()
{

	//camera position is probably incorrect.
	

	vec3 L = lightpos1 - intersectionPoint.xyz;
	vec3 C = normalize(intersectionPoint.xyz - cameraPos); // supposed to be a normalized version of the vector from the camera to the intersection point, reflected in the normal of the surface of the reflection point
	float diff = lightintensity1 / (L.x * L.x + L.y * L.y + L.z * L.z);
	float ndotl =  dot(normalize(L), normal.xyz);
	ndotl = max(ndotl, -ndotl);
	vec3 mirroredL = normalize(L - (2 * normal.xyz * dot(normal.xyz,L)));

	float specular = pow(dot(mirroredL,C),400);

    outputColor = ambientColor + diff * ndotl * texture( pixels, uv ) + specular * lightintensity1 * texture( pixels, uv );
}