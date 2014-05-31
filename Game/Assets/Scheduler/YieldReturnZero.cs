using System;
using System.Collections.Generic;

public class YieldReturnZero : IYieldInstruction
{
    private static YieldReturnZero mInstance = null;
    public static YieldReturnZero Instance
    {
        get 
        {
            if (mInstance == null)
            {
                mInstance = new YieldReturnZero();
            }
            return mInstance; 
        }
    }

    private YieldReturnZero()
    {
    }

    public bool IsReady()
    {
        return true;
    }
}

