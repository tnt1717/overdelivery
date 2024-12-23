Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)  // ��ɫ��
        _OutlineWidth ("Outline Width", Range(0.0, 0.03)) = 0.005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        // �����Ⱦ Pass
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            fixed4 _OutlineColor;
            float _OutlineWidth;

            v2f vert(appdata v)
            {
                v2f o;
                // ����c�ط���������ƫ�Ɓ�Uչ���
                float3 norm = mul((float3x3)unity_ObjectToWorld, v.normal);
                o.pos = UnityObjectToClipPos(v.vertex + float4(norm * _OutlineWidth, 0.0));
                o.color = _OutlineColor;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;  // ����ɫ���ɫ
            }
            ENDCG
        }

        // ԭʼ���|��Ⱦ Pass
        Pass
        {
            Name "MainTexture"
            Tags { "LightMode" = "Always" }
            Cull Back
            ZWrite On

            CGPROGRAM
            #pragma vertex vertMain
            #pragma fragment fragMain
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;

            v2f vertMain(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 fragMain(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);  // ʹ�������ԭʼ�y��
            }
            ENDCG
        }
    }

    // ����ʹ���A�O�Ĳ��|
    FallBack "Diffuse"
}