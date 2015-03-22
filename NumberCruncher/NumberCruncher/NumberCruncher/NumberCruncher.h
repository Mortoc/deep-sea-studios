/*
 *  NumberCruncher.h
 *  NumberCruncher
 *
 *  Created by Chris Mortonson on 3/19/15.
 *  Copyright (c) 2015 Deep Sea Studios. All rights reserved.
 *
 */



// External interface to the NumberCruncher
extern "C"
{
    #pragma GCC visibility push(default)
    
    void Mult(unsigned length, float* aArray, float* bArray, float* outArray);
    
    #pragma GCC visibility pop
}
