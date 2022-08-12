// Each #kernel tells which function to compile; you can have many kernels
#pragma kernel spad_kernel
#pragma kernel bbox_kernel
#pragma kernel crop_kernel
#pragma kernel post_kernel
// Create a RenderTexture with enableRandomWrite flag and set it
// with cs.SetTexture

#include "HandRegion.hlsl"
#include "LowPassFilter.hlsl"

uint _spad_width;
float2 _spad_scale;

sampler2D _inputTexture;
RWStructuredBuffer<float> _output;

[numthreads(8, 8, 1)] void CSMain(uint2 id
                                  : SV_DispatchThreadID) {
  if (any(id > _spad_width)) return;

  // Caluculate vertically flipped UV.
  float2 uv = (id + 0.5) / _spad_width;
  uv.y = 1 - uv.y;

  // scalling
  uv = (uv - 0.5) * _spad_scale + 0.5;

  // Caluculate vertically flipped UV gradients.
  float2 duv_dx = float2(1.0 / _spad_width * _spad_scale.x, 0);
  float2 duv_dy = float2(0, -1.0 / _spad_width * _spad_scale.y);

  // texture sampl
  float3 rgb = tex2Dgrad(_inputTexture, uv, duv_dx, duv_dy).rgb;

  // generate output
  uint offs = (id.y * _spad_width + id.x) * 3;
  _output[offs + 0] = rgb.r;
  _output[offs + 1] = rgb.g;
  _output[offs + 2] = rgb.b;
}

#define POST_KEYPOINT_COUNT 21

float _post_dt;
float _post_scale;

StructuredBuffer<float4> _post_input;
StructuredBuffer<HandRegion> _post_region;

RWStructuredBuffer<float4> _post_output;

[numthreads(POST_KEYPOINT_COUNT, 1, 1)] void post_kernel(
    uint id
    : SV_DispatchThreadID) {
  HandRegion region = _post_region[0];

  float3 x = _post_input[id + 1].xyz;
  float3 p_x = _post_output[id].xyz;
  float3 p_dx = _post_output[id + POST_KEYPOINT_COUNT].xyz;

  x = mul(region.cropMatrix, float4(x, 1)).xyz;
  x.xy = (x.xy - 0.5) * _post_scale;

  float3 lpf_params = float3(30, 1.5, _post_dt);
  float3 dx = lpf_Step_dx(x, p_x, p_dx, lpf_params);
  x = lpf_Step_x(x, p_x, dx, lpf_params);

  _post_output[id] = float4(x, 1);
  _post_output[id + POST_KEYPOINT_COUNT] = float4(dx, 1);
}
