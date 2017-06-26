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
	vec3 colLook = vec3(0,0,0);
	float div = 0.00389105058365758754863813229572; // = 1/257
	colLook.r = texture( pixels, vec2(uv.x + (dx * caI), uv.y  + (dy * caI)) ).r;
	colLook.g = texture( pixels, vec2(uv.x , uv.y ) ).g;
	colLook.b = texture( pixels, vec2(uv.x - (dx * caI), uv.y  - (dy * caI)) ).b;
	vec2 colGrad = vec2( ( (colLook.b * 256) + colLook.g) * div, colLook.r );
	
	//color grading
	outputColor.r = texture( colCub, colGrad).r * vignet.r;
	outputColor.g = texture( colCub, colGrad).g * vignet.g;
	outputColor.b = texture( colCub, colGrad).b * vignet.b;

	//output without color grading
	//outputColor.r = texture( pixels, vec2(uv.x + (dx * caI), uv.y  + (dy * caI)) ).r * vignet.r;
	//outputColor.g = texture( pixels, vec2(uv.x , uv.y ) ).g * vignet.g;
	//outputColor.b = texture( pixels, vec2(uv.x - (dx * caI), uv.y  - (dy * caI)) ).b * vignet.b;

}

// EOF