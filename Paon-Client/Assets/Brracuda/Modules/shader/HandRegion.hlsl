struct HandRegion
{
    float4 box; // center_x, center_y, size, angle
    float4 dBox;
    float4x4 cropMatrix;
};
