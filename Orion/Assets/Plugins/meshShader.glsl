#version 450
#extension GL_NV_mesh_shader : enable

layout(local_size_x = 3) in;
layout(max_vertices = 64) out;
layout(max_primitives = 126) out;
layout(triangles) out;

layout(std430, binding = 3) buffer ssbo_t
{
	float test;
} ssbo;



void main()
{
	vec3 vertices[3] = {vec3(-1,-1,0), vec3(1,-1,0), vec3(0, ssbo.test, 0)};
	uint id = gl_LocalInvocationID.x;
	
	if(id == 2)
	{
		gl_MeshVerticesNV[id].gl_Position = vec4(vertices[id],2.0);
	}
	else
	{
		gl_MeshVerticesNV[id].gl_Position = vec4(vertices[id], 2.0);
	}
	
	gl_PrimitiveIndicesNV[id] = id;
	gl_PrimitiveCountNV = 1;
}