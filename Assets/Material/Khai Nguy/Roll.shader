// Shader for mesh paper roll effect based on Archimedean Spiral.
// This implementation utilises a starting angle and that
// from the origin of the spiral till the point where it has swept through
// the starting angle will not be rendered by the mesh.
// The start of the mesh will be the split point of the rolled and unrolled
// portions.

Shader "Custom/Roll"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _StartAngle("Start Angle (Radians)", float) = 60.0
        _AnglePerUnit("Radians per Unit", float) = 0.2
        _Pitch("Pitch", float) = 0.02
        _UnrolledAngle("Unrolled Angle", float) = 1.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        fixed4 _Color;
        float _StartAngle; // the angle rolled at the start
        float _AnglePerUnit; // how much radian the distance between 2 vertices cover
        float _Pitch; // the rate at which its radius changes as its angle changes
        float _UnrolledAngle; // how much was unrolled


        // calculate the arc length when sweep through an angle
        float arcLengthToAngle(float angle) {
            float radical = sqrt(angle * angle + 1.0f);
            return _Pitch * 0.5f * (angle * radical + log(angle + radical));
        }

        void vert(inout appdata_full v) {
            // angle when rolled
            float fromStart = v.vertex.z * _AnglePerUnit;

            // angle from the origin to the angle when rolled
            float fromOrigin = _StartAngle - fromStart;

            // arc length from spiral origin to current vertex's point when rolled
            float lengthToHere = arcLengthToAngle(fromOrigin);
            // arc length from spiral origin after sweeping through the starting angle
            float lengthToStart = arcLengthToAngle(_StartAngle);

            // not rolled portion
            if (fromStart < _UnrolledAngle) {
                // arc length spiral origin to split
                float lengthToSplit = arcLengthToAngle(_StartAngle - _UnrolledAngle);

                // difference in arc length is the length from the current vertex to split point
                v.vertex.z = lengthToSplit - lengthToHere;

                // not in roll so stay base position
                v.vertex.y = 0.0f;
                v.normal = float3(0, 1, 0);
            }
            // rolled portion
            else {
                // radius from the spiral origin to the rolled and unrolled split point
                float radiusAtSplit = _Pitch * (_StartAngle - _UnrolledAngle);
                // radius from the spiral origin vertex coords
                float radius = _Pitch * fromOrigin;

                // how far from the rolled and unrolled split point
                float shifted = fromStart - _UnrolledAngle;

                // coordinates in the roll
                v.vertex.y = radiusAtSplit - cos(shifted) * radius;
                v.vertex.z = sin(shifted) * radius;

                v.normal = float3(0, cos(shifted), -sin(shifted));
            }
        }


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // apply albedo texture
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
