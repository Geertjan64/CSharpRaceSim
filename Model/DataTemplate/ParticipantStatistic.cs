using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface ParticipantStatistic
    {
        public string Name { get; set; }


        public abstract void Add<T>( List<T> lijst  ) where T : class, ParticipantStatistic ;
    }
}
 