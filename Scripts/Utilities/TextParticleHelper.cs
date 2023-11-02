using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class TextParticleHelper
    {
        public static Texture _texture;
        private static char[] _chars = " !*,-./:;<=>?ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_$+0123456789".ToCharArray();
        private static Dictionary<char, Vector2> _charsDict;
        private static Dictionary<string, (Vector4, Vector4)> _cache = new Dictionary<string, (Vector4, Vector4)>();

        static TextParticleHelper()
        {
            _charsDict = new Dictionary<char, Vector2>();

            for (int i = 0; i < _chars.Length; i++)
            {
                var c = char.ToLowerInvariant(_chars[i]);

                if (_charsDict.ContainsKey(c))
                    continue;

                var uv = new Vector2(i % 10, 9 - i / 10);

                _charsDict.Add(c, uv);
            }
        }

        public static void EmitWords(this ParticleSystem particleSystem, string msg, Color color = default)
        {
            int messageLenght = PackSentence(msg, out (Vector4, Vector4) customData);

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
            {
                startColor = color == default ? Color.black : color,
                applyShapeToPosition = true,
                startSize3D = new Vector3(messageLenght, 1, 1) * particleSystem.main.startSize.constant
            };

            particleSystem.Emit(emitParams, 1);

            List<Vector4> customDataList = new List<Vector4>();

            particleSystem.GetCustomParticleData(customDataList, ParticleSystemCustomData.Custom1);
            customDataList[^1] = customData.Item1;
            particleSystem.SetCustomParticleData(customDataList, ParticleSystemCustomData.Custom1);

            particleSystem.GetCustomParticleData(customDataList, ParticleSystemCustomData.Custom2);
            customDataList[^1] = customData.Item2;
            particleSystem.SetCustomParticleData(customDataList, ParticleSystemCustomData.Custom2);
        }

        private static int PackSentence(string msg, out (Vector4, Vector4) target)
        {
            if (msg.Length > 23)
                msg = msg.Remove(23);

            if (_cache.TryGetValue(msg, out target))
                return msg.Length;

            Vector2[] texCords = new Vector2[24];

            for (int i = 0; i < msg.Length; i++)
                texCords[i] = GetTextureCoordinates(msg[i]);

            texCords[^1] = new Vector2(0, msg.Length);

            target = (CreateCustomData(texCords), CreateCustomData(texCords, 12));

            _cache.Add(msg, target);

            return msg.Length;
        }

        private static Vector2 GetTextureCoordinates(char c)
        {
            c = char.ToLowerInvariant(c);

            if (_charsDict.TryGetValue(c, out Vector2 uvCoord))
                return uvCoord;

            return Vector2.zero;
        }

        private static Vector4 CreateCustomData(Vector2[] texCoords, int offset = 0)
        {
            Vector4 data = Vector4.zero;

            for (int i = 0; i < 4; i++)
            {
                Vector2[] vecs = new Vector2[3];
                int ind = i * 3 + offset;

                for (int j = 0; j < 3; j++)
                    if (ind + j < texCoords.Length)
                        vecs[j] = texCoords[ind + j];

                data[i] = PackFloat(vecs);
            }

            return data;
        }
        
        private static float PackFloat(Vector2[] vecs)
        {
            if (vecs == null || vecs.Length == 0)
                return 0;

            float result = 0;
            int multiplier = 1;

            for (int i = vecs.Length - 1; i >= 0; --i, multiplier *= 100)
                result += vecs[i].y * multiplier + vecs[i].x * (10 * multiplier);

            return result;
        }
    }
}