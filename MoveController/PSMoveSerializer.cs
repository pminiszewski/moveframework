using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MoveController
{
    public class PSMoveSerializer
    {
        public static byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            Serialize(ms, PSMove.RawPosition);

            Serialize(ms, PSMove.RawOrientation);
            ms.Close();
            return ms.ToArray();
        }

        public static void Deserialize(byte[] data)
        {
            Vector3 v = new Vector3();
            Quaternion q = new Quaternion();
            MemoryStream ms = new MemoryStream(data);
            
            Deserialize(ms, ref v); 
            PSMove.RawPosition = v;
            Deserialize(ms, ref q);
            PSMove.RawOrientation = q;
            ms.Close();
        }

        public static void Serialize(Stream inStr, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            inStr.Write(bytes, 0, bytes.Length);
        }
        public static void Serialize(Stream inStr, Vector3 value)
        {
            Serialize(inStr, value.x);
            Serialize(inStr, value.y);
            Serialize(inStr, value.z);
        }
        public static void Serialize(Stream inStr, Quaternion value)
        {
            Serialize(inStr, value.x);
            Serialize(inStr, value.y);
            Serialize(inStr, value.z);
            Serialize(inStr, value.w);
        }


        public static void Deserialize(Stream inStr, ref float value)
        {
            byte[] bytes = new byte[sizeof(float)];
            inStr.Read(bytes, 0, sizeof(float));
            value = BitConverter.ToSingle(bytes, 0);
        }
        public static void Deserialize(Stream inStr, ref Vector3 value)
        {
            Deserialize(inStr, ref value.x);
            Deserialize(inStr, ref value.y);
            Deserialize(inStr, ref value.z);
        }
        public static void Deserialize(Stream inStr, ref Quaternion value)
        {
            Deserialize(inStr, ref value.x);
            Deserialize(inStr, ref value.y);
            Deserialize(inStr, ref value.z);
            Deserialize(inStr, ref value.w);
        }
    }
}
