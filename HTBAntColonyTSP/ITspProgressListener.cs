using System;

namespace HTBAntColonyTSP
{
    public interface ITspProgressListener
    {
        void SetTotalTime(TimeSpan ts);
        void SetBestTourValue(double value);
    }
}
