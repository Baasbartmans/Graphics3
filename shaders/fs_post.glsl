#version 330

// shader input
in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)
uniform sampler2D colCub;		// collor cube

// shader output
out vec3 outputColor;

void main()
{
	//variabelen om effecten te tunen
	float caI = 0.01;
	float vI = 0;

	//vignetting
	float dx = P.x - 0.5, dy = P.y - 0.5;
	float dist = sqrt( dx * dx + dy * dy );
	vec3 vignet = vec3(1 - dist - vI,1 - dist - vI,1 - dist - vI);

	//chromatic aberrations
	//vec3 colLook = vec3(0,0,0);
	//colLook.r = texture( pixels, vec2(uv.x + (dx * caI), uv.y  + (dy * caI)) ).r;
	//colLook.g = texture( pixels, vec2(uv.x , uv.y ) ).g;
	//colLook.b = texture( pixels, vec2(uv.x - (dx * caI), uv.y  - (dy * caI)) ).b;
	//vec2 colGrad = vec2((colLook.r * 64),(colLook.b * 4032) + (colLook.g * 64) );
	
	//color grading
	outputColor.r = texture( colCub, uv).r * vignet.r;
	outputColor.g = texture( colCub, uv).g * vignet.g;
	outputColor.b = texture( colCub, uv).b * vignet.b;

	
}

// EOF