#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 intersectionPoint;		// the location of the pixel you are drawing
uniform sampler2D pixels;		// texture sampler
uniform vec4 ambientColor;		// color that's added to every value of light
vec4 lightpos1 = vec4(0, 20, 0, 1);
float lightintensity1 = 6;		//supposed to become a vector3

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	vec4 L = lightpos1 - intersectionPoint;
	float diff = lightintensity1 / sqrt(L.x * L.x + L.y * L.y + L.z * L.z);
	float ndotl = dot(normalize(L).xyz, normal.xyz);
    outputColor = ambientColor + ndotl * diff * texture( pixels, uv );
	
	// + 0.5f * vec4( normal.xyz, 1 );
}