using System;
using System.Collections;
using System.Runtime.InteropServices;

#if !UNITY_EDITOR && !UNITY_STANDALONE
public struct Vector3
{
    public float x;
    public float y;
    public float z;
    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public override string ToString()
    {
        return string.Format("x:{0}, y:{1}, z:{2}", x, y, z);
    }
}

public struct Quaternion
{
    public float x, y, z, w;
    public override string ToString()
    {
        return string.Format("[x:{0}, y:{1}, z:{2}, w:{3}]", x, y, z, w);
    }
}

public class Debug
{
    public static void Log(object log)
    {
        Console.WriteLine(log.ToString());
    }
   
    
}

#endif

public static class PSMove 
{

    public static Vector3 RawPosition { get; private set; }
    public static Quaternion RawOrientation { get; private set; }

    static PSMove()
    {
        MoveFramework_CS.MoveWrapper.init();
        MoveFramework_CS.MoveWrapper.subscribeMoveUpdate(
            OnMoveUpdate,
            OnKeyDown,
            OnKeyUp,
            OnNavUpdate,
            OnNavKeyDown,
            OnNavKeyUp);

    }

    private static void OnMoveUpdate(int id, [MarshalAs(UnmanagedType.Struct)] MoveFramework_CS.MoveWrapper.Vector3 position, [MarshalAs(UnmanagedType.Struct)] MoveFramework_CS.MoveWrapper.Quaternion orientation, int trigger)
    {
        RawPosition = PS2UVec(position);
    }
    private static void OnKeyDown(int id, int keyCode)
    {

    }
    private static void OnKeyUp(int id, int keyCode)
    {

    }
    private static void OnNavUpdate(int id, int trigger1, int trigger2, int stickX, int stickY)
    {

    }
    private static void OnNavKeyDown(int id, int keyCode)
    {

    }
    private static void OnNavKeyUp(int id, int keyCode)
    {

    }

    private static Vector3 PS2UVec(MoveFramework_CS.MoveWrapper.Vector3 input)
    {
        return new Vector3(input.x, input.y, input.z);
    }
    private static void PS2UVec(MoveFramework_CS.MoveWrapper.Vector3 input, ref Vector3 output)
    {
        output.x = input.x;
        output.y = input.y;
        output.z = input.z;
    }
    
}
