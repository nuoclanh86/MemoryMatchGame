// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/UI/VFX_UI_Alp"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        [NoScaleOffset]_Base_Texture("Base_Texture", 2D) = "white" {}
        [Toggle]_Base_R("Base_R", Float) = 1
        _Base_Power("Base_Power", Float) = 1
        _Base_Tilling_Offset("Base_Tilling_Offset", Vector) = (1,1,0,0)
        _Base_Speed("Base_Speed", Vector) = (0,0,0,0)
        [HDR]_Base_Color("Base_Color", Color) = (1,1,1,1)
        [NoScaleOffset]_Mask_Texture("Mask_Texture", 2D) = "white" {}
        [Toggle]_Mask_R("Mask_R", Float) = 0
        _Mask_Power("Mask_Power", Float) = 1
        _Mask_Tilling_Offset("Mask_Tilling_Offset", Vector) = (1,1,0,0)
        _Mask_Speed("Mask_Speed", Vector) = (0,0,0,0)
        [NoScaleOffset]_Emission_Texture("Emission_Texture", 2D) = "white" {}
        [Toggle]_Emission_R("Emission_R", Float) = 0
        _Emission_Tilling_Offset("Emission_Tilling_Offset", Vector) = (1,1,0,0)
        _Emission_Speed("Emission_Speed", Vector) = (0,0,0,0)
        [HDR]_Emission_Color("Emission_Color", Color) = (0,0,0,0)
        _Emission_Power("Emission_Power", Float) = 1
        [NoScaleOffset]_Texture_Distortion("Distortion_Texture", 2D) = "white" {}
        _Distortion_Tilling_Offset("Distortion_Tilling_Offset", Vector) = (1,1,0,0)
        _DistortionSpeed_Power("Distortion Speed_Power", Vector) = (0,0,0,0)

    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	CompFront [_StencilComp]
        	PassFront [_StencilOp]
        	FailFront Keep
        	ZFailFront Keep
        	CompBack Always
        	PassBack Keep
        	FailBack Keep
        	ZFailBack Keep
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityShaderVariables.cginc"
            #define ASE_NEEDS_FRAG_COLOR


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            uniform float4 _Base_Color;
            uniform float _Base_R;
            uniform sampler2D _Base_Texture;
            uniform float4 _Base_Tilling_Offset;
            uniform float2 _Base_Speed;
            uniform sampler2D _Texture_Distortion;
            uniform float4 _Distortion_Tilling_Offset;
            uniform float4 _DistortionSpeed_Power;
            uniform float _Base_Power;
            uniform float _Mask_R;
            uniform sampler2D _Mask_Texture;
            uniform float4 _Mask_Tilling_Offset;
            uniform float2 _Mask_Speed;
            uniform float _Mask_Power;
            uniform float4 _Emission_Color;
            uniform float _Emission_R;
            uniform sampler2D _Emission_Texture;
            uniform float4 _Emission_Tilling_Offset;
            uniform float2 _Emission_Speed;
            uniform float _Emission_Power;

            
            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                

                v.vertex.xyz +=  float3( 0, 0, 0 ) ;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float2 appendResult30 = (float2(_Base_Tilling_Offset.x , _Base_Tilling_Offset.y));
                float2 appendResult31 = (float2(_Base_Tilling_Offset.z , _Base_Tilling_Offset.w));
                float2 texCoord26 = IN.texcoord.xy * appendResult30 + ( appendResult31 + ( _Time.y * _Base_Speed ) );
                float2 appendResult73 = (float2(_Distortion_Tilling_Offset.x , _Distortion_Tilling_Offset.y));
                float2 appendResult67 = (float2(_Distortion_Tilling_Offset.z , _Distortion_Tilling_Offset.w));
                float2 appendResult84 = (float2(_DistortionSpeed_Power.x , _DistortionSpeed_Power.y));
                float2 texCoord69 = IN.texcoord.xy * appendResult73 + ( appendResult67 + ( _Time.y * appendResult84 ) );
                float4 break74 = tex2D( _Texture_Distortion, texCoord69 );
                float4 appendResult75 = (float4(break74.r , break74.g , 0.0 , 0.0));
                float2 appendResult85 = (float2(_DistortionSpeed_Power.z , _DistortionSpeed_Power.w));
                float4 tex2DNode14 = tex2D( _Base_Texture, ( float4( texCoord26, 0.0 , 0.0 ) - ( appendResult75 * float4( appendResult85, 0.0 , 0.0 ) ) ).xy );
                float4 temp_cast_3 = (saturate( ( tex2DNode14.r * _Base_Power ) )).xxxx;
                float2 appendResult54 = (float2(_Mask_Tilling_Offset.x , _Mask_Tilling_Offset.y));
                float2 appendResult56 = (float2(_Mask_Tilling_Offset.z , _Mask_Tilling_Offset.w));
                float2 texCoord57 = IN.texcoord.xy * appendResult54 + ( appendResult56 + ( _Time.y * _Mask_Speed ) );
                float4 tex2DNode41 = tex2D( _Mask_Texture, texCoord57 );
                float4 temp_cast_4 = (saturate( ( tex2DNode41.r * _Mask_Power ) )).xxxx;
                float2 appendResult101 = (float2(_Emission_Tilling_Offset.x , _Emission_Tilling_Offset.y));
                float2 appendResult102 = (float2(_Emission_Tilling_Offset.z , _Emission_Tilling_Offset.w));
                float2 texCoord100 = IN.texcoord.xy * appendResult101 + ( appendResult102 + ( _Time.y * _Emission_Speed ) );
                float4 tex2DNode111 = tex2D( _Emission_Texture, texCoord100 );
                float4 temp_cast_5 = (saturate( ( tex2DNode111.r * _Emission_Power ) )).xxxx;
                

                half4 color = ( IN.color * ( _Base_Color * ( ( (( _Base_R )?( temp_cast_3 ):( saturate( ( tex2DNode14 * _Base_Power ) ) )) * (( _Mask_R )?( temp_cast_4 ):( saturate( ( tex2DNode41 * _Mask_Power ) ) )) ) + ( _Emission_Color * (( _Emission_R )?( temp_cast_5 ):( saturate( ( tex2DNode111 * _Emission_Power ) ) )) ) ) ) );

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.CommentaryNode;94;-2707.026,-460.1118;Inherit;False;1565.883;540.0626;Mask;15;95;51;41;92;91;93;60;55;53;59;58;56;54;57;96;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;87;-2756.602,-1133.745;Inherit;False;1655.975;566.261;Base;16;89;29;26;14;36;88;90;78;30;39;38;40;31;37;97;98;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;86;-4552.464,-394.1178;Inherit;False;1630.394;501.0273;Distortion;14;76;75;74;65;69;68;73;82;85;84;67;72;71;66;;1,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-476.1368,-222.643;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-739.0305,-181.7067;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;79;-696.814,-693.0703;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-358.814,-356.0703;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-44.6649,-90.04288;Float;False;True;-1;2;ASEMaterialInspector;0;3;VFX/UI/VFX_UI_Alp;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;3;1;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;True;True;2;False;;True;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;True;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;True;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-3977.642,-165.8565;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-4105.642,-149.8565;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;72;-4273.64,-164.8565;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;-4102.867,-250.1176;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;84;-4249.464,-92.73594;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;85;-4246.464,8.263988;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;82;-4502.465,-88.73594;Inherit;False;Property;_DistortionSpeed_Power;Distortion Speed_Power;19;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;73;-4103.867,-344.1177;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;68;-4340.864,-339.1177;Inherit;False;Property;_Distortion_Tilling_Offset;Distortion_Tilling_Offset;18;0;Create;True;0;0;0;False;0;False;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;-3876.396,-330.5865;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;74;-3397.982,-330.4349;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;75;-3285.11,-328.5713;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-3088.388,-178.558;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-2381.377,-895.4841;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-2506.604,-979.7451;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;40;-2656.378,-809.4841;Float;False;Property;_Base_Speed;Base_Speed;4;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-2509.378,-879.4841;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;39;-2677.378,-894.4841;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-2415.603,-1083.745;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;78;-2040.213,-1003.927;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1783.02,-843.4755;Inherit;False;Property;_Base_Power;Base_Power;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1611.02,-1040.475;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;14;-1895.946,-1026.864;Inherit;True;Property;_Base_Texture;Base_Texture;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-2260.065,-1003.359;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;29;-2706.602,-1079.745;Inherit;False;Property;_Base_Tilling_Offset;Base_Tilling_Offset;3;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;65;-3676.434,-332.9052;Inherit;True;Property;_Texture_Distortion;Distortion_Texture;17;1;[NoScaleOffset];Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-696.4941,-440.8665;Inherit;False;Property;_Base_Color;Base_Color;5;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-1611.141,-942.4755;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;36;-1317.624,-999.2415;Inherit;False;Property;_Base_R;Base_R;1;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;97;-1472.007,-1038.581;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;98;-1473.007,-937.5809;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-546.2363,-59.23126;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-2161.27,-396.8592;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;54;-2408.27,-403.8592;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-2410.27,-301.8592;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-2416.796,-203.0492;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;59;-2584.795,-218.0494;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;53;-2564.795,-133.0492;Float;False;Property;_Mask_Speed;Mask_Speed;10;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-2276.796,-309.0495;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;60;-2657.026,-402.8925;Inherit;False;Property;_Mask_Tilling_Offset;Mask_Tilling_Offset;9;0;Create;True;0;0;0;False;0;False;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;93;-1849.716,-227.2908;Inherit;False;Property;_Mask_Power;Mask_Power;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1666.37,-404.8332;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-1662.37,-306.8333;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1951.243,-410.1118;Inherit;True;Property;_Mask_Texture;Mask_Texture;6;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;51;-1374.143,-391.4661;Inherit;False;Property;_Mask_R;Mask_R;7;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;95;-1521.54,-407.7541;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;96;-1523.54,-305.7541;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;-1822.236,225.0209;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;101;-2069.236,218.0209;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;102;-2071.236,320.0209;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-2077.761,418.8309;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;104;-2245.76,403.8307;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;-1937.761,312.8306;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-1327.336,217.0469;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-1323.336,315.0468;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;113;-1182.506,214.126;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;114;-1184.506,316.126;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;112;-1035.109,230.414;Inherit;False;Property;_Emission_R;Emission_R;12;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;111;-1612.209,211.7683;Inherit;True;Property;_Emission_Texture;Emission_Texture;11;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;107;-2319.991,216.9876;Inherit;False;Property;_Emission_Tilling_Offset;Emission_Tilling_Offset;13;0;Create;True;0;0;0;False;0;False;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;105;-2225.76,488.8309;Float;False;Property;_Emission_Speed;Emission_Speed;14;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-627.9934,119.5025;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;119;-906.9934,34.5025;Inherit;False;Property;_Emission_Color;Emission_Color;15;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;108;-1511.682,398.5893;Inherit;False;Property;_Emission_Power;Emission_Power;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
WireConnection;35;0;6;0
WireConnection;35;1;99;0
WireConnection;45;0;36;0
WireConnection;45;1;51;0
WireConnection;80;0;79;0
WireConnection;80;1;35;0
WireConnection;0;0;80;0
WireConnection;66;0;67;0
WireConnection;66;1;71;0
WireConnection;71;0;72;0
WireConnection;71;1;84;0
WireConnection;67;0;68;3
WireConnection;67;1;68;4
WireConnection;84;0;82;1
WireConnection;84;1;82;2
WireConnection;85;0;82;3
WireConnection;85;1;82;4
WireConnection;73;0;68;1
WireConnection;73;1;68;2
WireConnection;69;0;73;0
WireConnection;69;1;66;0
WireConnection;74;0;65;0
WireConnection;75;0;74;0
WireConnection;75;1;74;1
WireConnection;76;0;75;0
WireConnection;76;1;85;0
WireConnection;37;0;31;0
WireConnection;37;1;38;0
WireConnection;31;0;29;3
WireConnection;31;1;29;4
WireConnection;38;0;39;0
WireConnection;38;1;40;0
WireConnection;30;0;29;1
WireConnection;30;1;29;2
WireConnection;78;0;26;0
WireConnection;78;1;76;0
WireConnection;88;0;14;0
WireConnection;88;1;90;0
WireConnection;14;1;78;0
WireConnection;26;0;30;0
WireConnection;26;1;37;0
WireConnection;65;1;69;0
WireConnection;89;0;14;1
WireConnection;89;1;90;0
WireConnection;36;0;97;0
WireConnection;36;1;98;0
WireConnection;97;0;88;0
WireConnection;98;0;89;0
WireConnection;99;0;45;0
WireConnection;99;1;117;0
WireConnection;57;0;54;0
WireConnection;57;1;55;0
WireConnection;54;0;60;1
WireConnection;54;1;60;2
WireConnection;56;0;60;3
WireConnection;56;1;60;4
WireConnection;58;0;59;0
WireConnection;58;1;53;0
WireConnection;55;0;56;0
WireConnection;55;1;58;0
WireConnection;91;0;41;0
WireConnection;91;1;93;0
WireConnection;92;0;41;1
WireConnection;92;1;93;0
WireConnection;41;1;57;0
WireConnection;51;0;95;0
WireConnection;51;1;96;0
WireConnection;95;0;91;0
WireConnection;96;0;92;0
WireConnection;100;0;101;0
WireConnection;100;1;106;0
WireConnection;101;0;107;1
WireConnection;101;1;107;2
WireConnection;102;0;107;3
WireConnection;102;1;107;4
WireConnection;103;0;104;0
WireConnection;103;1;105;0
WireConnection;106;0;102;0
WireConnection;106;1;103;0
WireConnection;109;0;111;0
WireConnection;109;1;108;0
WireConnection;110;0;111;1
WireConnection;110;1;108;0
WireConnection;113;0;109;0
WireConnection;114;0;110;0
WireConnection;112;0;113;0
WireConnection;112;1;114;0
WireConnection;111;1;100;0
WireConnection;117;0;119;0
WireConnection;117;1;112;0
ASEEND*/
//CHKSM=083936B244151B80E44845D81B717F6CE76C5DA6