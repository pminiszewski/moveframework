#if !UNITY_EDITOR && !UNITY_STANDALONE
#define NOT_UNITY
#endif

using System;
using System.Collections;
using System.Runtime.InteropServices;



#if NOT_UNITY

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
    public Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
    public override string ToString()
    {
        return string.Format("[x:{0}, y:{1}, z:{2}, w:{3}]", x, y, z, w);
    }
}

public class Debug
{
    public static string From = "";
    public static void Log(object log)
    {
        Console.WriteLine(From + ": "+ log.ToString());
    }
   
    
}

#endif

public class PSMove 
{
    
    private Vector3 _RawPosition;
    private Quaternion _RawOrientation;

#if NOT_UNITY
    private static PSMove _Inst;
    public static PSMove Instance
    {
        get
        {
            if(_Inst == null)
            {
                _Inst = new PSMove();
            }
            return _Inst;
        }
    }
#endif 
    public static Vector3 RawPosition {
        get
        {
            return Instance._RawPosition;
        }
        internal set
        {
            Instance._RawPosition = value;
        }
    }

    public static Quaternion RawOrientation {
        get
        {
            return Instance._RawOrientation;
        }
        internal set
        {
            Instance._RawOrientation = value;
        }
    }
    public static long SendTime { get; internal set; }
    public static long ActualTime { get; internal set; }

    static PSMove()
    {

        


    }
    private PSMove()
    {

    }
   #if NOT_UNITY
    public static void InitDevice()
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
#endif
    private static void OnMoveUpdate(int id, [MarshalAs(UnmanagedType.Struct)] MoveFramework_CS.MoveWrapper.Vector3 position, [MarshalAs(UnmanagedType.Struct)] MoveFramework_CS.MoveWrapper.Quaternion orientation, int trigger)
    {
        RawPosition = PS2UVec(position);
        RawOrientation = PS2UQat( orientation);
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
    private static Quaternion PS2UQat(MoveFramework_CS.MoveWrapper.Quaternion input)
    {
        return new Quaternion(input.x, input.y, input.z, input.w);
    }
    private static void PS2UQuat(MoveFramework_CS.MoveWrapper.Quaternion input, ref Quaternion output)
    {
        output.x = input.x;
        output.y = input.y;
        output.z = input.z;
        output.w = input.w;
    }

    
}
