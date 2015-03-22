/*
 *  NumberCruncher.cp
 *  NumberCruncher
 *
 *  Created by Chris Mortonson on 3/19/15.
 *  Copyright (c) 2015 Deep Sea Studios. All rights reserved.
 *
 */

#include "NumberCruncher.h"
#include "NumberCruncherPriv.h"


void Mult(unsigned length, float* aArray, float* bArray, float* outArray)
{
    for (auto i = 0; i < length; ++i)
    {
        outArray[i] = aArray[i] * bArray[i];
    }
}