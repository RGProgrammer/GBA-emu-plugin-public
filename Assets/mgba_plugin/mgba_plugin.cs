using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class mgba_plugin 
{
    
    [DllImport("mgba_plugin", EntryPoint = "createInstance")]
    public static extern int createEmuIntstance(IntPtr romBuffer,int romBufferSize, 
                                                IntPtr biosBuffer,int biosSize,string savePath);

    [DllImport("mgba_plugin", EntryPoint = "releaseInstance")]
    public static extern int releaseEmuInstance(int instanceID);

    [DllImport("mgba_plugin", EntryPoint = "releaseAll")]
    public static extern void releaseAllInstances();

    [DllImport("mgba_plugin", EntryPoint = "reset")]
    public static extern int resetEmu(int instanceID);

    [DllImport("mgba_plugin", EntryPoint = "updateSingleFrame")]
    public static extern int updateSingleFrame(int instanceID);

    [DllImport("mgba_plugin", EntryPoint = "updateFrames")]
    public static extern int updateFrames(int instanceID, float deltaTime);

    [DllImport("mgba_plugin", EntryPoint = "pushKeys")]
    public static extern int pushKeys(int instanceID, int Keys);

    [DllImport("mgba_plugin", EntryPoint = "getCurrentFramePixels")]
    public static extern IntPtr copyPixelsBuffer(int instanceID);
    
    [DllImport("mgba_plugin", EntryPoint = "getCurrentFramePixelsAt")]
    public static extern uint getCurrentFramePixelsAt(int instanceID, uint index);

     [DllImport("mgba_plugin", EntryPoint = "getCurrentFramePixel2At")]
    public static extern uint getCurrentFramePixel2At(int instanceID, uint row , uint column,bool vertical);

  
    [DllImport("mgba_plugin", EntryPoint = "getNumSamplesAvailable")]
    public static extern int getNumAudioSamples(int instanceID); // return Number of samples produced by the Emu


    [DllImport("mgba_plugin", EntryPoint = "getCurrentFrameSoundSamples")]
    public static extern unsafe int getAudioSamplesBuffer(int instanceID,
                                                    int count, float* outBuffer);

    [DllImport("mgba_plugin", EntryPoint = "getCurrentFrameSoundSamples")]
    public static extern int readAudioData(int instanceID, int count, IntPtr outBuffer);

    [DllImport("mgba_plugin", EntryPoint = "getOneSample")]
    public static extern float readOneSampleAndIncrement(int instanceID);


    [DllImport("mgba_plugin", EntryPoint = "getAudioBufferSizeInSamples")]
    public static extern int getAudioBufferSizeInSamples(int instanceID);

    [DllImport("mgba_plugin", EntryPoint = "getAudioBufferSizeInBytes")]
    public static extern int getAudioBufferSizeInBytes(int instanceID); 
    
    [DllImport("mgba_plugin", EntryPoint = "clearAudioData")]
    public static extern void clearAudioData(int instanceID); 

    [DllImport("mgba_plugin", EntryPoint = "getSampleRate")]
    public static extern int getSampleRate(int instanceID);

    



}
