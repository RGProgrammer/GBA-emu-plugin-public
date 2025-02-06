using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class GBA_Component : MonoBehaviour
{

    public enum KEYS
    {
        A = (1 << 0),
        B = (1 << 1),
        SELECT = (1 << 2),
        START = (1 << 3),
        RIGHT = (1 << 4),
        LEFT = (1 << 5),
        UP = (1 << 6),
        DOWN = (1 << 7),
        R = (1 << 8),
        L = (1 << 9),
    }


    [SerializeField]
    private AssetGameBoy rom;
    [SerializeField]
    private AssetGameBoy bios;
    [SerializeField]
    private Material m_Material;


    public bool isInited
    {
        get; private set;
    } = false;

    private AudioSource m_AudioSource;
    private AudioClip m_AudioClip;

    private int m_EmuInstanceID = -1;
    private const int CHANNELS = 2;
    private const int WIDTH = 240;
    private const int HEIGHT = 160;
    private const int BYTES_PER_PIXEL = 4;
    private const float frameTime = 1.0f / 60.0f;
    private uint[] m_Pixels = new uint[WIDTH * HEIGHT];
    private float[] m_AudioData = null;
    private IntPtr m_RomDataAsPointer = IntPtr.Zero;
    private IntPtr m_BiosDataAsPointer = IntPtr.Zero;
    public Texture2D m_TargetTexture { get; private set; }

    private int m_Keys = 0;

    private float timer = 0.0f;
    private bool isPlaying = true;
    private bool isEmulating = false;
    private Thread thread;
    private String savePath;

    void Start()
    {
        if (!Init())
        {
            throw new Exception("Could not initialize Component");
        }

    }

    private bool Init()
    {
        if (rom == null)
        {
            CleanUp();
            throw new Exception("Must Select a Rom File");

        }
        if (m_Material == null)
        {
            CleanUp();
            throw new Exception("Must Assign Material in the inspector");
        }
        else
        {
            Debug.Log("Rom path = " + rom.assetPath);
        }

        var size = rom.readDataInMemory();


        if (size != 0)
        {
            if (m_RomDataAsPointer == IntPtr.Zero)
            {
                m_RomDataAsPointer = Marshal.AllocCoTaskMem(rom.GetSize());
            }
            if (m_RomDataAsPointer == IntPtr.Zero)
            {
                Debug.Log("could not allocate memory for rom");
                CleanUp();
                return isInited;
            }
            Marshal.Copy(rom.GetData(), 0, m_RomDataAsPointer, rom.GetSize());
        }
        if (bios != null)
        {
            size = bios.readDataInMemory();

            if (size != 0)
            {
                if (m_BiosDataAsPointer == IntPtr.Zero)
                {
                    m_BiosDataAsPointer = Marshal.AllocCoTaskMem(rom.GetSize());
                }
                if (m_BiosDataAsPointer == IntPtr.Zero)
                {
                    Debug.Log("could not allocate memory for bios");
                }
                Marshal.Copy(bios.GetData(), 0, m_BiosDataAsPointer, bios.GetSize());
            }

        }
        else
        {
            Debug.LogWarning("it is recommend to include a bios to avoid unexpected emulation issues");
        }
        savePath = Application.persistentDataPath + "/save/" ;

        m_EmuInstanceID = mgba_plugin.createEmuIntstance(m_RomDataAsPointer, rom.GetSize(), 
          m_BiosDataAsPointer, (bios != null) ? bios.GetSize() : 0, savePath); 
        if (m_EmuInstanceID == -1)
        {
            Debug.Log("could not create Emu instance from Rom " + rom.assetPath);
            CleanUp();
            return isInited;
        }
        else
        {

            Marshal.FreeCoTaskMem(m_RomDataAsPointer);
            Marshal.FreeCoTaskMem(m_BiosDataAsPointer);
            m_RomDataAsPointer = IntPtr.Zero;
            m_BiosDataAsPointer = IntPtr.Zero;

            Debug.Log("Emu instance created successfully " + m_EmuInstanceID);
        }
        m_TargetTexture = new Texture2D(WIDTH, HEIGHT, TextureFormat.RGBA32, false);
        m_Material.mainTexture = m_TargetTexture;


        if (m_AudioSource == null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        }
        if (m_AudioSource == null)
        {
            Debug.Log("no audio Source Found for GameObject " + name);
        }
        else
        {


            var sampleRate = mgba_plugin.getSampleRate(m_EmuInstanceID);
            var numsamples = mgba_plugin.getAudioBufferSizeInSamples(m_EmuInstanceID);
            if (numsamples > 0)
            {

                m_AudioClip = AudioClip.Create(name, numsamples,
                CHANNELS, sampleRate, false, OnPCMRead);
                m_AudioData = new float[numsamples];
                m_AudioSource.clip = m_AudioClip;
            }
            else
            {
                Debug.Log("samples size = 0");
            }
            m_AudioSource.playOnAwake = true;
            m_AudioSource.loop = false;
            m_AudioSource.Play();

        }
        isInited = true;
        isEmulating = true;
        isPlaying = true;
        timer = 0.0f;
        thread = new Thread(new ThreadStart(EmulationLoop));
        thread.Start();
        return isInited;
    }

    private void CleanUp()
    {
        if (thread != null)
        {
            isEmulating = false;
            thread.Join();
        }
        if (m_EmuInstanceID != -1)
        {
            mgba_plugin.releaseEmuInstance(m_EmuInstanceID);
            m_EmuInstanceID = -1;
        }

        if (m_RomDataAsPointer != IntPtr.Zero)
        {
            Marshal.FreeCoTaskMem(m_RomDataAsPointer);
            m_RomDataAsPointer = IntPtr.Zero;
        }
        if (m_BiosDataAsPointer != IntPtr.Zero)
        {
            Marshal.FreeCoTaskMem(m_BiosDataAsPointer);
            m_BiosDataAsPointer = IntPtr.Zero;
        }
        isInited = false;
    }


    private void OnPCMRead(float[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = mgba_plugin.readOneSampleAndIncrement(m_EmuInstanceID);
        }
    }


    private void Update()
    {
        if (m_EmuInstanceID == -1 || thread == null)
        {
            return;
        }
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (m_AudioSource != null && !m_AudioSource.isPlaying)
            {
                m_AudioClip.SetData(m_AudioData, 0);
                m_AudioSource.Play();
            }
        }
        mgba_plugin.pushKeys(m_EmuInstanceID, m_Keys);
        copyPixelData();


    }

    private void copyAudioData()
    {
        if (m_AudioData != null && m_AudioData.Length > 0)
        {
            for (int i = 0; i < m_AudioData.Length; i++)
            {
                m_AudioData[i] = mgba_plugin.readOneSampleAndIncrement(m_EmuInstanceID);
            }
        }
    }

    private void copyPixelData()
    {

        for (uint i = 0; i < m_Pixels.Length; i++)
        {
            m_Pixels[i] = mgba_plugin.getCurrentFramePixelsAt(m_EmuInstanceID, i);
        }
        m_TargetTexture.SetPixelData(m_Pixels, 0);
        m_TargetTexture.Apply();

    }

    public void PushKeys(int keys)
    {
  
        m_Keys = keys;
    }

    private void OnDestroy()
    {
        CleanUp();
    }



    public void resetEmulator()
    {
        mgba_plugin.resetEmu(m_EmuInstanceID);
    }


    private void EmulationLoop()
    {
        while (isEmulating)
        {
            if (isPlaying && timer >= frameTime)
            {
                mgba_plugin.updateSingleFrame(m_EmuInstanceID);
                copyAudioData();
                timer -= frameTime;
            }
        }
    }
}