using System.Collections.Generic;

namespace Adtones.Rollups.Data.DataObjects
{
    public class SpikePlayLengthsDao : LeveledStatsObjectDaoBase
    {
        public SpikePlayLengthsDao()
        {
            Values = new List<long>();
        }
        public List<long> Values { get; set; }
    }
}