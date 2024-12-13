using UnityEngine;

public static class AEMDebug
{
    public delegate void Function(params object[] args);
    
    public static float getExcecuteTime(Function f,params object[] args)
    {
        float t = Time.timeSinceLevelLoad;
        f(args);
        return Time.timeSinceLevelLoad - t;
    }
}

