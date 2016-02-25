float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;

float4 AmbienceColor = float4(0.03f, 0.03f, 0.03f, 1.0f);

// For Diffuse Lightning
float4x4 WorldInverseTransposeMatrix;
float3 DiffuseLightDirection = float3(0.0f, 0.5f, 1.0f);
float3 DiffuseLightDirection2 = float3(0.0f, 0.5f, -1.0f);
float4 DiffuseColor = float4(1.0f, 1.0f, 1.0f, 1.0f);

// For Texture
texture ModelTexture;

sampler2D TextureSampler = sampler_state 
{
	Texture = (ModelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	// For Diffuse Lightning
	float4 NormalVector : NORMAL0;
	// For Texture
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	// For Diffuse Lightning
	float4 VertexColor : COLOR0;
	float4 VertexColor2 : COLOR1;
	// For Texture    
	float2 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, WorldMatrix);
		float4 viewPosition = mul(worldPosition, ViewMatrix);
		output.Position = mul(viewPosition, ProjectionMatrix);

	// For Diffuse Lightning
	float4 normal = normalize(mul(input.NormalVector, WorldInverseTransposeMatrix));
	float lightIntensity = dot(normal, DiffuseLightDirection);
	float lightIntensity2 = dot(normal, DiffuseLightDirection2);
	output.VertexColor = saturate(DiffuseColor * lightIntensity);
	output.VertexColor2 = saturate(DiffuseColor * lightIntensity2);

	// For Texture
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// For Texture
	float4 VertexTextureColor = tex2D(TextureSampler, input.TextureCoordinate);
	VertexTextureColor.a = 1;

	return saturate(VertexTextureColor * input.VertexColor + AmbienceColor) +
		saturate(VertexTextureColor * input.VertexColor2 + AmbienceColor);
}

technique Texture
{
	pass Pass1
	{
		VertexShader = compile vs_1_1 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
