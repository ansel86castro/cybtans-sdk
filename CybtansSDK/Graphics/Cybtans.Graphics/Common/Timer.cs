using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Cybtans.Graphics.Common
{
    ///// <summary>
    ///// Hight Resolution Timer in Seconds
    ///// </summary>
    //public sealed class Timer
    //{
    //    #region Members       
       
    //    private  static bool usingQPF = false;
    //    private  bool timerStopped = true;
    //    private  static long QPFTicksPerSec = 0;
    //    private  long stopTime = 0;
    //    private  long m_llLastElapsedTime = 0;
    //    private  long m_llBaseTime = 0;
    //    private  double m_fLastElapsedTime = 0.0;
    //    private  double m_fBaseTime = 0.0;
    //    private  double m_fStopTime = 0.0;
    //    private float elapseTime;
    //    #endregion

    //    static Timer()
    //    {
    //        // Use QueryPerformanceFrequency() to get frequency of timer.  If QPF is
    //        // not supported, we will timeGetTime() which returns milliseconds.
    //        long qwTicksPerSec = 0;
    //        usingQPF = QueryPerformanceFrequency(ref qwTicksPerSec);
    //        if (usingQPF)
    //            QPFTicksPerSec = qwTicksPerSec;
    //    }

    //    public Timer()
    //    {         
           
    //    }        

    //    #region IMPORT DLL

    //    [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
    //    [DllImport("kernel32")]
    //    private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);
    //    [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
    //    [DllImport("kernel32")]
    //    private static extern bool QueryPerformanceCounter(ref long PerformanceCount);
    //    [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
    //    [DllImport("winmm.dll")]
    //    private static extern int timeGetTime();

    //    #endregion

    //    public static float Now()
    //    {
    //        long qwTime = 0;
    //        double time;
    //        if (usingQPF)
    //        {
    //            QueryPerformanceCounter(ref qwTime);
    //            time = qwTime / (double)QPFTicksPerSec;               
    //        }
    //        else            
    //            time = timeGetTime() * 0.001;
            
    //        Environment.TickCount

    //        return (float)time;
    //    }

    //    public void Start()
    //    {
    //        if (usingQPF)
    //        {
    //            long qwTime = 0;
    //            QueryPerformanceCounter(ref qwTime);

    //            if (timerStopped)
    //                m_llBaseTime += qwTime - stopTime;
    //            stopTime = 0;
    //            m_llLastElapsedTime = qwTime;              

    //        }
    //        else
    //        {
    //            double time = timeGetTime();
    //            if (timerStopped)
    //                m_fBaseTime += time - m_fStopTime;
    //            m_fStopTime = 0.0f;
    //            m_fLastElapsedTime = time;                
    //        }

    //        timerStopped = false;
    //    }

    //    public void Reset()
    //    {
    //        long qwTime = 0;
    //        if (usingQPF)
    //            QueryPerformanceCounter(ref qwTime);
    //        else
    //            qwTime = timeGetTime();

    //        m_llBaseTime = qwTime;
    //        m_llLastElapsedTime = qwTime;
    //        stopTime = 0;
    //        timerStopped = false;

    //    }
        
    //    public void Reset(float baseTime)
    //    {
    //        long qwTime = 0;
    //        if (usingQPF)
    //        {
    //            QueryPerformanceCounter(ref qwTime);
    //            m_llBaseTime = qwTime - (long)(baseTime * (double)QPFTicksPerSec);
    //            m_llLastElapsedTime = m_llBaseTime;
    //            stopTime = 0;

    //        }
    //        else
    //        {
    //            double time = timeGetTime() * 0.001;
    //            m_fBaseTime = time - baseTime;
    //            m_fLastElapsedTime = m_fBaseTime;
    //            m_fStopTime = 0;               
    //        }         
    //        timerStopped = false;
    //    }
       
    //    public void Resume()
    //    {
    //        timerStopped = false;
    //    }

    //    public void Stop()
    //    {            
    //        if (!timerStopped)
    //        {
    //            if (usingQPF)
    //            {
    //                long qwTime = 0;
    //                QueryPerformanceCounter(ref qwTime);
    //                stopTime = qwTime;
    //                m_llLastElapsedTime = qwTime;                  
    //            }
    //            else
    //            {
    //                double time = timeGetTime()*0.001;
    //                m_fStopTime = time;
    //                m_fLastElapsedTime = time;                   
    //            }
    //            timerStopped = true;
    //        }
    //    }

    //    public float Time
    //    {
    //        get
    //        {
    //            if (usingQPF)
    //            {
    //                long qwTime = 0;
    //                QueryPerformanceCounter(ref qwTime);
    //                double fAppTime = (double)(qwTime - m_llBaseTime) / (double)QPFTicksPerSec;
    //                return (float)fAppTime;
    //            }
    //            else
    //            {
    //                double time = timeGetTime() * 0.001;
    //                return (float)(time - m_fBaseTime);
    //            }
    //        }
    //    }

    //    public float Elapsed()
    //    {
    //        double time;
    //        double fElapsedTime;
    //        long qwTime = 0;

    //        if (usingQPF)
    //        {
    //            QueryPerformanceCounter(ref qwTime);           
    //            fElapsedTime = (double)(qwTime - m_llLastElapsedTime) / (double)QPFTicksPerSec;
    //            m_llLastElapsedTime = qwTime;
    //        }
    //        else
    //        {
    //            time = timeGetTime() * 0.001;
    //            fElapsedTime = (double)(time - m_fLastElapsedTime);
    //            m_fLastElapsedTime = time;
    //        }
    //        elapseTime = (float)fElapsedTime;
    //        return (float)fElapsedTime;

    //    }

    //    public float ComputedElapsedTime
    //    {
    //        get
    //        {
    //            return elapseTime;
    //        }
    //    }
    //}

    /// <summary>
    /// Hight Resolution Timer in Seconds
    /// </summary>
    public sealed class Timer
    {
        #region Members
       
        private bool _timerStopped = true;              
        private double _mFLastElapsedTime = 0.0;
        private double _mFBaseTime = 0.0;
        private double _mFStopTime = 0.0;
        private float _elapseTime;
        #endregion

        static Timer()
        {
            // Use QueryPerformanceFrequency() to get frequency of timer.  If QPF is
            // not supported, we will timeGetTime() which returns milliseconds.           
        }

        public Timer()
        {

        }

        #region IMPORT DLL

        //[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        //[DllImport("kernel32")]
        //private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);
        //[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        //[DllImport("kernel32")]
        //private static extern bool QueryPerformanceCounter(ref long PerformanceCount);
        //[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        //[DllImport("winmm.dll")]
        //private static extern int timeGetTime();

        #endregion

        public static float Now()
        {         
            return (float)Environment.TickCount * 0.001f;            
        }

        public void Start()
        {
            //if (usingQPF)
            //{
            //    long qwTime = 0;
            //    QueryPerformanceCounter(ref qwTime);

            //    if (timerStopped)
            //        m_llBaseTime += qwTime - stopTime;
            //    stopTime = 0;
            //    m_llLastElapsedTime = qwTime;

            //}
            //else
            //{
                double time = Environment.TickCount * 0.001;
                if (_timerStopped)
                    _mFBaseTime += time - _mFStopTime;
                _mFStopTime = 0.0f;
                _mFLastElapsedTime = time;
            //}

            _timerStopped = false;
        }

        public void Reset()
        {
            //long qwTime = 0;
            //if (usingQPF)
            //    QueryPerformanceCounter(ref qwTime);
            //else
                //qwTime = Environment.TickCount;

            _mFBaseTime = Environment.TickCount * 0.001;
            _mFLastElapsedTime = _mFBaseTime;
            _mFStopTime = 0;            
            _elapseTime = 0;
            _timerStopped = false;

        }

        //public void Reset(float baseTime)
        //{
        //    //long qwTime = 0;
        //    //if (usingQPF)
        //    //{
        //    //    QueryPerformanceCounter(ref qwTime);
        //    //    m_llBaseTime = qwTime - (long)(baseTime * (double)QPFTicksPerSec);
        //    //    m_llLastElapsedTime = m_llBaseTime;
        //    //    stopTime = 0;

        //    //}
        //    //else
        //    //{
        //        double time = Environment.TickCount * 0.001;
        //        m_fBaseTime = time - baseTime;
        //        m_fLastElapsedTime = m_fBaseTime;
        //        m_fStopTime = 0;
        //        elapseTime = 0;
        //    //}
        //    timerStopped = false;
        //}

        public void Resume()
        {
            _timerStopped = false;
        }

        public void Stop()
        {
            if (!_timerStopped)
            {
                //if (usingQPF)
                //{
                //    long qwTime = 0;
                //    QueryPerformanceCounter(ref qwTime);
                //    stopTime = qwTime;
                //    m_llLastElapsedTime = qwTime;
                //}
                //else
                //{
                    double time = Environment.TickCount * 0.001;
                    _mFStopTime = time;
                    _mFLastElapsedTime = time;
                //}
                _timerStopped = true;
            }
        }

        public float Time
        {
            get
            {
                //if (usingQPF)
                //{
                //    long qwTime = 0;
                //    QueryPerformanceCounter(ref qwTime);
                //    double fAppTime = (double)(qwTime - m_llBaseTime) / (double)QPFTicksPerSec;
                //    return (float)fAppTime;
                //}
                //else
                //{
                    double time = Environment.TickCount * 0.001;
                    return (float)(time - _mFBaseTime);
                //}
            }
        }

        public float Elapsed()
        {
            double time;
            double fElapsedTime;
            //long qwTime = 0;

            //if (usingQPF)
            //{
            //    QueryPerformanceCounter(ref qwTime);
            //    fElapsedTime = (double)(qwTime - m_llLastElapsedTime) / (double)QPFTicksPerSec;
            //    m_llLastElapsedTime = qwTime;
            //}
            //else
            //{
                time = Environment.TickCount * 0.001;
                fElapsedTime = time - _mFLastElapsedTime;
                _mFLastElapsedTime = time;
            //}
            _elapseTime = (float)fElapsedTime;
            return _elapseTime;

        }

        public float ComputedElapsedTime
        {
            get
            {
                return _elapseTime;
            }
        }
    }
}
